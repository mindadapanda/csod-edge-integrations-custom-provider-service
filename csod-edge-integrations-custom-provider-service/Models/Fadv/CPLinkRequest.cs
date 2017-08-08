using System;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlRoot(Namespace = "http://www.cpscreen.com/schemas", DataType = "http://www.cpscreen.com/schemas CPLinkRequest.xsd")]
    public class CPLinkRequest
    {
        [XmlAttribute("account")]
        public string Account { get; set; }
        [XmlAttribute("userId")]
        public string UserId { get; set; }
        [XmlAttribute("password")]
        public string Password { get; set; }
        [XmlElement]
        public string Type { get; set; }
        [XmlElement]
        public string ProviderReferenceId { get; set; }
        [XmlElement]
        public viewAs ViewAs { get; set; }
    }
}
