using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlRootAttribute(Namespace = "http://www.cpscreen.com/schemas")]
    public class PackageDetail
    {
        public string Account { get; set; }
        public string PackageId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        [System.Xml.Serialization.XmlArrayItemAttribute("Component", typeof(Component))]
        public List<Component> Components { get; set; }
    }
    
    public class Component
    {
        public string Type { get; set; }
        public string Subtype { get; set; }
        public string Name { get; set; }
    }
}
