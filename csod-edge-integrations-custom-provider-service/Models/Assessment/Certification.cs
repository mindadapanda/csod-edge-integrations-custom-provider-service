using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Assessment
{
    public class Certification
    {
        public string Name { get; set; }
        public string Organization { get; set; }
        public string IssuedDate { get; set; }
        public string ExpirationDate { get; set; }
        public string Description { get; set; }
    }
}
