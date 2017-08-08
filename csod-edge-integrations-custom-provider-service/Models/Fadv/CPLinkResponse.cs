using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlRootAttribute(Namespace = "http://www.cpscreen.com/schemas")]
    public class CPLinkResponse
    {
        [XmlElement]
        public string Status { get; set; }
        [XmlElement]
        public string Link { get; set; }
        [XmlElement("ErrorReport")]
        public FadvAdminError Error { get; set; }
    }
}
