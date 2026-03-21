using ApiTodo.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApiTodo.Models
{
    public class ToDoItem
    {
        [Key]
        public int id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public ToDoStatus Status { get; set; } = ToDoStatus.Pendiente;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User? User { get; set; }

    }
}
