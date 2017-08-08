using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class DemographicDetail
    {
        public string DateOfBirth { get; set; }
        public string HasSSN { get; set; }
        public GovernmentId GovernmentId { get; set; }
        public string Gender { get; set; }
    }
}
