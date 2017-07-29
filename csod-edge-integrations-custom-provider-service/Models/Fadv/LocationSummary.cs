using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class LocationSummary
    {
        public DeliveryAddress DeliveryAddress { get; set; }
        public string CountryCode { get; set; }
        public Municipality Municipality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }

    }
}
