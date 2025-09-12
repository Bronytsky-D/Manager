using Manager.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Manager.Domain.Entites
{
    public class TaskEntity
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public Prioritys Priority { get; set; } = Prioritys.Medium;
        public DateTime CreatedAt = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
