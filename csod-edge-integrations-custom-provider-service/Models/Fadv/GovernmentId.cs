using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class GovernmentId
    {
        [XmlAttribute("issuingCountry")]
        public string IssuingCountry { get; set; }
        [XmlAttribute("issuingAuthority")]
        public string IssuingAuthority { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
