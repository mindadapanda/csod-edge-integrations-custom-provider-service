using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Models.Fadv
{
    public enum IntegrationOrderResultStatus
    {
        Fail = 0,
        Pass = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4,
        Hold = 5,
        Disabled = 6,
        Unknown = 7
    }
}
