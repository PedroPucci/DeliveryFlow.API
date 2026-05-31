using DeliveryFlow.Application.Abstractions.Repositories;
using DeliveryFlow.Domain.Entities;
using DeliveryFlow.Infrastructure.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<UserEntity> Add(UserEntity userEntity)
        {
            var result = await _userManager.CreateAsync(userEntity, userEntity.PasswordHash);

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join(" | ", result.Errors.Select(e => e.Description)));

            return userEntity;
        }

        public async Task<bool> CheckPassword(UserEntity userEntity, string password)
        {
            var result = await _userManager.CheckPasswordAsync(userEntity, password);
            return result;
        }


        public async Task<bool> Delete(string id)
        {
            var user = await GetByIdCheck(id);

            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<UserEntity>> Get()
        {
            return await (
                from user in _context.Users.AsNoTracking()
                join userRole in _context.UserRoles.AsNoTracking()
                    on user.Id equals userRole.UserId
                join role in _context.Roles.AsNoTracking()
                    on userRole.RoleId equals role.Id
                orderby user.Id
                select new UserEntity
                {
                    Email = user.Email,
                    Name = user.Name,
                    IsActive = user.IsActive
                }
            ).ToListAsync();
        }

        public async Task<UserEntity> GetByUserName(string userName)
        {
            var result = await _userManager.FindByEmailAsync(userName);
            return result;
        }

        public async Task<UserEntity?> GetByIdCheck(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public UserEntity Update(UserEntity userEntity)
        {
            return _context.Users.Update(userEntity).Entity;
        }
    }
}