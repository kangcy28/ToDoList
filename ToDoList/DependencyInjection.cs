using FluentValidation;  // Add this
using ToDoList.Models;
using ToDoList.Validators;
using ToDoList.DTOs;
using FluentValidation.AspNetCore;
using ToDoList.Mappings;

namespace ToDoList
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TodoMappingProfile));
            // Add FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            // Register validators
            services.AddScoped<IValidator<CreateTodoDto>, CreateTodoValidator>();
            services.AddScoped<IValidator<UpdateTodoDto>, UpdateTodoValidator>();
            services.AddScoped<IValidator<Todo>, TodoValidator>();

            return services;
        }
    }
}