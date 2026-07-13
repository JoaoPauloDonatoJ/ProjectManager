using ProjectMannager.API.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace ProjectMannager.API.Entities
{
    public class Column : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Position { get; set; }
        public int BoardId { get; set; }
        public Board? Board { get; set; }

    }
}
