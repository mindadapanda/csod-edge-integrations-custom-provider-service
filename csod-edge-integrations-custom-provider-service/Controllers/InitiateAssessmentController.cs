using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Models.Assessment;
using AonHewitt.Lib;
using AonHewitt.Lib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "Basic")]
    [Produces("application/json")]
    [Route("api/initiateassessment")]
    public class InitiateAssessmentController : Controller
    {
        SettingsRepository _settingsRepository;
        CallbackGenerator _callbackGenerator;
        Options _options;
        private readonly ILogger _logger;
        public InitiateAssessmentController(SettingsRepository settingsRepository, CallbackGenerator callbackGenerator, IOptions<Options> options, ILogger<InitiateAssessmentController> logger)
        {
            _settingsRepository = settingsRepository;
            _callbackGenerator = callbackGenerator;
            _options = options.Value;
            _logger = logger;
        }

        public IActionResult Post([FromBody]InitiateAssessment request)
        {
            InitiateAssessmentResponse response = null;

            var userId = User.GetUserId();
            var settings = _settingsRepository.GetSettingsUsingHashCode(userId);

            _logger.LogDebug("Initiate assessment called for user {0}", userId);

            var aonHewittSettings = new AonHewittSettings()
            {
                ClientId = settings.ClientId,
                VendorCode = settings.VendorCode,
                ServiceBaseUrl = settings.ServiceBaseUrl
            };

            string message = "Unknown Error";
            if (settings != null)
            {
                var callbackGuid = _callbackGenerator.GenerateCallback(request.CallbackUrl);
                var callbackUrl = string.Format("{0}?id={1}", _options.BaseCallbackUrl, callbackGuid);

                _logger.LogDebug("Callback URL generated: {0}", callbackUrl);

                var aonHewittClient = new AonHewittClient();

                var body = aonHewittClient.CreateRegisterCandidateRequestBody(aonHewittSettings.VendorCode, aonHewittSettings.ClientId, request.AssessmentId, request.TrackingId, callbackUrl);
                var result = aonHewittClient.SubmitReqest(aonHewittSettings.ServiceBaseUrl, RequestMethod.POST, body);

                _logger.LogDebug("Request submitted to Aon with results {0}", result);

                Serializer serializer = new Serializer();

                AcknowledgeAssessmentOrder successBody = null;
                try
                {
                    successBody = serializer.Deserialize<AcknowledgeAssessmentOrder>(result);

                    response = new InitiateAssessmentResponse()
                    {
                        Success = true,
                        Message = "Request was successful see LaunchUrl property",
                        LaunchUrl = successBody.DataArea.AssessmentOrder.AssessmentAccess.AssessmentCommunication.FirstOrDefault()
                    };
                }
                catch
                {

                }

                ConfirmBOD errorBody = null;
                try
                {
                    errorBody = serializer.Deserialize<ConfirmBOD>(result);

                    response = new InitiateAssessmentResponse()
                    {
                        Success = false,
                        Message = errorBody.DataArea.BOD.BODFailureMessage.NounFailureMessage.ErrorProcessMessage.Description
                    };
                }
                catch
                {

                }

                return Ok(response);
            }
            else
            {
                message = "No Settings Defined for Aon";
            }

            response = new InitiateAssessmentResponse()
            {
                Success = false,
                Message = message
            };

            return Ok(response);
        }
    }
}