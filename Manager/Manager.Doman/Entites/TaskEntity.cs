
using Manager.Doman.Enums;

namespace Manager.Doman.Entites
{
    public class TaskEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public Prioritys Priority { get; set; }
        public DateTime CreatedAt = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
    }
}
