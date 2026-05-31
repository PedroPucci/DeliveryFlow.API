using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Abstractions.Services;
using DeliveryFlow.Application.Contracts.Dto.OrderDto;
using DeliveryFlow.Application.Validators;
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

                var isValid = await IsValidOrderRequest(createOrderRequestDto);
                if (!isValid.Success)
                {
                    Log.Information(isValid.Message);
                    return Result<OrderEntity>.Error(isValid.Message);
                }

                var address = await GetAddressByZipCode(createOrderRequestDto.ZipCode);

                if (address is null)
                    return Result<OrderEntity>.Error("Invalid zip code.");

                var orderEntity = CreateOrderEntity(createOrderRequestDto, address);

                orderEntity.DeliveryAddress.OrderId = orderEntity.Id;

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

        public async Task<List<OrderEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                List<OrderEntity> orderEntities = await _repositoryUoW.OrderRepository.Get();
                _repositoryUoW.Commit();
                Log.Information(LogMessages.GetAllOrdersSuccess());
                return orderEntities;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.GetAllOrdersError(ex));
                throw new InvalidOperationException("Error loading order list. See logs for details.", ex);
            }
        }

        public async Task<Result<OrderEntity>> GetById(string id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var order = await _repositoryUoW.OrderRepository.GetByIdCheck(id);

                if (order is null)
                {
                    transaction.Rollback();
                    var message = LogMessages.CannotPerformActionOnOrder("retrieve", id);
                    Log.Error(message);

                    return Result<OrderEntity>.Error(message);
                }

                _repositoryUoW.Commit();

                Log.Information(LogMessages.GetOrderByIdSuccess(order));

                return Result<OrderEntity>.Ok(order);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.GetOrderByIdError(ex));
                throw new InvalidOperationException("Error retrieving the order. See inner exception for details.", ex);
            }
        }

        public async Task<Result<OrderEntity>> GetByOrderNumber(int orderNumber)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var order = await _repositoryUoW.OrderRepository.GetByOrderNumber(orderNumber);

                if (order is null)
                {
                    transaction.Rollback();
                    var message = LogMessages.CannotPerformActionOnOrder("retrieve by order number",orderNumber.ToString());
                    Log.Error(message);

                    return Result<OrderEntity>.Error(message);
                }

                _repositoryUoW.Commit();
                Log.Information(LogMessages.GetOrderByOrderNumberSuccess(order));

                return Result<OrderEntity>.Ok(order);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.GetOrderByOrderNumberError(ex));
                throw new InvalidOperationException("Error retrieving the order by order number. See inner exception for details.", ex);
            }
        }

        public async Task<Result<bool>> RegisterDelivery(string id, RegisterDeliveryRequestDto registerDeliveryRequestDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var order = await _repositoryUoW.OrderRepository.GetByIdCheck(id);

                if (order is null)
                {
                    var message = LogMessages.CannotPerformActionOnOrder(
                        "register delivery",
                        id);

                    Log.Error(message);
                    return Result<bool>.Error(message);
                }

                order.DeliveryDate = registerDeliveryRequestDto.DeliveryDate;
                order.ModificationDate = DateTime.UtcNow;

                await _repositoryUoW.OrderRepository.RegisterDelivery(order);
                await _repositoryUoW.SaveAsync();

                await transaction.CommitAsync();

                Log.Information(LogMessages.RegisterDeliverySuccess(order));

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.RegisterDeliveryError(ex));
                throw new InvalidOperationException("Failed to register order delivery. See logs for details.", ex);
            }
        }

        public async Task<Result<bool>> Update(string id, UpdateOrderRequestDto updateOrderRequestDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var order = await _repositoryUoW.OrderRepository.GetByIdCheck(id);

                if (order is null)
                {
                    var message = LogMessages.CannotPerformActionOnOrder("update", id);

                    Log.Error(message);

                    return Result<bool>.Error(message);
                }

                var address = await GetAddressByZipCode(updateOrderRequestDto.ZipCode);

                if (address is null)
                    return Result<bool>.Error("Invalid zip code.");

                order.Description = updateOrderRequestDto.Description;
                order.Value = updateOrderRequestDto.Value;
                order.ModificationDate = DateTime.UtcNow;

                order.DeliveryAddress.ZipCode = address.ZipCode!;
                order.DeliveryAddress.Street = address.Street!;
                order.DeliveryAddress.Number = updateOrderRequestDto.Number;
                order.DeliveryAddress.District = address.District!;
                order.DeliveryAddress.City = address.City!;
                order.DeliveryAddress.State = address.State!;

                _repositoryUoW.OrderRepository.Update(order);

                await _repositoryUoW.SaveAsync();

                await transaction.CommitAsync();

                Log.Information(LogMessages.UpdateOrderSuccess(order));

                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.UpdateOrderError(ex));
                throw new InvalidOperationException($"Failed to update order with id. See logs for details.", ex);
            }
        }

        /// <summary>
        /// Decisão:
        /// O endereço de entrega é obtido automaticamente através da API ViaCEP
        /// utilizando o CEP informado pelo usuário.
        /// Apenas o CEP e o número do endereço precisam ser enviados pelo frontend.
        /// </summary>
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

        private async Task<Result<CreateOrderRequestDto>> IsValidOrderRequest(CreateOrderRequestDto createOrderRequestDto)
        {
            var requestValidator = await new OrderRequestValidator().ValidateAsync(createOrderRequestDto);

            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<CreateOrderRequestDto>.Error(errorMessage);
            }

            return Result<CreateOrderRequestDto>.Ok();
        }
    }
}