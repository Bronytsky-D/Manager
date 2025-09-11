using Manager.Doman.Enums;

namespace Manager.Common.DTOs
{
    public class UpdateTaskRequestDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Status? Status { get; set; }
        public Prioritys? Priority { get; set; }
    }
}
