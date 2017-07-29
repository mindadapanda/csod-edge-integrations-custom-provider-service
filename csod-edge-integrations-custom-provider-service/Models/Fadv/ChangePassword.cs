using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    [XmlRootAttribute(Namespace = "http://www.cpscreen.com/schemas")]
    public class ChangePassword
    {
        public string Account { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string StatusId { get; set; }
        public FadvAdminError Error { get; set; }
    }
}
