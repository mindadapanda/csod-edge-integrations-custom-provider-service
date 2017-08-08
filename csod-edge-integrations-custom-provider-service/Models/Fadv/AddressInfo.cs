using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class AddressInfo
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("validFrom")]
        public string ValidFrom { get; set; }
        [XmlAttribute("validTo")]
        public string ValidTo { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public Municipality Municipality { get; set; }
        public DeliveryAddress DeliveryAddress { get; set; }
    }
}
