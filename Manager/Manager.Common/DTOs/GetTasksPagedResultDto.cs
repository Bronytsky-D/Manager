using Manager.Doman.Entites;

namespace Manager.Common.DTOs
{
    public class GetTasksPagedResultDto
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<TaskEntity> Items { get; set; }
    }
}
