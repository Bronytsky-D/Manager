using Manager.Domain.Enums;

namespace Manager.Common.DTOs
{
    public class CreateTaskRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Status? Status { get; set; }
        public Prioritys? Prioritys { get; set; }

    }
}
