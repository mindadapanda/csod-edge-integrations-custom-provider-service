using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class SchoolOrInstitution
    {
        [XmlAttribute("schoolType")]
        public string SchoolType { get; set; }
        public string SchoolName { get; set; }
        public string LocalLangSchoolName { get; set; }
        public LocationSummary LocationSummary { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public Degree Degree { get; set; }
        public DatesOfAttendance DatesOfAttendance { get; set; }
    }
}
