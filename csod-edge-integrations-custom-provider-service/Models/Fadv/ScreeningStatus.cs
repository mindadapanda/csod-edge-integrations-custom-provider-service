using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlType(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    [XmlRoot(Namespace = "http://www.cpscreen.com/schemas", IsNullable = false)]
    public class ScreeningStatus
    {
        public string OrderStatus { get; set; }
        public string ResultStatus { get; set; }
        public string Score { get; set; }
    }
}
