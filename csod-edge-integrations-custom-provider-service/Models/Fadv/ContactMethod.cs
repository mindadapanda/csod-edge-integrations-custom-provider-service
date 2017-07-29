using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class ContactMethod
    {
        public string InternetEmailAddress { get; set; }
        public Telephone Telephone { get; set; }
        public AddressInfo PostalAddress { get; set; }
    }
}
