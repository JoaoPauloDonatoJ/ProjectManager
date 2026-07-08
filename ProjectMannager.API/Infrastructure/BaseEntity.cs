namespace ProjectMannager.API.Infrastructure
{
    public abstract class BaseEntity
    {
        public string? InternalControl { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedByName { get; set; } = string.Empty;
        public int StateCode { get; set; } = 1; // Ex: 1 = Ativo, 0 = Inativo

    }
}
