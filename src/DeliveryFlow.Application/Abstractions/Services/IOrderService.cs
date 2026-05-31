using DeliveryFlow.Application.Contracts.Dto.OrderDto;
using DeliveryFlow.Domain.Common;
using DeliveryFlow.Domain.Entities;

namespace DeliveryFlow.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<Result<OrderEntity>> Add(CreateOrderRequestDto createOrderRequestDto);

        Task<Result<bool>> Update(string id, UpdateOrderRequestDto updateOrderRequestDto);

        Task<Result<bool>> Delete(string id);

        Task<List<OrderEntity>> Get();

        Task<Result<OrderEntity>> GetById(string id);

        Task<Result<OrderEntity>> GetByOrderNumber(int orderNumber);

        Task<Result<bool>> RegisterDelivery(string id, RegisterDeliveryRequestDto registerDeliveryRequestDto);
    }
}