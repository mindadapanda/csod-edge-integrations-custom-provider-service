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
        public int Limit { get; set; }
        //the callback data that came from csod
        public CallbackData CallbackDataFromCsod { get; set; } 
    }
}
