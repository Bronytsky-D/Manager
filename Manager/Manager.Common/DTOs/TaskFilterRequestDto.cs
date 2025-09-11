using Manager.Doman.Enums;

namespace Manager.Common.DTOs
{
    public class TaskFilterRequestDto
    {
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public DateTime? DueDate { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;

        public Status? GetStatus()
            => Enum.TryParse<Status>(Status, true, out var s) ? s : null;

        public Prioritys? GetPriority()
            => Enum.TryParse<Prioritys>(Priority, true, out var p) ? p : null;
    }
}
