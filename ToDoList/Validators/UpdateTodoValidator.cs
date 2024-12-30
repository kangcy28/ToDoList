using FluentValidation;
using ToDoList.DTOs;
namespace ToDoList.Validators
{
    public class UpdateTodoValidator : AbstractValidator<UpdateTodoDto>
    {
        public UpdateTodoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid Todo Id");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
