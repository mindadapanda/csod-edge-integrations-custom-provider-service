using csod_edge_integrations_custom_provider_service.Models.Fadv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models
{
    public class BackgroundCheckDebugData
    {
        public int Id { get; set; }
        //the callback guid used to identify callbacks to the request that was sent out
        public Guid CallbackGuid { get; set; }
        public string BackgroundCheckRequestToFadvRawXml { get; set; }
        public string BackgroundCheckResponseFromFadvRawXml { get; set; }
        public List<string> BackgroundReportsFromFadvRawXml { get; set; }
    }
}
