using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public class Military
    {
        public string Branch { get; set; }
        public string CurrentlyServing { get; set; }
        public string CountryCode { get; set; }
        public string RankAchieved { get; set; }
        public string DischargeStatusId { get; set; }
        public string AreaOfExpertise { get; set; }
        public DatesOfService DatesOfService { get; set; }
    }
}
