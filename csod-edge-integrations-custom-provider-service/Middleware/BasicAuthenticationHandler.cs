using csod_edge_integrations_custom_provider_service.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Middleware
{

    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public BasicAuthenticationHandler()
        {
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

                if (username == "test" && password == "test")
                {
                    var user = new GenericPrincipal(new GenericIdentity("test"), null);
                    var ticket = new AuthenticationTicket(user, new AuthenticationProperties(), Options.AuthenticationScheme);
                    return Task.FromResult(AuthenticateResult.Success(ticket));           
                }
                else
                {

                    //var dbUser = _userContext.Users.SingleOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                    //if (dbUser != null)
                    //{
                    //    if (dbUser.Password.Equals(password))
                    //    {
                    //        var user = new CustomPrinciple(dbUser);
                    //        var ticket = new AuthenticationTicket(user, new AuthenticationProperties(), Options.AuthenticationScheme);
                    //        return Task.FromResult(AuthenticateResult.Success(ticket));
                    //    }
                    //}
                }

                return Task.FromResult(AuthenticateResult.Fail("No valid user."));
            }

            this.Response.Headers["WWW-Authenticate"] = "Basic realm=\"yourawesomesite.net\"";
            return Task.FromResult(AuthenticateResult.Fail("No credentials."));
        }
    }
}
