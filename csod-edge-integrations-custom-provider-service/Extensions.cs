using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service
{
    public static class Extensions
    {
        public static int GetUserId(this ClaimsPrincipal principle)
        {            
            return int.Parse(principle.Claims.Single(x => x.Type == "id").Value);
        }
    }
}
