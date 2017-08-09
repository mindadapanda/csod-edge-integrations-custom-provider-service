

using System.ComponentModel.DataAnnotations.Schema;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class User
    {
        //Id is autogen if using the provided LiteDb as a database
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
