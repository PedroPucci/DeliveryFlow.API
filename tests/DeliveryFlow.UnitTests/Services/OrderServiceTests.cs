using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Abstractions.Repositories;
using DeliveryFlow.Application.Contracts.Dto.OrderDto;
using DeliveryFlow.Application.Services;
using DeliveryFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace DeliveryFlow.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IDbContextTransaction> _transactionMock;

        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _transactionMock = new Mock<IDbContextTransaction>();

            _repositoryUoWMock
                .Setup(x => x.BeginTransaction())
                .Returns(_transactionMock.Object);

            _repositoryUoWMock
                .Setup(x => x.OrderRepository)
                .Returns(_orderRepositoryMock.Object);

            _orderService = new OrderService(
                _repositoryUoWMock.Object,
                _httpClientFactoryMock.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnOrders_WhenOrdersExist()
        {
            var orders = new List<OrderEntity>
            {
                new OrderEntity
                {
                    Id = "1",
                    OrderNumber = 1001
                }
            };

            _orderRepositoryMock
                .Setup(x => x.Get())
                .ReturnsAsync(orders);

            var result = await _orderService.Get();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task Get_ShouldThrowException_WhenRepositoryFails()
        {
            _orderRepositoryMock
                .Setup(x => x.Get())
                .ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _orderService.Get());
        }

        [Fact]
        public async Task GetById_ShouldReturnSuccess_WhenOrderExists()
        {
            var order = new OrderEntity
            {
                Id = "1",
                OrderNumber = 1001
            };

            _orderRepositoryMock
                .Setup(x => x.GetByIdCheck("1"))
                .ReturnsAsync(order);

            var result = await _orderService.GetById("1");

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetById_ShouldReturnError_WhenOrderDoesNotExist()
        {
            _orderRepositoryMock
                .Setup(x => x.GetByIdCheck("1"))
                .ReturnsAsync((OrderEntity?)null);

            var result = await _orderService.GetById("1");

            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetByOrderNumber_ShouldReturnSuccess_WhenOrderExists()
        {
            var order = new OrderEntity
            {
                Id = "1",
                OrderNumber = 1001
            };

            _orderRepositoryMock
                .Setup(x => x.GetByOrderNumber(1001))
                .ReturnsAsync(order);

            var result = await _orderService.GetByOrderNumber(1001);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetByOrderNumber_ShouldReturnError_WhenOrderDoesNotExist()
        {
            _orderRepositoryMock
                .Setup(x => x.GetByOrderNumber(1001))
                .ReturnsAsync((OrderEntity?)null);

            var result = await _orderService.GetByOrderNumber(1001);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenOrderExists()
        {
            var order = new OrderEntity
            {
                Id = "1",
                OrderNumber = 1001,
                DeliveryAddress = new DeliveryAddressEntity()
            };

            var request = new UpdateOrderRequestDto
            {
                Description = "Updated order",
                Value = 100,
                ZipCode = "01310930",
                Number = "120"
            };

            _orderRepositoryMock
                .Setup(x => x.GetByIdCheck("1"))
                .ReturnsAsync(order);

            var result = await _orderService.Update("1", request);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_ShouldReturnError_WhenOrderDoesNotExist()
        {
            var request = new UpdateOrderRequestDto
            {
                Description = "Updated order",
                Value = 100,
                ZipCode = "01310930",
                Number = "120"
            };

            _orderRepositoryMock
                .Setup(x => x.GetByIdCheck("1"))
                .ReturnsAsync((OrderEntity?)null);

            var result = await _orderService.Update("1", request);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task RegisterDelivery_ShouldReturnSuccess_WhenOrderExists()
        {
            var order = new OrderEntity
            {
                Id = "1",
                OrderNumber = 1001
            };

            var request = new RegisterDeliveryRequestDto
            {
                DeliveryDate = DateTime.UtcNow
            };

            _orderRepositoryMock
                .Setup(x => x.GetByIdCheck("1"))
                .ReturnsAsync(order);

            var result = await _orderService.RegisterDelivery("1", request);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task RegisterDelivery_ShouldReturnError_WhenOrderDoesNotExist()
        {
            var request = new RegisterDeliveryRequestDto
            {
                DeliveryDate = DateTime.UtcNow
            };

            _orderRepositoryMock
                .Setup(x => x.GetByIdCheck("1"))
                .ReturnsAsync((OrderEntity?)null);

            var result = await _orderService.RegisterDelivery("1", request);

            Assert.False(result.Success);
        }
    }
}