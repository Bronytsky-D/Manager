using Manager.Doman.Enums;

namespace Manager.Common.DTOs
{
    public class UpdateTaskRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Status? Status { get; set; }
        public Prioritys? Priority { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
