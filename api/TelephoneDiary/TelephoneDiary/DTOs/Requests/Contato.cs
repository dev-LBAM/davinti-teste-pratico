using System.ComponentModel.DataAnnotations;

namespace TelephoneDiary.DTOs.Requests
{
    public class ContatoRequestDTO
    {
        public record Create(
            [property: Required(ErrorMessage = "O campo Nome é obrigatório")]
            [property: MaxLength(100, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres")]
            string Nome,

            [property: Range(0, 150, ErrorMessage = "O campo Idade deve estar entre 0 e 150")]
            int Idade,

            List<TelefoneRequestDTO>? Telefones = null
        );

        public record Update(
            [property: MaxLength(100, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres")]
            string? Nome = null,

            [property: Range(0, 150, ErrorMessage = "O campo Idade deve estar entre 0 e 150")]
            int? Idade = null,

            List<TelefoneRequestDTO>? Telefones = null
        );
    }
}
