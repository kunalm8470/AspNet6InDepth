using Api.Models.Requests;
using Api.Models.Responses;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _accountService = accountService;
            
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult<SignupUserResponseDto>> SignupUserAsync([FromBody] SignupUserRequestDto signupUser)
        {
            CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

            SignupUserResponseDto newAuthUser = await _accountService.SignupUserAsync(signupUser, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, newAuthUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginUserResponseDto>> LoginUserAsync([FromBody] LoginUserRequestDto loginUser)
        {
            CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

            LoginUserResponseDto loginResponse = await _accountService.LoginUserAsync(loginUser, cancellationToken);

            return Ok(loginResponse);
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginUserResponseDto>> GenerateNewAccessTokenBasedOnRefreshTokenAsync([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
            CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

            LoginUserResponseDto accessTokenResponse = await _accountService.RegenerateAccessTokenAsync(refreshTokenRequest, cancellationToken);

            return Ok(accessTokenResponse);
        }

        [HttpPost("revoke")]
        [AllowAnonymous]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RevokeRefreshTokenRequestDto revokeRefreshTokenRequest)
        {
            CancellationToken cancellationToken = _httpContextAccessor.HttpContext.RequestAborted;

            await _accountService.RevokeRefreshTokenAsync(revokeRefreshTokenRequest.RefreshToken, cancellationToken);

            return NoContent();
        }
    }
}
