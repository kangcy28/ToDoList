// Data/TodoDbContext.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Reflection.Emit;
using ToDoList.Models;

public class TodoDbContext : IdentityDbContext<ApplicationUser>
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Todo>()
            .Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<Todo>()
            .Property(t => t.Description)
            .HasMaxLength(500);
        // Add relationship configuration
        builder.Entity<Todo>()
            .HasOne(t => t.User)
            .WithMany(u => u.Todos)
            .HasForeignKey(t => t.UserId)
            .IsRequired(false);  // Make the relationship optional
    }
}