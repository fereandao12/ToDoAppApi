namespace ApiTodo.DTOs
{
    public class ToDoItemUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int StatusId { get; set; }
    }
}
