using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Abstractions.Services;
using DeliveryFlow.Application.Contracts.Dto;
using DeliveryFlow.Domain.Common;
using DeliveryFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DeliveryFlow.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUoW _repositoryUoW;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<ProfileEntity> _roleManager;

        public UserService(
            IRepositoryUoW repositoryUoW,
            UserManager<UserEntity> userManager,
            RoleManager<ProfileEntity> roleManager)
        {
            _repositoryUoW = repositoryUoW;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<Result<UserEntity>> Add(CreateUserRequestDto createUserRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserEntity>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Result<UserResponseDto>> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> Update(string id, UpdateUserRequestDto updateUserRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}