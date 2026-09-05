using System.ComponentModel.DataAnnotations;

namespace ProjectMannager.API.DTOs
{
    public record CreateColumnDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome deve ter entre 1 e 100 caracteres.")]
        public string Name { get; init; } = string.Empty;
    }

    public record UpdateColumnDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome deve ter entre 1 e 100 caracteres.")]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "A posição é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A posição deve ser um valor positivo maior que zero.")]
        public int Position { get; init; }
    }

    public record ColumnResponseDto(int Id, string Name, int Position, int BoardId);
}
