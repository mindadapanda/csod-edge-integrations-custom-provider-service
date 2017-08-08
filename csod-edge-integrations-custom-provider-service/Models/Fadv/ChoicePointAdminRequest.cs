using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.cpscreen.com/schemas", IsNullable = false)]
    public class ChoicePointAdminRequest
    {
        [System.Xml.Serialization.XmlElementAttribute("ChangePassword")]
        public ChangePassword ChangePassword { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("PackageDetail")]
        public PackageDetail PackageDetail { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute("userId")]
        public string UserId { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute("account")]
        public string Account { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute("password")]
        public string Password { get; set; }
    }
}
