using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service
{
    public class CallbackMapper
    {
        public CallbackMapper()
        {

        }

        public string RemapCallback(string url)
        {
            url = url.Replace("lax-dev-ex.csod.com", "52.53.135.240");
            url = url.Replace("lax-qar-ex.csod.com", "52.53.135.240:8001");
            url = url.Replace("lax-qanr-ex.csod.com", "52.53.135.240:8002");
            url = url.Replace("lax-qap-ex.csod.com", "52.53.135.240:8003");
            url = url.Replace("lax-qah-ex.csod.com", "52.53.135.240:8004");

            return url;
        }
    }
}
