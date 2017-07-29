using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class PersonalData
    {
        public Locale Locale { get; set; }
        public PersonName PersonName { get; set; }
        public ContactMethod ContactMethod { get; set; }
        public List<PostalAddress> PostalAddresses { get; set; }
        [XmlElement("PostalAddress")]
        public AddressInfo PostalAddress { get; set; }
        public DemographicDetail DemographicDetail { get; set; }
        public Eeoc EEOC { get; set; }
        public EmploymentHistory EmploymentHistory { get; set; }
        public EducationHistory EducationHistory { get; set; }
        public References References { get; set; }
        public MilitaryHistory MilitaryHistory { get; set; }
        public Licenses Licenses { get; set; }
        public AdditionalInformation AdditionalInformation { get; set; }
    }
}
