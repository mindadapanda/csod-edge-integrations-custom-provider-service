using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlType(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    [XmlRoot(Namespace = "http://www.cpscreen.com/schemas", IsNullable = false)]
    public class BackgroundReports
    {
        [XmlElement("BackgroundReportPackage")]
        public List<BackgroundReportPackage> BackgroundReportPackage { get; set; }
        [XmlAttribute("userId")]
        public string UserId { get; set; }
        [XmlAttribute("account")]
        public string Account { get; set; }
    }
}
