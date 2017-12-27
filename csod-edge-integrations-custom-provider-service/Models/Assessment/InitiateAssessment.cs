using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Assessment
{
    public class InitiateAssessment
    {
        public string AssessmentId { get; set; }
        public ApplicantData ApplicantData { get; set; }
        public CallbackData CallbackData { get; set; }
    }
}
