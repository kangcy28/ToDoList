using Microsoft.EntityFrameworkCore;
using ToDoList.Repositories.Interfaces;
using ToDoList.Repositories;
using Microsoft.OpenApi.Models;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Todo List API",
        Version = "v1",
        Description = "A simple Todo List API"
    });
});

// Register services
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<TodoManager>();
builder.Services.AddScoped<MenuSystem>();
builder.Services.AddScoped<FileService>();
// Add DbContext
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo List API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run menu system
using (var scope = app.Services.CreateScope())
{
    var menuSystem = scope.ServiceProvider.GetRequiredService<MenuSystem>();
    await menuSystem.RunAsync();
}

app.Run();