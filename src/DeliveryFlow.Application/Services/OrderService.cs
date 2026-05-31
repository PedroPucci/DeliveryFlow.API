using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Abstractions.Services;
using DeliveryFlow.Application.Contracts.Dto.OrderDto;
using DeliveryFlow.Domain.Common;
using DeliveryFlow.Domain.Entities;
using DeliveryFlow.Shared.Logging;
using Serilog;
using System.Net.Http.Json;

namespace DeliveryFlow.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryUoW _repositoryUoW;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(
            IRepositoryUoW repositoryUoW,
            IHttpClientFactory httpClientFactory)
        {
            _repositoryUoW = repositoryUoW;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result<OrderEntity>> Add(CreateOrderRequestDto createOrderRequestDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var existingOrder = await _repositoryUoW
                    .OrderRepository
                    .GetByOrderNumber(createOrderRequestDto.OrderNumber);

                if (existingOrder is not null)
                {
                    Log.Information(LogMessages.OrderAlreadyExists(existingOrder.OrderNumber));
                    return Result<OrderEntity>.Error("An order with this number already exists.");
                }

                var address = await GetAddressByZipCode(createOrderRequestDto.ZipCode);

                if (address is null)
                    return Result<OrderEntity>.Error("Invalid zip code.");

                var orderEntity = CreateOrderEntity(createOrderRequestDto, address);

                var result = await _repositoryUoW.OrderRepository.Add(orderEntity);

                _repositoryUoW.Commit();
                transaction.Commit();

                Log.Information(LogMessages.AddOrderSuccess(orderEntity));

                return Result<OrderEntity>.Ok(result);
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                Log.Error(LogMessages.AddOrderError(ex));

                return Result<OrderEntity>.Error("Error while adding order.");
            }
        }

        public Task<Result<bool>> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderEntity>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Result<OrderEntity>> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<OrderEntity>> GetByOrderNumber(int orderNumber)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> RegisterDelivery(string id, RegisterDeliveryRequestDto registerDeliveryRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Update(string id, UpdateOrderRequestDto updateOrderRequestDto)
        {
            throw new NotImplementedException();
        }

        private async Task<ViaCepResponseDto?> GetAddressByZipCode(string zipCode)
        {
            try
            {
                var normalizedZipCode = zipCode
                    .Replace("-", "")
                    .Trim();

                if (string.IsNullOrWhiteSpace(normalizedZipCode) || normalizedZipCode.Length != 8)
                    return null;

                var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.GetAsync(
                    $"https://viacep.com.br/ws/{normalizedZipCode}/json/");

                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Log.Warning($"Error calling ViaCEP. StatusCode: {response.StatusCode}. Content: {content}");
                    return null;
                }

                var address = await response.Content.ReadFromJsonAsync<ViaCepResponseDto>();

                if (address is null || address.Error)
                    return null;

                return address;
            }
            catch (Exception ex)
            {
                Log.Error($"Error retrieving address from ViaCEP. Details: {ex.Message}");
                return null;
            }
        }

        private static OrderEntity CreateOrderEntity(
            CreateOrderRequestDto request,
            ViaCepResponseDto address)
        {
            return new OrderEntity
            {
                OrderNumber = request.OrderNumber,
                Description = request.Description,
                Value = request.Value,
                CreateDate = DateTime.UtcNow,

                DeliveryAddress = new DeliveryAddressEntity
                {
                    ZipCode = address.ZipCode,
                    Street = address.Street,
                    Number = request.Number,
                    District = address.District,
                    City = address.City,
                    State = address.State
                }
            };
        }
    }
}