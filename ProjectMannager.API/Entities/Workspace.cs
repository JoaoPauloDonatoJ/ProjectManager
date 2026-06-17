using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;


namespace ProjectMannager.API.Entities
{
    public class Workspace
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Board> Boards { get; set; } = new List<Board>();
    }
}
