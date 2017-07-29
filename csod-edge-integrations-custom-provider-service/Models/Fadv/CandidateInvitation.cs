using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class CandidateInvitation
    {
        [XmlElement("CandidateStatusNotificationUrl")]
        public string CandidateStatusNotificationUrl { get; set; }
        [XmlElement("CaseNotificationUrl")]
        public string CaseNotificationUrl { get; set; }
        [XmlElement("UserDefinedFields")]
        public UserDefinedFields UserDefinedFields { get; set; }
        public ClientReferences ClientReferences { get; set; }
        [XmlElement("Quotebacks")]
        public Quotebacks Quotebacks { get; set; }
        public RequestingAccount RequestingAccount { get; set; }
        public string CandidateType { get; set; }
        public PersonalData PersonalData { get; set; }
        public ExpectedCompensation ExpectedCompensation { get; set; }
        public string BackgroundSearchPackageId { get; set; }
    }
}
