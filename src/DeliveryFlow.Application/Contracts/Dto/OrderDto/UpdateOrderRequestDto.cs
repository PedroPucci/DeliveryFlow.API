namespace DeliveryFlow.Application.Contracts.Dto.OrderDto
{
    public class UpdateOrderRequestDto
    {
        public string Description { get; set; } = string.Empty;

        public decimal Value { get; set; }

        public string ZipCode { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string Number { get; set; } = string.Empty;

        public string District { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;
    }
}