using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Abstractions.Services;
using DeliveryFlow.Application.Contracts.Dto.UserDto;
using DeliveryFlow.Application.Validators;
using DeliveryFlow.Domain.Common;
using DeliveryFlow.Domain.Entities;
using DeliveryFlow.Shared.Logging;
using Microsoft.AspNetCore.Identity;
using Serilog;

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

        public async Task<Result<UserEntity>> Add(CreateUserRequestDto createUserRequestDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var userEntity = new UserEntity
                {
                    Email = createUserRequestDto.Email,
                    Name = createUserRequestDto.Name,
                    UserName = createUserRequestDto.Email,
                    CreateDate = DateTime.UtcNow,
                    IsActive = true
                };

                var isValid = await IsValidUserRequest(createUserRequestDto);
                if (!isValid.Success)
                {
                    Log.Information(isValid.Message);
                    return Result<UserEntity>.Error(isValid.Message);
                }

                if (string.IsNullOrWhiteSpace(createUserRequestDto.Role))
                {
                    Log.Information("'Role' can not be null or empty!");
                    return Result<UserEntity>.Error("'Role' can not be null or empty!");
                }

                var role = createUserRequestDto.Role.Trim();

                var roleExists = await _roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    Log.Information("Invalid role.");
                    return Result<UserEntity>.Error("You do not have permission to perform this action.");
                }

                var createResult = await _userManager.CreateAsync(userEntity, createUserRequestDto.Password!);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(" ", createResult.Errors.Select(e => e.Description));
                    return Result<UserEntity>.Error(errors);
                }

                var roleResult = await _userManager.AddToRoleAsync(userEntity, role);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(" ", roleResult.Errors.Select(e => e.Description));
                    return Result<UserEntity>.Error(errors);
                }

                await transaction.CommitAsync();
                Log.Information(LogMessages.AddUserSuccess(userEntity));
                return Result<UserEntity>.Ok(userEntity);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Information(LogMessages.AddUserError(ex));
                throw;
            }
        }

        public async Task<Result<bool>> Delete(string id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var user = await _repositoryUoW.UserRepository.GetByIdCheck(id);

                if (user is null)
                {
                    transaction.Rollback();

                    var message = LogMessages.CannotPerformActionOnUser("retrieve", id);
                    Log.Error(message);

                    return Result<bool>.Error(message);
                }

                user.IsActive = false;
                user.ModificationDate = DateTime.UtcNow;

                _repositoryUoW.UserRepository.Update(user);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information(LogMessages.DeleteUserSuccess(user));
                return Result<bool>.Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.DeleteUserError(ex));
                throw new InvalidOperationException($"Failed to delete user with id {id}. See logs for details.", ex);
            }
        }

        public async Task<List<UserEntity>> Get()
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                List<UserEntity> userEntities = await _repositoryUoW.UserRepository.Get();
                _repositoryUoW.Commit();

                Log.Information(LogMessages.GetAllUsersSuccess());
                return userEntities;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.GetAllUsersError(ex));
                throw new InvalidOperationException("Error to loading the list User. See logs for details.", ex);
            }
        }

        public async Task<Result<UserResponseDto>> GetById(string id)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var user = await _repositoryUoW.UserRepository.GetByIdCheck(id);

                if (user is null)
                {
                    transaction.Rollback();

                    var message = LogMessages.CannotPerformActionOnUser("retrieve", id);
                    Log.Error(message);

                    return Result<UserResponseDto>.Error(message);
                }

                var userResponse = new UserResponseDto
                {
                    Email = user?.Email,
                    Name = user?.Name,
                    IsActive = user?.IsActive ?? false
                };

                _repositoryUoW.Commit();

                Log.Information(LogMessages.GetUserByIdSuccess(user));
                return Result<UserResponseDto>.Ok(userResponse);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.GetUserByIdError(ex));
                throw new InvalidOperationException("Error retrieving the user. See inner exception for details.", ex);
            }
        }

        public async Task<Result<bool>> Update(string id, UpdateUserRequestDto updateUserRequestDto)
        {
            using var transaction = _repositoryUoW.BeginTransaction();

            try
            {
                var user = await _repositoryUoW.UserRepository.GetByIdCheck(id);

                if (user is null)
                {
                    var message = LogMessages.CannotPerformActionOnUser("update", id);
                    Log.Error(message);
                    return Result<bool>.Error(message);
                }

                user.Email = updateUserRequestDto.Email;
                user.UserName = updateUserRequestDto.UserName;
                user.NormalizedUserName = updateUserRequestDto.UserName;
                user.Name = updateUserRequestDto.Name;
                user.IsActive = updateUserRequestDto.IsActive;
                user.ModificationDate = DateTime.UtcNow;

                _repositoryUoW.UserRepository.Update(user);
                await _repositoryUoW.SaveAsync();
                await transaction.CommitAsync();

                Log.Information(LogMessages.UpdateUserSuccess(user));
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error(LogMessages.UpdateUserError(ex));
                throw new InvalidOperationException($"Failed to update user with id. See logs for details.", ex);
            }
        }

        private async Task<Result<CreateUserRequestDto>> IsValidUserRequest(CreateUserRequestDto createUserRequestDto)
        {
            var requestValidator = await new UserRequestValidator().ValidateAsync(createUserRequestDto);

            if (!requestValidator.IsValid)
            {
                string errorMessage = string.Join(" ", requestValidator.Errors.Select(e => e.ErrorMessage));
                errorMessage = errorMessage.Replace(Environment.NewLine, "");
                return Result<CreateUserRequestDto>.Error(errorMessage);
            }

            return Result<CreateUserRequestDto>.Ok();
        }
    }
}