using Microsoft.EntityFrameworkCore;
using taskManager.Models;


namespace taskManager.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }  //

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
