using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.cpscreen.com/schemas")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.cpscreen.com/schemas", IsNullable = false)]
    public class GetAccountsResponse
    {
        public string Status { get; set; }
        [System.Xml.Serialization.XmlArrayItemAttribute("Account", typeof(Account))]
        public List<Account> AccountDetails { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("ErrorReport", typeof(FadvAdminError))]
        public FadvAdminError Error { get; set; }
    }
}
