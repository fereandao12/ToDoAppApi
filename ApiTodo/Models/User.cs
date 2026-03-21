namespace ApiTodo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<ToDoItem> ToDos { get; set; } = new List<ToDoItem>();
    }
}
