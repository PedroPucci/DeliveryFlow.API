using DeliveryFlow.Application.Abstractions.Repositories;
using DeliveryFlow.Domain.Entities;
using DeliveryFlow.Infrastructure.Connections;
using Microsoft.EntityFrameworkCore;

namespace DeliveryFlow.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;

        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<OrderEntity> Add(OrderEntity orderEntity)
        {
            var result = await _context.Orders.AddAsync(orderEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> Delete(string id)
        {
            var order = await GetByIdCheck(id);

            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<OrderEntity>> Get()
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.DeliveryAddress)
                .OrderBy(x => x.OrderNumber)
                .ToListAsync();
        }

        public async Task<OrderEntity?> GetByIdCheck(string id)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.DeliveryAddress)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<OrderEntity?> GetByOrderNumber(int orderNumber)
        {
            return await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);
        }

        public async Task<bool> RegisterDelivery(OrderEntity orderEntity)
        {
            _context.Orders.Update(orderEntity);

            await _context.SaveChangesAsync();

            return true;
        }

        public OrderEntity Update(OrderEntity orderEntity)
        {
            return _context.Orders.Update(orderEntity).Entity;
        }
    }
}