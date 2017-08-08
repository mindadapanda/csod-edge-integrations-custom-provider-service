using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Assessment
{
    public class AssessmentResults
    {
        public bool IsPass { get; set; }
        public string Score { get; set; }
        public string DetailsUrl { get; set; }
        public string Comments { get; set; }

    }
}
