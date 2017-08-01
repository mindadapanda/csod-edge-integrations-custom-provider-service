using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlType(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    public class ScreeningResults
    {
        public string InternetWebAddress { get; set; }
        [XmlAttribute("mediaType")]
        public string MediaType { get; set; }
    }
}
