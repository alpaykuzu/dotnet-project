using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using YirmibesYazilim.Framework.Models.Responses;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<NoContent>>> Register(RegisterRequest req)
        {
            return Ok(await _userService.RegisterAsync(req));
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<LoginResponse>>> Login(LoginRequest req)
        {
            return Ok(await _userService.LoginAsync(req));
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<Response<NoContent>>> RefreshToken(TokenRequest tokenRequest)
        {
            return Ok(await _userService.RefreshTokenAsync(tokenRequest));
        }
    }
}
