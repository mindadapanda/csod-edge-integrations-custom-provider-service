using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class UpdateUserPasswordRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UpdatedPassword { get; set; }
    }
}
