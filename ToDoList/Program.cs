using Microsoft.EntityFrameworkCore;
using ToDoList.Repositories.Interfaces;
using ToDoList.Repositories;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the repository
builder.Services.AddScoped<ITodoRepository, TodoRepository>();


// Add services to the container.
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// 4. Swagger Configuration
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo List API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
