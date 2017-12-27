using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Assessment
{
    public class ApplicantData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string NamePrefix { get; set; }
        public string NameSuffix { get; set; }
        public string RecruiterEmail { get; set; }
        public OrganizationUnit OrganizationUnit { get; set; }
        public ApplicantContactInfo ContactInfo { get; set; }
        public ApplicantAddress Address { get; set; }
        public ApplicantResume Resume { get; set; }
    }
}
