using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlRootAttribute(Namespace = "http://www.cpscreen.com/schemas")]
    public class CandidateInvitations
    {
        [XmlAttribute("userId")]
        public string UserId { get; set; }
        [XmlAttribute("password")]
        public string Password { get; set; }
        [XmlAttribute("account")]
        public string Account { get; set; }
        public CandidateInvitation CandidateInvitation { get; set; }
    }
}
