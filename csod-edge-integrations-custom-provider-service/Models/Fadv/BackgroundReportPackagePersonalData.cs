using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlType(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    public class BackgroundReportPackagePersonalData
    {
        [XmlElement("PersonName")]
        public List<PersonalDataPersonName> PersonName { get; set; }
        [XmlArrayItem("GovernmentId", typeof(DemographicDetailGovernmentId))]
        public List<DemographicDetailGovernmentId> DemographicDetail { get; set; }
    }
}
