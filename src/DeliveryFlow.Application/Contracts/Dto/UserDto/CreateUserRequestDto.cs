namespace DeliveryFlow.Application.Contracts.Dto.UserDto
{
    public class CreateUserRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
        public string? Role { get; set; }
    }
}