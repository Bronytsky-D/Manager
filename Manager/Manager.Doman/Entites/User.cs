
namespace Manager.Doman.Entites
{
    public class User
    {
        public Guid Id = Guid.NewGuid();
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();

        public DateTime createdAt = DateTime.Now;
        public DateTime updatedAt { get; set; }
    }
}
