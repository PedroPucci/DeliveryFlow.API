namespace DeliveryFlow.Domain.Entities
{
    public class OrderEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int OrderNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Value { get; set; }
        public DeliveryAddressEntity DeliveryAddress { get; set; } = new();
        public DateTime? DeliveryDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool IsActive { get; set; }
    }
}