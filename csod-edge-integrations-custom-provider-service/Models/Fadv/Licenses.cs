using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class Licenses
    {
        //may need to mark this one up
        [XmlElement]
        public List<License> licenses { get; set; }
    }
}
