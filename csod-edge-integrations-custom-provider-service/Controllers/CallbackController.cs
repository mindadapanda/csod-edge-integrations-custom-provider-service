using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Models;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    //to do: create a quartz scheduler to periodically remove callbacks with limits = 0
    [Produces("application/json")]
    public class CallbackController : Controller
    {
        protected CallbackRepository CallbackRepository;
        public CallbackController(CallbackRepository callbackRepository)
        {
            CallbackRepository = callbackRepository;
        }

        [Route("api/callback/{id}")]
        [HttpPost]
        public IActionResult CallbackEndpoint(Guid id)
        {
            var callbackFromRepo = CallbackRepository.GetCallbackByGuid(id);
            if(callbackFromRepo != null)
            {
                //maybe delete callbacks that have their limits reached?
                //or you can choose to retain all callbacks and not do anything
                if(callbackFromRepo.Limit == 0)
                {
                    return BadRequest("Callback limit reached!");
                }
                //do all your work here
                //read the request body for data and then act and process on it
                //finally create a data payload that edge would understand and set it to callbackFromRepo.EdgeCallbackUrl

                //don't modify we're going to decrement the limit by 1
                CallbackRepository.DecrementCallbackLimit(callbackFromRepo.Id);



                //finally return status 200 to let requestor know we received the request
                return Ok();
            }
            //returns 400, to tell them that the callback supplied GUID is not in our system
            return BadRequest("Cannot find callback data to process.");
        }

        private string GenerateCallback(string edgeCallbackUrl, int callbackLimit = 10)
        {
            var request = HttpContext.Request;
            if (request == null)
            {
                throw new Exception("request context cannot be null");
            }
            if (string.IsNullOrWhiteSpace(edgeCallbackUrl))
            {
                throw new Exception("edge callback url cannot be empty string");
            }
            if (callbackLimit > 100 || callbackLimit <= 0)
            {
                callbackLimit = 100;
            }
            var publicId = Guid.NewGuid();
            var callback = new Callback()
            {
                PublicId = publicId,
                EdgeCallbackUrl = edgeCallbackUrl,
                Limit = callbackLimit
            };
            CallbackRepository.CreateCallback(callback);

            var callbackUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}/api/callback/{publicId}";
            return callbackUrl;
        }

    }
}