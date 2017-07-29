using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class PersonName
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        public string Salutation { get; set; }
        public string Suffix { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string MiddleName { get; set; }
    }
}
