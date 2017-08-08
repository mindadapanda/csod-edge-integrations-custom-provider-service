

namespace csod_edge_integrations_custom_provider_service.Models
{
    //add settings in here as you see fit
    //model binding and UI generation will be based on this model
    //recommendation of making this a flat object, UI currently will not support nested objects
    public class Settings
    {
        //Id is autogen if using the provided LiteDb as a database
        public int Id { get; set; }
        //linking to userId property on user
        public int InternalUserId { get; set; }
        public string VendorUrl { get; set; }
        public string VendorUserIdForUser { get; set; }
    }
}
