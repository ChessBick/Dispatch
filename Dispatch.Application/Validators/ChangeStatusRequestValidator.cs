using Dispatch.Application.Dtos;
using Dispatch.Domain.Enums;
using FluentValidation;

namespace Dispatch.Application.Validators
{
    public class ChangeStatusRequestValidator : AbstractValidator<ChangeStatusRequest>
    {
        public ChangeStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(s => Enum.TryParse<WorkOrderStatus>(s, ignoreCase: true, out _))
                .WithMessage(x => $"Invalid status '{x.Status}'. Valid values: {string.Join(", ", Enum.GetNames<WorkOrderStatus>())}.");

            RuleFor(x => x.Note).MaximumLength(1000);
        }
    }
}
