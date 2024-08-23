namespace taskManager.Models
{
    public class TaskModel
    {
        public  int Id { get; set; }
        public string? Title { get; set; }
        public  string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public  bool? IsCompleted { get; set; }
        public  TaskType? Type { get; set; }
        public  int UserId { get; set; }
        
    }
}
