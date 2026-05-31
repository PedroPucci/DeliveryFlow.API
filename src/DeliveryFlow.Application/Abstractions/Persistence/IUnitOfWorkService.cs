using DeliveryFlow.Application.Services;

namespace DeliveryFlow.Application.Abstractions.Persistence
{
    public interface IUnitOfWorkService
    {
        UserService UserService { get; }
        OrderService OrderService { get; }
        AuthenticationService AuthenticationService { get; }
    }
}