namespace DeliveryFlow.Application.Contracts.Dto.UserDto
{
    public class UpdateUserRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}