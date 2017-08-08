using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    [XmlRootAttribute(Namespace = "http://www.cpscreen.com/schemas", IsNullable = false)]
    public class GetAccountsRequest
    {
        [XmlElementAttribute("ChangePassword")]
        public ChoicePointAdminRequestChangePassword[] ChangePassword { get; set; }
        [XmlElementAttribute("PackageDetail")]
        public ChoicePointAdminRequestPackageDetail[] PackageDetail { get; set; }
        [XmlAttribute("userId")]
        public string UserId { get; set; }
        [XmlAttribute("account")]
        public string Account { get; set; }
        [XmlAttribute("password")]
        public string Password { get; set; }
        public string SourceAccount { get; set; }
        [XmlElement("type")]
        public string Type { get; set; }
    }
}
