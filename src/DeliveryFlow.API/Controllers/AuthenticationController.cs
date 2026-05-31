using DeliveryFlow.Application.Abstractions.Persistence;
using DeliveryFlow.Application.Contracts.Dto.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryFlow.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public AuthenticationController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDTO user)
        {
            var result = await _uow.AuthenticationService.Login(user);
            return Ok(result);
        }
    }
}