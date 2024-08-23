using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using taskManager.Models;
using taskManager.Data;
namespace taskManager.Services
{
    public class TaskService
    {
        private readonly DataContext _context;

        // Constructor: DataContext enjekte edilir
        public TaskService(DataContext context)
        {
            _context = context;
        }

        // ID ve UserId'ye göre görev getirme
        public async Task<TaskModel> GetTaskById(int id, int userId)
        {
            // Veritabanı sorgusu kullanarak görev getirilir
            return await _context.Tasks.SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        // Yeni görev ekleme
        public async Task AddTask(TaskModel task)
        {
            _context.Tasks.Add(task); // Görev eklenir
            await _context.SaveChangesAsync(); // Değişiklikler veritabanına kaydedilir
        }

        // Görevi güncelleme
        public async Task UpdateTask(TaskModel task)
        {
            var existingTask = await _context.Tasks.FindAsync(task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.IsCompleted = task.IsCompleted;
                existingTask.Type = task.Type;

                await _context.SaveChangesAsync(); // Değişiklikler veritabanına kaydedilir
            }
        }

        // Görevi silme
        public async Task DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task); // Görev kaldırılır
                await _context.SaveChangesAsync(); // Değişiklikler veritabanına kaydedilir
            }
        }
    }
}
