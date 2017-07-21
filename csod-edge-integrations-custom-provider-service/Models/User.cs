

using System.ComponentModel.DataAnnotations.Schema;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int SettingsId { get; set; }
        [ForeignKey("SettingsId")]
        public Settings Settings { get; set; }
    }
}
