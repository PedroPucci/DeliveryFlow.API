using DeliveryFlow.Application.Services;
using DeliveryFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DeliveryFlow.Application.Abstractions.Persistence
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IRepositoryUoW _repositoryUoW;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<ProfileEntity> _roleManager;
        private UserService userService;
        private AuthenticationService authenticationService;

        public UnitOfWorkService(
            IRepositoryUoW repositoryUoW,
            UserManager<UserEntity> userManager,
            RoleManager<ProfileEntity> roleManager)
        {
            _repositoryUoW = repositoryUoW;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public UserService UserService
        {
            get
            {
                if (userService is null)
                    userService = new UserService(
                        _repositoryUoW,
                        _userManager,
                        _roleManager);
                return userService;
            }
        }

        public AuthenticationService AuthenticationService
        {
            get
            {
                if (authenticationService is null)
                    authenticationService = new AuthenticationService(
                        _repositoryUoW,
                        _userManager);
                return authenticationService;
            }
        }
    }
}