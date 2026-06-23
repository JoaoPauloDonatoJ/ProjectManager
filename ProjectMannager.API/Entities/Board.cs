using ProjectMannager.API.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace ProjectMannager.API.Entities
{
    public class Board : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int WorkspaceId { get; set; }
        public Workspace? Workspace { get; set; }
    }
}
