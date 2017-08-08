using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Assessment
{
    public class InitiateAssessmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string LaunchUrl { get; set; }
    }
}
