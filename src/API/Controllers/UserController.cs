using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spentir.Application.Services.Interfaces;
using Spentir.Application.DTOs;

namespace Spentir.API.Controllers.Public
{
    [AllowAnonymous]
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("signup")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var UserTokenDto = await _userService.CreateUser(dto);
            return StatusCode(StatusCodes.Status201Created, UserTokenDto);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto dto)
        {
            var UserTokenDto = await _userService.LoginUser(dto);
            return Ok(UserTokenDto);
        }
    }
}