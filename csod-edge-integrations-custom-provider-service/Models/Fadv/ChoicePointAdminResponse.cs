using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlRootAttribute(Namespace = "http://www.cpscreen.com/schemas")]
    public class ChoicePointAdminResponse
    {
        [XmlElement("PackageDetail")]
        public List<PackageDetail> PackageDetails { get; set; }
        public ChangePassword ChangePassword { get; set; }
        public string Status { get; set; }
        public FadvAdminError Error { get; set; }
    }
}
