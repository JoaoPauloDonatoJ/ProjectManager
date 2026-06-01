using System.ComponentModel.DataAnnotations;

namespace ProjectMannager.API.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string UserName  { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email deve ser um endereço de email válido.")]
        [StringLength(150, ErrorMessage = "O email deve ter no máximo 150 caracteres.")]
        public string Email  { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string PasswordHash  { get; set; }

        public DateTime CreatedAt  { get; set; } = DateTime.Now;
    }
}
