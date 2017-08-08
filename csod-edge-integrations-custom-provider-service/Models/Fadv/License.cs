using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class License
    {
        [XmlAttribute("validFrom")]
        public string ValidFrom { get; set; }
        [XmlAttribute("validto")]
        public string ValidTo { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseName { get; set; }
        public string LicenseDescription { get; set; }
        public string CountryCode { get; set; }
        public string LicenseRegion { get; set; }
        public string LicenseType { get; set; }
        public string CandidateNameOnLicense { get; set; }
        public string LicensingAgency { get; set; }
        public string LocalLanguageAgencyName { get; set; }
        public Institute Institute { get; set; }
        public string LicenseStatusId { get; set; }
        public LicenseAuthorityAddress LicenseAuthorityAddress { get; set; }
        public string IsValidLicense { get; set; }
        public string LicenseClass { get; set; }
    }
}
