using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlType(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    public class Screening
    {
        [XmlElement("ScreeningStatus")]
        public List<ScreeningStatus> ScreeningStatus { get; set; }
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("subtype")]
        public string SubType { get; set; }
        [XmlAttribute("desc")]
        public string Description { get; set; }
    }
}
