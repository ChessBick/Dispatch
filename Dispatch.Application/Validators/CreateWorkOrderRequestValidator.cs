using Dispatch.Application.Dtos;
using FluentValidation;

namespace Dispatch.Application.Validators
{
    public class CreateWorkOrderRequestValidator : AbstractValidator<CreateWorkOrderRequest>
    {
        public CreateWorkOrderRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(2000);
            RuleFor(x => x.AssignedTo).MaximumLength(200);
        }
    }
}
