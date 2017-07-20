

namespace csod_edge_integrations_custom_provider_service.Models
{
    //add settings in here as you see fit
    //model binding and UI generation will be based on this model
    //recommendation of making this a flat object, UI currently will not support nested objects
    //also if you're updating the DB context to not use in memory, you should strongly think about migration strategy when adding new settings
    public class Settings
    {
        public string VendorUrl { get; set; }
        public string VendorUserIdForUser { get; set; }
    }
}
