using DeliveryFlow.Application.Contracts.Dto.OrderDto;
using DeliveryFlow.Application.Validators;
using Xunit;

namespace DeliveryFlow.Tests.Validators
{
    public class OrderRequestValidatorTests
    {
        private readonly OrderRequestValidator _validator;

        public OrderRequestValidatorTests()
        {
            _validator = new OrderRequestValidator();
        }

        [Fact]
        public async Task Validate_ShouldReturnSuccess_WhenRequestIsValid()
        {
            var request = new CreateOrderRequestDto
            {
                OrderNumber = 1001,
                Description = "Gaming notebook delivery",
                Value = 5999.90,
                ZipCode = "01310930",
                Number = "120"
            };

            var result = await _validator.ValidateAsync(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenOrderNumberIsInvalid()
        {
            var request = new CreateOrderRequestDto
            {
                OrderNumber = 0,
                Description = "Gaming notebook delivery",
                Value = 5999.90,
                ZipCode = "01310930",
                Number = "120"
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenDescriptionIsEmpty()
        {
            var request = new CreateOrderRequestDto
            {
                OrderNumber = 1001,
                Description = string.Empty,
                Value = 5999.90,
                ZipCode = "01310930",
                Number = "120"
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenDescriptionLengthIsLessThanFive()
        {
            var request = new CreateOrderRequestDto
            {
                OrderNumber = 1001,
                Description = "abc",
                Value = 5999.90,
                ZipCode = "01310930",
                Number = "120"
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenValueIsInvalid()
        {
            var request = new CreateOrderRequestDto
            {
                OrderNumber = 1001,
                Description = "Gaming notebook delivery",
                Value = 0,
                ZipCode = "01310930",
                Number = "120"
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenZipCodeIsEmpty()
        {
            var request = new CreateOrderRequestDto
            {
                OrderNumber = 1001,
                Description = "Gaming notebook delivery",
                Value = 5999.90,
                ZipCode = string.Empty,
                Number = "120"
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenNumberIsEmpty()
        {
            var request = new CreateOrderRequestDto
            {
                OrderNumber = 1001,
                Description = "Gaming notebook delivery",
                Value = 5999.90,
                ZipCode = "01310930",
                Number = string.Empty
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }
    }
}