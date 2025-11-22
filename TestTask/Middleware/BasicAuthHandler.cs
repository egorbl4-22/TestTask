using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace TestTask.Middleware
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IConfiguration configuration)
            : base(options, logger, encoder)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                
                if (authHeader.Scheme != "Basic" || authHeader.Parameter == null)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Scheme"));
                }

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

                if (credentials.Length != 2)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
                }

                var username = credentials[0];
                var password = credentials[1];

                var adminUsername = _configuration["Admin:Username"] ?? "admin";
                var adminPassword = _configuration["Admin:Password"] ?? "admin123";

                if (username != adminUsername || password != adminPassword)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
                }

                var claims = new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Admin")
                };

                var identity = new System.Security.Claims.ClaimsIdentity(claims, Scheme.Name);
                var principal = new System.Security.Claims.ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}

