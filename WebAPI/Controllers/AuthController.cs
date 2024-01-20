using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using Schema;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> LoginToken(Login loginDto)
        {
            var result = await _authenticationService.LoginAsync(loginDto);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.LoginByRefreshTokenAsync(refreshTokenDto.RefreshToken);

            return Ok(result);

        }

    }
}
