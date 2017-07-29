using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class Reference
    {
        public PersonName PersonName { get; set; }
        public string ReferenceType { get; set; }
        public PostalAddress PostalAddresses { get; set; }
        public string YearsAcquainted { get; set; }
        public string EmployerName { get; set; }
        public string EmploymentTitle { get; set; }
        public ContactMethod ContactMethod { get; set; }
    }
}
