using Manager.Doman.Enums;

namespace Manager.DTOs
{
    public class CreateTaskRequstDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Status? Status { get; set; }
        public Prioritys? Prioritys { get; set; }

    }
}
