using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class GetReportUrlRequest
    {
        public string ProviderReferenceId { get; set; }
        public string RecruiterEmail { get; set; }
        public string OrderingAccount { get; set; }
    }
}
