﻿

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
        public string Account { get; set; }
        public string SourceAccount { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public string InviteUrl { get; set; }
        public string AccountsUrl { get; set; }
        public string AdminUrl { get; set; }
        public string CPLinkUrl { get; set; }
    }
}
