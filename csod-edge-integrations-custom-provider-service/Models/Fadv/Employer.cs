using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class Employer
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
        public string EmployerName { get; set; }
        public string LocalLangEmployerName { get; set; }
        public string EmployeeID { get; set; }
        public EmployerAddress EmployerAddress { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public DatesOfEmployment DatesOfEmployment { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public Supervisor Supervisor { get; set; }
        public string ReasonForLeaving { get; set; }
        public string BusinessType { get; set; }
        public string JobType { get; set; }
        public string Agency { get; set; }
        public string Duties { get; set; }

    }
}
