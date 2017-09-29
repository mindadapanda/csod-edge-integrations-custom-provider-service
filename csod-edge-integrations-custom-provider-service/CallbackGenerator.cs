using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Models;
using System;

namespace csod_edge_integrations_custom_provider_service
{
    public class CallbackGenerator
    {
        CallbackRepository _callbackRepository;
        public CallbackGenerator(CallbackRepository callbackRepository)
        {
            _callbackRepository = callbackRepository;
        }
        public Guid GenerateCallback(string url, int callbackLimit = 10)
        {
            Guid guid = Guid.NewGuid();
            var callback = new Callback()
            {
                PublicId = guid,
                EdgeCallbackUrl = url,
                Limit = callbackLimit
            };

            _callbackRepository.CreateCallback(callback);
            return guid;
        }
    }
}
