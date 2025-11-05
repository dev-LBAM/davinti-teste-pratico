using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelephoneDiary.Models;
using TelephoneDiary.Data;
using TelephoneDiary.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelephoneDiary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController(AgendaContext context) : ControllerBase
    {
        private readonly AgendaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Contatos>>> GetContatos([FromQuery] string? pesquisa)
        {
            IQueryable<Contatos> query = _context.Contatos.Include(c => c.Telefones).AsQueryable();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                query = query.Where(
                    (Contatos c) => c.Nome.Contains(pesquisa)
                                  || c.Telefones.Any((Telefones t) => t.Numero.Contains(pesquisa))
                );
            }

            List<Contatos> contatos = await query.ToListAsync();

            if (contatos.Count == 0)
            {
                return NotFound("Nenhum contato encontrado.");
            }

            return contatos;
        }

        [HttpPost]
        public async Task<ActionResult<Contatos>> PostContato(ContatoRequestDTO.Create dto)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<object> errors = ModelState
                    .Where(ms => ms.Value != null && ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Campo = ms.Key,
                        Mensagens = ms.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? []
                    });

                return BadRequest(errors);
            }

            Contatos contato = new()
            {
                ID = Guid.NewGuid(),
                Nome = dto.Nome,
                Idade = dto.Idade,
                Telefones = dto.Telefones?.Select(
                    (TelefoneRequestDTO t) => new Telefones
                    {
                        ID = Guid.NewGuid(),
                        Numero = t.Numero,
                        IDContato = Guid.Empty // Será atualizado pelo EF ou no relacionamento
                    }
                ).ToList() ?? []
            };

            _context.Contatos.Add(contato);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContatos), new { id = contato.ID }, contato);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Contatos>> PutContato([FromRoute] Guid id, [FromBody] ContatoRequestDTO.Update dto)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<object> errors = ModelState
                    .Where(ms => ms.Value != null && ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Campo = ms.Key,
                        Mensagens = ms.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? []
                    });

                return BadRequest(errors);
            }

            Contatos? contato = await _context.Contatos
                .Include((Contatos c) => c.Telefones!)
                .FirstOrDefaultAsync((Contatos c) => c.ID == id);

            if (contato == null)
                return NotFound("Contato não encontrado.");

            if (!string.IsNullOrWhiteSpace(dto.Nome))
                contato.Nome = dto.Nome!;

            if (dto.Idade.HasValue)
                contato.Idade = dto.Idade.Value;

            if (dto.Telefones != null)
            {
                List<TelefoneRequestDTO> telefonesDto = dto.Telefones;

                List<Guid> idsDtoExistentes = telefonesDto
                    .Where((TelefoneRequestDTO t) => t.ID.HasValue)
                    .Select((TelefoneRequestDTO t) => t.ID!.Value)
                    .ToList();

                List<Telefones> telefonesParaRemover = contato.Telefones
                    .Where((Telefones t) => !idsDtoExistentes.Contains(t.ID))
                    .ToList();

                if (telefonesParaRemover.Count > 0)
                    _context.Telefones.RemoveRange(telefonesParaRemover);

                foreach (TelefoneRequestDTO tDto in telefonesDto.Where((TelefoneRequestDTO t) => t.ID.HasValue))
                {
                    Telefones? telefoneExistente = contato.Telefones.FirstOrDefault((Telefones t) => t.ID == tDto.ID!.Value);

                    if (telefoneExistente != null)
                    {
                        telefoneExistente.Numero = tDto.Numero!;
                    }
                }

                List<Telefones> telefonesNovos = telefonesDto
                    .Where((TelefoneRequestDTO t) => !t.ID.HasValue)
                    .Select((TelefoneRequestDTO t) => new Telefones
                    {
                        ID = Guid.NewGuid(),
                        IDContato = contato.ID,
                        Numero = t.Numero!
                    })
                    .ToList();

                if (telefonesNovos.Count != 0)
                    await _context.Telefones.AddRangeAsync(telefonesNovos);
            }

            await _context.SaveChangesAsync();

            return Ok(contato);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContato(Guid id)
        {
            Contatos? contato = await _context.Contatos.FindAsync(id);
            if (contato == null)
            {
                return NotFound("Contato não encontrado.");
            }

            _context.Contatos.Remove(contato);
            await _context.SaveChangesAsync();

            System.IO.File.AppendAllText("log.txt", $"Contato {contato.Nome} excluído em {DateTime.Now}\n");

            return NoContent();
        }
    }
}