using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class RequestingAccount
    {
        public string UserId { get; set; }
        public string Account { get; set; }
        public ContactMethod ContactMethod { get; set; }
    }
}
