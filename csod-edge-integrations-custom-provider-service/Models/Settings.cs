

using System.Collections.Generic;

namespace csod_edge_integrations_custom_provider_service.Models
{
    //add settings in here as you see fit
    //model binding and UI generation will be based on this model
    //recommendation of making this a flat object, UI currently will not support nested objects
    public class Settings
    {
        //Id is autogen if using the provided LiteDb as a database
        public int Id { get; set; }
        //Using the user class generated Hash Code to match a setting to a user
        public int UserHashCode { get; set; }



        public string VendorCode { get; set; }
        public string ClientId { get; set; }
        public string ServiceBaseUrl { get; set; }        
        public string Assessments { get; set; }
    }
}
