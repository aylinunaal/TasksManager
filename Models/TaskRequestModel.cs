namespace taskManager.Models
{
    public class TaskRequestModel
    {
        
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public required bool IsCompleted { get; set; }
        public required TaskType Type { get; set; }
    }
}
