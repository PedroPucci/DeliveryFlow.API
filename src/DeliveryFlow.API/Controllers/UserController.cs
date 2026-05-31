using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Contracts.Dto.UserDto;
using DeliveryFlow.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFlow.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public UserController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [AllowAnonymous]
        [HttpPost()]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add([FromBody] CreateUserRequestDto createUserRequestDto)
        {
            var result = await _uow.UserService.Add(createUserRequestDto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            var result = await _uow.UserService.Update(id, updateUserRequestDto);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var result = await _uow.UserService.Delete(id);

            if (!result.Success)
                return NotFound(result);

            return NoContent();
        }

        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<UserEntity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var result = await _uow.UserService.Get();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _uow.UserService.GetById(id);
            return Ok(result);
        }
    }
}