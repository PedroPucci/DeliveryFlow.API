using DeliveryFlow.Application.Abstractions.Repositories;
using DeliveryFlow.Domain.Entities;
using DeliveryFlow.Infrastructure.Connections;
using Microsoft.AspNetCore.Identity;

namespace DeliveryFlow.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public UserRepository(
            DataContext context,
            UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task<UserEntity> Add(UserEntity userEntity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckPassword(UserEntity userEntity, string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserEntity>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserEntity?> GetByIdCheck(string id)
        {
            throw new NotImplementedException();
        }

        public UserEntity Update(UserEntity userEntity)
        {
            throw new NotImplementedException();
        }
    }
}