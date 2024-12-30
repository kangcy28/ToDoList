using AutoMapper;
using ToDoList.Models;
using ToDoList.DTOs;

namespace ToDoList.Mappings
{
    public class TodoMappingProfile : Profile
    {
        public TodoMappingProfile()
        {
            // CreateTodoDto -> Todo
            CreateMap<CreateTodoDto, Todo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // UpdateTodoDto -> Todo
            CreateMap<UpdateTodoDto, Todo>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            // Todo -> TodoDto
            CreateMap<Todo, TodoDto>();
        }
    }
}