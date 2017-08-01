using System;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class viewAs
    {
        [XmlElement]
        public string Account { get; set; }
        public string UserId { get; set; }
    }
}
