using DeliveryFlow.Domain.Entities;

namespace DeliveryFlow.Application.Abstractions.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderEntity> Add(OrderEntity orderEntity);

        OrderEntity Update(OrderEntity orderEntity);

        Task<bool> Delete(string id);

        Task<List<OrderEntity>> Get();

        Task<OrderEntity?> GetByIdCheck(string id);

        Task<OrderEntity?> GetByOrderNumber(int orderNumber);

        Task<bool> RegisterDelivery(OrderEntity orderEntity);
    }
}