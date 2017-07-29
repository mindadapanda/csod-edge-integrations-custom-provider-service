using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    public class CandidateReport
    {
        public string ApplicationStatus { get; set; }
        public string IsRescreen { get; set; }
        public string ApplicantId { get; set; }
        public string ApplicantLink { get; set; }
        public string ApplicantLinkExpirationDate { get; set; }
        [System.Xml.Serialization.XmlArrayItemAttribute("ClientReference", typeof(CandidateReportClientReference))]
        public List<CandidateReportClientReference> ClientReferences { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("ErrorReport", typeof(FadvAdminError))]
        public FadvAdminError Error { get; set; }
        [System.Xml.Serialization.XmlArrayItemAttribute("UserDefinedField", typeof(CandidateReportUserDefinedField))]
        public List<CandidateReportUserDefinedField> UserDefinedFields { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("PersonalData")]
        public List<CandidateReportPersonalData> PersonalData { get; set; }
        [System.Xml.Serialization.XmlArrayItemAttribute("Quoteback", typeof(CandidateReportQuoteback))]
        public List<CandidateReportQuoteback> Quotebacks { get; set; }
    }
}
