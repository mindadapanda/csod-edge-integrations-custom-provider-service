using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    public class CandidateReportPersonalDataPersonName
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        [XmlAttributeAttribute("type")]
        public string Type { get; set; }

    }
}
