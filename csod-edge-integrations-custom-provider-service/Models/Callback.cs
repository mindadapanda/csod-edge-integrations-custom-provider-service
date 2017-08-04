using csod_edge_integrations_custom_provider_service.Models.EdgeBackgroundCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class Callback
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string EdgeCallbackUrl { get; set; }
        //the url that was generated for a vendor to call this service's callback endpoint
        public string GeneratedCallbackUrl { get; set; }
        public int Limit { get; set; }
        //the userid tied to the callback thus we can get the settings
        public int UserId { get; set; }
        //the callback data that came from csod
        public CallbackData CallbackDataFromCsod { get; set; } 
    }
}
