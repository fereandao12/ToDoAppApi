namespace ApiTodo.DTOs
{
    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; }
    }
}
