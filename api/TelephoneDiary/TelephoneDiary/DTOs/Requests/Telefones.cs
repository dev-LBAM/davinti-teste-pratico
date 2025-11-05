using System.ComponentModel.DataAnnotations;

namespace TelephoneDiary.DTOs.Requests
{
    public record TelefoneRequestDTO(
        Guid? ID,
        [Required(ErrorMessage = "O campo Número é obrigatório")]
        [MaxLength(16, ErrorMessage = "O campo Número deve ter no máximo 16 caracteres")]
        string Numero
    );
}
