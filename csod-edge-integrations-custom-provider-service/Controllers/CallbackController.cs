using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Models;
using AonHewitt.Lib;
using System.IO;
using AonHewitt.Lib.Models;
using csod_edge_integrations_custom_provider_service.Models.Assessment;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    //to do: create a quartz scheduler to periodically remove callbacks with limits = 0
    [Produces("application/json")]
    public class CallbackController : Controller
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        protected CallbackRepository CallbackRepository;
        public CallbackController(CallbackRepository callbackRepository, ILogger<CallbackController> logger)
        {
            CallbackRepository = callbackRepository;
            _logger = logger;
        }

        [Route("api/callback/{id}")]
        [HttpPost]
        public async Task<IActionResult> CallbackEndpoint(Guid id)
        {
            _logger.LogInformation("Callback recieved");

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

                var aonHewittClient = new AonHewittClient();
                ApplicantScore applicantScore = null;
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    var content = reader.ReadToEnd();
                    applicantScore = aonHewittClient.ParseApplicantScore(content);
                    _logger.LogInformation("Callback response {0}", content);
                }

                if (applicantScore != null)
                {                    
                    bool isPassed = applicantScore.Pass.Equals("true", StringComparison.OrdinalIgnoreCase) || applicantScore.Pass.Equals("1", StringComparison.OrdinalIgnoreCase) ? true : false;
                    string band = aonHewittClient.GetResourceKeyFromAonBandEnumerator(applicantScore.Band);

                    var result = new AssessmentResults()
                    {
                        IsPass = isPassed,
                        Score = applicantScore.Score,
                        Comments = band,
                        DetailsUrl = applicantScore.TextDescription
                    };                    

                    var callbackMapper = new CallbackMapper();
                    Uri callbackUri = new Uri(callbackMapper.RemapCallback(callbackFromRepo.EdgeCallbackUrl));

                    _logger.LogInformation("Sending callback results to {0} remapped to {1}", callbackFromRepo.EdgeCallbackUrl, callbackUri);

                    var resultJson = JsonConvert.SerializeObject(result);

                    using (var httpClientHandler = new HttpClientHandler())
                    {
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                        using (var client = new HttpClient(httpClientHandler))
                        {
                            client.BaseAddress = callbackUri;
                            client.DefaultRequestHeaders.Add("x-csod-edge-api-key", "PW9totT67fiz87tNMoR2yoncF/M=");

                            HttpRequestMessage request = new HttpRequestMessage();
                            request.Method = HttpMethod.Post;
                            request.Content = new StringContent(resultJson, Encoding.UTF8, "application/json");

                            var response = await client.SendAsync(request);
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                _logger.LogInformation("Successfully sent callback results back to edge!");
                            }
                            else
                            {
                                _logger.LogInformation("Error sending callback results back to edge. {0} : {1}", callbackUri, resultJson);
                                _logger.LogInformation("Error {0} : {1}", response.StatusCode, await response.Content.ReadAsStringAsync());
                            }
                        }
                    }
                }
               
                //finally return status 200 to let requestor know we received the request
                return Ok();
            }
            //returns 400, to tell them that the callback supplied GUID is not in our system
            return BadRequest("Cannot find callback data to process.");
        }
    }
}