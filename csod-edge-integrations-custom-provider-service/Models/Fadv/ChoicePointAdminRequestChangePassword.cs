using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    public class ChoicePointAdminRequestChangePassword
    {
        public string Account { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
