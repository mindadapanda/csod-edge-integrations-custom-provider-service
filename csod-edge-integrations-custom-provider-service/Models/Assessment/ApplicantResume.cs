using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Assessment
{
    public class ApplicantResume
    {
        public IEnumerable<ProfessionalExperience> ProfessionalExperiences { get; set; }
        public IEnumerable<Education> Educations { get; set; }
        public IEnumerable<Certification> Certifications { get; set; }
    }
}
