using csod_edge_integrations_custom_provider_service.Data;
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
        private readonly UserRepository _userRepository;
        private readonly CustomCrypto _crypto;
        public BasicAuthenticationMiddleware(
           RequestDelegate next,
           IOptions<BasicAuthenticationOptions> options,
           ILoggerFactory loggerFactory,
           UrlEncoder encoder,
           UserRepository userRepository,
           CustomCrypto crypto)
           : base(next, options, loggerFactory, encoder)
        {
            _userRepository = userRepository;
            _crypto = crypto;
        }

        protected override AuthenticationHandler<BasicAuthenticationOptions> CreateHandler()
        {
            return new BasicAuthenticationHandler(_userRepository, _crypto);
        }
    }
}
