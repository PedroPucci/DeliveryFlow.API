using DeliveryFlow.Application.Contracts.Dto;
using DeliveryFlow.Domain.Common;
using DeliveryFlow.Domain.Entities;

namespace DeliveryFlow.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<Result<UserEntity>> Add(CreateUserRequestDto createUserRequestDto);
        Task<Result<bool>> Update(string id, UpdateUserRequestDto updateUserRequestDto);
        Task<Result<bool>> Delete(string id);
        Task<List<UserEntity>> Get();
        Task<Result<UserResponseDto>> GetById(string id);
    }
}