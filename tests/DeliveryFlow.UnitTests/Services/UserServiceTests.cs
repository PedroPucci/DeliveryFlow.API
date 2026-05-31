using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Abstractions.Repositories;
using DeliveryFlow.Application.Contracts.Dto.UserDto;
using DeliveryFlow.Application.Services;
using DeliveryFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace DeliveryFlow.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IRepositoryUoW> _repositoryUoWMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<UserManager<UserEntity>> _userManagerMock;
        private readonly Mock<RoleManager<ProfileEntity>> _roleManagerMock;
        private readonly Mock<IDbContextTransaction> _transactionMock;

        private readonly UserService _userService;

        public UserServiceTests()
        {
            _repositoryUoWMock = new Mock<IRepositoryUoW>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _transactionMock = new Mock<IDbContextTransaction>();

            var userStoreMock = new Mock<IUserStore<UserEntity>>();
            _userManagerMock = new Mock<UserManager<UserEntity>>(
                userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            var roleStoreMock = new Mock<IRoleStore<ProfileEntity>>();
            _roleManagerMock = new Mock<RoleManager<ProfileEntity>>(
                roleStoreMock.Object, null!, null!, null!, null!);

            _repositoryUoWMock
                .Setup(x => x.BeginTransaction())
                .Returns(_transactionMock.Object);

            _repositoryUoWMock
                .Setup(x => x.UserRepository)
                .Returns(_userRepositoryMock.Object);

            _userService = new UserService(
                _repositoryUoWMock.Object,
                _userManagerMock.Object,
                _roleManagerMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnSuccess_WhenUserIsValid()
        {
            var request = new CreateUserRequestDto
            {
                Name = "Pedro Pucci",
                Email = "pedro@email.com",
                Password = "Pedro@12345",
                Role = "User"
            };

            _roleManagerMock.Setup(x => x.RoleExistsAsync("User")).ReturnsAsync(true);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<UserEntity>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userService.Add(request);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(request.Email, result.Data.Email);
        }

        [Fact]
        public async Task Add_ShouldReturnError_WhenRoleIsInvalid()
        {
            var request = new CreateUserRequestDto
            {
                Name = "Pedro Pucci",
                Email = "pedro@email.com",
                Password = "Pedro@12345",
                Role = "Invalid"
            };

            _roleManagerMock.Setup(x => x.RoleExistsAsync("Invalid")).ReturnsAsync(false);

            var result = await _userService.Add(request);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Delete_ShouldReturnSuccess_WhenUserExists()
        {
            var user = new UserEntity
            {
                Id = "1",
                Name = "Pedro Pucci",
                Email = "pedro@email.com",
                IsActive = true
            };

            _userRepositoryMock.Setup(x => x.GetByIdCheck("1")).ReturnsAsync(user);
            _repositoryUoWMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _userService.Delete("1");

            Assert.True(result.Success);
            _userRepositoryMock.Verify(x => x.Update(user), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnError_WhenUserDoesNotExist()
        {
            _userRepositoryMock.Setup(x => x.GetByIdCheck("1")).ReturnsAsync((UserEntity?)null);

            var result = await _userService.Delete("1");

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Get_ShouldReturnUsers_WhenUsersExist()
        {
            var users = new List<UserEntity>
            {
                new UserEntity { Id = "1", Name = "Pedro Pucci", Email = "pedro@email.com" }
            };

            _userRepositoryMock.Setup(x => x.Get()).ReturnsAsync(users);

            var result = await _userService.Get();

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task Get_ShouldThrowException_WhenRepositoryFails()
        {
            _userRepositoryMock.Setup(x => x.Get()).ThrowsAsync(new Exception("Database error"));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.Get());
        }

        [Fact]
        public async Task GetById_ShouldReturnSuccess_WhenUserExists()
        {
            var user = new UserEntity
            {
                Id = "1",
                Name = "Pedro Pucci",
                Email = "pedro@email.com",
                IsActive = true
            };

            _userRepositoryMock.Setup(x => x.GetByIdCheck("1")).ReturnsAsync(user);

            var result = await _userService.GetById("1");

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(user.Email, result.Data.Email);
        }

        [Fact]
        public async Task GetById_ShouldReturnError_WhenUserDoesNotExist()
        {
            _userRepositoryMock.Setup(x => x.GetByIdCheck("1")).ReturnsAsync((UserEntity?)null);

            var result = await _userService.GetById("1");

            Assert.False(result.Success);
        }

        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenUserExists()
        {
            var user = new UserEntity
            {
                Id = "1",
                Name = "Pedro Pucci",
                Email = "old@email.com",
                IsActive = true
            };

            var request = new UpdateUserRequestDto
            {
                Name = "Pedro Atualizado",
                Email = "new@email.com",
                IsActive = true
            };

            _userRepositoryMock.Setup(x => x.GetByIdCheck("1")).ReturnsAsync(user);
            _repositoryUoWMock.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _userService.Update("1", request);

            Assert.True(result.Success);
            Assert.Equal(request.Email, user.Email);
            Assert.Equal(request.Name, user.Name);
        }

        [Fact]
        public async Task Update_ShouldReturnError_WhenUserDoesNotExist()
        {
            var request = new UpdateUserRequestDto
            {
                Name = "Pedro Atualizado",
                Email = "new@email.com",
                IsActive = true
            };

            _userRepositoryMock.Setup(x => x.GetByIdCheck("1")).ReturnsAsync((UserEntity?)null);

            var result = await _userService.Update("1", request);

            Assert.False(result.Success);
        }
    }
}