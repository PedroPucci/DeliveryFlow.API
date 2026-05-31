using DeliveryFlow.Application.Contracts.DomainErrors;
using DeliveryFlow.Application.Contracts.Dto.OrderDto;
using DeliveryFlow.Shared.Helpers;
using FluentValidation;

namespace DeliveryFlow.Application.Validators
{
    public class OrderRequestValidator : AbstractValidator<CreateOrderRequestDto>
    {
        public OrderRequestValidator()
        {
            RuleFor(p => p.OrderNumber)
                .GreaterThan(0)
                    .WithMessage(OrderErrors.Order_Error_InvalidOrderNumber.Description());

            RuleFor(p => p.Description)
                .NotEmpty()
                    .WithMessage(OrderErrors.Order_Error_DescriptionCanNotBeNullOrEmpty.Description())
                .MinimumLength(5)
                    .WithMessage(OrderErrors.Order_Error_DescriptionLengthLessFive.Description());

            RuleFor(p => p.Value)
                .GreaterThan(0)
                    .WithMessage(OrderErrors.Order_Error_InvalidValue.Description());

            RuleFor(p => p.ZipCode)
                .NotEmpty()
                    .WithMessage(OrderErrors.Order_Error_InvalidZipCode.Description());

            RuleFor(p => p.Number)
                .NotEmpty()
                    .WithMessage(OrderErrors.Order_Error_InvalidAddressNumber.Description());
        }
    }
}