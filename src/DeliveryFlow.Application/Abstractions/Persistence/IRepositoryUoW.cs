using DeliveryFlow.Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeliveryFlow.Application.Abstractions.Persistence
{
    public interface IRepositoryUoW
    {
        IUserRepository UserRepository { get; }
        IOrderRepository OrderRepository { get; }

        Task SaveAsync();
        void Commit();
        IDbContextTransaction BeginTransaction();
    }
}