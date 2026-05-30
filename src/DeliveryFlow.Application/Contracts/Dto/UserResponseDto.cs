namespace DeliveryFlow.Application.Contracts.Dto
{
    public class UserResponseDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsActive { get; set; }
    }
}