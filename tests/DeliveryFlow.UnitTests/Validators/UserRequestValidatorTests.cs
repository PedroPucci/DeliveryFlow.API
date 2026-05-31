using DeliveryFlow.Application.Contracts.Dto.UserDto;
using DeliveryFlow.Application.Validators;

namespace DeliveryFlow.Tests.Validators
{
    public class UserRequestValidatorTests
    {
        private readonly UserRequestValidator _validator;

        public UserRequestValidatorTests()
        {
            _validator = new UserRequestValidator();
        }

        [Fact]
        public async Task Validate_ShouldReturnSuccess_WhenNameIsValid()
        {
            var request = new CreateUserRequestDto
            {
                Name = "Pedro Pucci"
            };
            var result = await _validator.ValidateAsync(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenNameIsEmpty()
        {
            var request = new CreateUserRequestDto
            {
                Name = string.Empty
            };

            var result = await _validator.ValidateAsync(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldReturnError_WhenNameLengthIsLessThanEight()
        {
            var request = new CreateUserRequestDto
            {
                Name = "Pedro"
            };

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
        }
    }
}