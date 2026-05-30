namespace DeliveryFlow.Application.Contracts.Dto
{
    public class UpdateUserRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}