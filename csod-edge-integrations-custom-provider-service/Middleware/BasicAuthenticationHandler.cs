using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Middleware
{

    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly UserRepository _userRepository;
        private readonly CustomCrypto _crypto;
        public BasicAuthenticationHandler(UserRepository userRepository, CustomCrypto crypto)
        {
            _userRepository = userRepository;
            _crypto = crypto;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {            
            var authHeader = (string)this.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                //Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                var user = _userRepository.GetUserByUsername(username);
                if (user != null)
                {
                    if (_crypto.DoPasswordsMatch(password, user.Password))
                    {
                        var identity = new ClaimsIdentity("Basic");
                        identity.AddClaim(new Claim("id", user.Id.ToString()));

                        var principle = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principle, new AuthenticationProperties(), Options.AuthenticationScheme);
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    }
                }
                return Task.FromResult(AuthenticateResult.Fail("No valid user."));
            }

            this.Response.Headers["WWW-Authenticate"] = "Basic realm=\"yourawesomesite.net\"";
            return Task.FromResult(AuthenticateResult.Fail("No credentials."));
        }
    }
}
