
using System.ComponentModel.DataAnnotations;


namespace taskManager.Models
{

    public class RegisterModel
    {
        public required string Username { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
