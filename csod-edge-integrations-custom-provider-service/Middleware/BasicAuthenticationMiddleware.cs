using csod_edge_integrations_custom_provider_service.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Middleware
{
    public class BasicAuthenticationMiddleware : AuthenticationMiddleware<BasicAuthenticationOptions>
    {
        public BasicAuthenticationMiddleware(
           RequestDelegate next,
           IOptions<BasicAuthenticationOptions> options,
           ILoggerFactory loggerFactory,
           UrlEncoder encoder)
           : base(next, options, loggerFactory, encoder)
        {            
        }

        protected override AuthenticationHandler<BasicAuthenticationOptions> CreateHandler()
        {
            return new BasicAuthenticationHandler();
        }
    }
}
