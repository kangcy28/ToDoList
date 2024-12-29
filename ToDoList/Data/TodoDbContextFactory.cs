using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Data
{
    // Add this class in the same directory as your TodoDbContext
    public class TodoDbContextFactory : IDesignTimeDbContextFactory<TodoDbContext>
    {
        public TodoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TodoDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodoDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new TodoDbContext(optionsBuilder.Options);
        }
    }
}
