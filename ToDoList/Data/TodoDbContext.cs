// Data/TodoDbContext.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>()
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Todo>()
            .Property(t => t.Description)
            .HasMaxLength(500);
    }
}