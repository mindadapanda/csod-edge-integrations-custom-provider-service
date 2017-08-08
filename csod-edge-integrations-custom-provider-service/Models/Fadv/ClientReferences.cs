using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class ClientReferences
    {
        [XmlElement("ClientReference")]
        public List<ClientReference> ClientReference { get; set; }
    }
}
