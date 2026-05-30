using DeliveryFlow.Application.Contracts.DomainErrors;
using DeliveryFlow.Application.Contracts.Dto;
using DeliveryFlow.Shared.Helpers;
using FluentValidation;

namespace DeliveryFlow.Application.Validators
{
    public class UserRequestValidator : AbstractValidator<CreateUserRequestDto>
    {
        public UserRequestValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage(UserErrors.User_Error_NameCanNotBeNullOrEmpty.Description())
                .MinimumLength(8)
                    .WithMessage(UserErrors.User_Error_NameLengthLessEight.Description());
        }
    }
}