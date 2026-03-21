using ApiTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTodo.Data
{
    // Contexto principal de la base de datos que gestiona las entidades de usuarios y tareas mediante Entity Framework Core.
    public class AppDbContext : DbContext
    {
        // Inicializa el contexto de la base de datos aplicando las opciones de configuración proporcionadas (como la cadena de conexión).
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ToDoItem> toDoItems { get; set; }

        // Configura las reglas y restricciones del modelo de datos, estableciendo que el nombre de usuario debe ser único.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
        }
    }
}