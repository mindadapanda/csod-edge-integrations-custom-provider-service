using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Assessment
{
    public class OrganizationUnit
    {
        public string Division { get; set; }
        public OrganizationUnitLocation Location { get; set; }
        public string CostCenter { get; set; }
        public string Position { get; set; }
        public string Grade { get; set; }
    }
}
