using Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Api.SecurityHandler;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserService userService
    ) : base(options, logger, encoder, clock)
    {
        _userService = userService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string authorizationHeader = Request.Headers["Authorization"].ToString();

        // If auth header is missing or malformed
        if (
            string.IsNullOrEmpty(authorizationHeader)
            || string.IsNullOrWhiteSpace(authorizationHeader)
            || !authorizationHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase)
        )
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;

            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"localhost\"");

            return Task.FromResult(AuthenticateResult.Fail("Invalid authentication header"));
        }

        string token = authorizationHeader.Substring("Basic ".Length).Trim();

        string decodedCrdentials = Encoding.UTF8.GetString(Convert.FromBase64String(token));

        string[] credentials = decodedCrdentials.Split(':');

        string username = credentials[0];

        string password = credentials[1];

        (bool isAuthenticated, string loggedInRole) = _userService.AuthenticateUser(username, password);

        if (!isAuthenticated)
        {
            Response.StatusCode = StatusCodes.Status401Unauthorized;

            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"localhost\"");

            return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
        }

        List<Claim> claims = new()
        {
            new Claim("name", username),
            new Claim(ClaimTypes.Role, loggedInRole)
        };

        ClaimsIdentity claimsIdentity = new(claims, "Basic");

        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
    }
}
