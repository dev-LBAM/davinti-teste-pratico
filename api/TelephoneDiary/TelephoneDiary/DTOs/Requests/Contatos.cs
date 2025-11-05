using System.ComponentModel.DataAnnotations;

namespace TelephoneDiary.DTOs.Requests
{
    public class ContatoRequestDTO
    {
        public record Create(
            [Required(ErrorMessage = "O campo Nome é obrigatório")]
            [MaxLength(100, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres")]
            string Nome,

            [Range(0, 150, ErrorMessage = "O campo Idade deve estar entre 0 e 150")]
            int Idade,

            [Required(ErrorMessage = "O campo Telefone é obrigatório")]
            List<TelefoneRequestDTO> Telefones
        );

        public record Update(
            [MaxLength(100, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres")]
            string? Nome = null,

            [Range(0, 150, ErrorMessage = "O campo Idade deve estar entre 0 e 150")]
            int? Idade = null,

            List<TelefoneRequestDTO>? Telefones = null
        );
    }
}
