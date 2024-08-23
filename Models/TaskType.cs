namespace taskManager.Models
{
    public class TaskType
    {
        public int Id { get; set; } // Primary key
        public string Name { get; private set; }

        // Constructor to initialize based on Id
        public TaskType(int id)
        {
            Id = id;
            Name = FromId(id); // Corrected method name
        }

        public override string ToString() => Name;

        // Static method to get the name based on Id
        public static string FromId(int id) // Renamed for clarity
        {
            return id switch
            {
                0 => "Daily",
                1 => "Weekly",
                2 => "Monthly",
                _ => throw new ArgumentException($"Invalid TaskType ID: {id}", nameof(id))
            };
        }
    }
}