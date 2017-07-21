

namespace csod_edge_integrations_custom_provider_service.Models
{
    //add settings in here as you see fit
    //model binding and UI generation will be based on this model
    //recommendation of making this a flat object, UI currently will not support nested objects
    //if you're updating the DB context to not use something other than in memory, you should strongly think about migration strategy when adding new settings
    //you might encounter errors when setting values because the UI doesn't understand the type of the property
    //you have two options, one is to edit the UI to bind the correct type when posting data
    //two, simply convert the type yourself on the way up from the UI, this way you don't have to compromise your strongly typed properties here
    public class Settings
    {
        public int Id { get; set; }
        public string VendorUrl { get; set; }
        public string VendorUserIdForUser { get; set; }
    }
}
