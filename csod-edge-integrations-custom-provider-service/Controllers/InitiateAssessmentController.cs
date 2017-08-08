using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Models.Assessment;
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
        private readonly ILogger _logger;
        public InitiateAssessmentController(SettingsRepository settingsRepository, CallbackGenerator callbackGenerator, ILogger<InitiateAssessmentController> logger)
        {
            _settingsRepository = settingsRepository;
            _callbackGenerator = callbackGenerator;
            _logger = logger;
        }

        public IActionResult Post([FromBody]InitiateAssessment order)
        {
            int userId = User.GetUserId();

            var response = new InitiateAssessmentResponse()
            {
                Success = true,
                LaunchUrl = "Assessment launch URL",
                Message = "My message"
            };

            return Ok(response);
        }
    }
}