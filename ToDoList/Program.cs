using Microsoft.EntityFrameworkCore;
using ToDoList.Repositories.Interfaces;
using ToDoList.Repositories;
using Microsoft.OpenApi.Models;
using ToDoList.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "TodoList API",
        Description = "An ASP.NET Core Web API for managing todo items",
        Contact = new OpenApiContact
        {
            Name = "Bert_kang",
            Email = "bert_kang@bankpro.com"
        }
    });

    // Enable annotations
    options.EnableAnnotations();

    // Add XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Register services
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<TodoManager>();
builder.Services.AddScoped<FileService>();

// Remove MenuSystem registration since it's not needed for API
// builder.Services.AddScoped<MenuSystem>();

// Add DbContext
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Remove the menu system execution since this is now a Web API
// using (var scope = app.Services.CreateScope())
// {
//     var menuSystem = scope.ServiceProvider.GetRequiredService<MenuSystem>();
//     await menuSystem.RunAsync();
// }

app.Run();