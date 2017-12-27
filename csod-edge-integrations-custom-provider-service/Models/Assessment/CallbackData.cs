using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Assessment
{
    public class CallbackData
    {
        public string TrackingId { get; set; }
        public string CallbackUrl { get; set; }
        public string ApplicantRefId { get; set; }
        public string ApplicantRefUserId { get; set; }
        public string ApplicantRefOrderId { get; set; }
    }
}
