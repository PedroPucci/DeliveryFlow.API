namespace DeliveryFlow.Application.Contracts.Dto.OrderDto
{
    public class CreateOrderRequestDto
    {
        public int OrderNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Value { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
    }
}