using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class CandidateReportPersonalData
    {
        public string EEOC { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("PersonName")]
        public List<CandidateReportPersonalDataPersonName> PersonName { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("DemographicDetail")]
        public List<CandidateReportDemographicDetail> DemographicDetail { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("ContactMethod")]
        public List<CandidateReportPersonalDataContactMethod> ContactMethod { get; set; }
    }
}
