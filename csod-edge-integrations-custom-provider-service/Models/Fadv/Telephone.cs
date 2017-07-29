using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class Telephone
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        public string Number { get; set; }
        public string Extension { get; set; }
    }
}
