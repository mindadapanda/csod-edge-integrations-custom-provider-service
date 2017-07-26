using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}
