using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChristmasPresents
{
    /// <summary>
    /// Default values used by Basic Access Authentication scheme.
    /// </summary>
    public class BasicAuthenticationDefaults
    {
        /// <summary>
        /// Basic authentication scheme
        /// </summary>
        public const string AuthenticationScheme = "Basic";
        /// <summary>
        /// Default authentication display name
        /// </summary>
        public static readonly string DisplayName = "Basic";
    }
    public class CustomAuthenticationOptions : AuthenticationSchemeOptions
    {
        public CustomAuthenticationOptions()
        {
        }
    }

    internal class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        private const string _Scheme = "MyScheme";

        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorizationHeader = Request.Headers["Authentication"];

            if (string.IsNullOrEmpty(authorizationHeader))
                return AuthenticateResult.Fail("NOT AUTHORIZED");

            authorizationHeader = authorizationHeader.Replace("Basic ", "");

            // create a ClaimsPrincipal from your header
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "My Name")
            };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
            var ticket = new AuthenticationTicket(claimsPrincipal,
                new AuthenticationProperties { IsPersistent = false },
                Scheme.Name
            );

            if (authorizationHeader == "ASDASDASD")
                return AuthenticateResult.Success(ticket);
            
            return AuthenticateResult.Fail("NOT AUTHORIZED");
        }
    }
}
