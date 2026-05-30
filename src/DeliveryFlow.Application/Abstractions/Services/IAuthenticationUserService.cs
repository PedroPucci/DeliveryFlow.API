using DeliveryFlow.Application.Contracts.Dto;
using DeliveryFlow.Domain.Common;

namespace DeliveryFlow.Application.Abstractions.Services
{
    public interface IAuthenticationUserService
    {
        Task<Result<string>> Login(UserForAuthenticationDTO userEntity);
    }
}