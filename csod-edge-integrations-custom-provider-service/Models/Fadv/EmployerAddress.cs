using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class EmployerAddress
    {
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public Municipality Municipality { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
    }
}
