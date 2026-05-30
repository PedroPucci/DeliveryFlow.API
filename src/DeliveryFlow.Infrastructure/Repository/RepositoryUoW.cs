using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Abstractions.Repositories;
using DeliveryFlow.Domain.Entities;
using DeliveryFlow.Infrastructure.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace DeliveryFlow.Infrastructure.Repository
{
    public class RepositoryUoW : IRepositoryUoW
    {
        private readonly DataContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private bool _disposed = false;
        private IUserRepository? _userEntityRepository = null;

        public RepositoryUoW(DataContext context, UserManager<UserEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userEntityRepository is null)
                {
                    _userEntityRepository = new UserRepository(_context, _userManager);
                }
                return _userEntityRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error($"Database connection failed: {ex.Message}");
                throw new ApplicationException("Database is not available. Please check the connection.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}