using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlRoot(Namespace = "http://www.cpscreen.com/schemas")]
    public class FadvAdminError
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
