using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Models.Assessment;
using Microsoft.AspNetCore.Authorization;
using csod_edge_integrations_custom_provider_service.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "Basic")]
    [Produces("application/json")]
    [Route("api/assessment")]
    public class AssessmentController : Controller
    {
        private readonly ILogger _logger;
        SettingsRepository _settingsRepository;
        public AssessmentController(SettingsRepository settingsRepository, ILogger<AssessmentController> logger)
        {
            _settingsRepository = settingsRepository;
            _logger = logger;
        }

        public IActionResult Get()
        {
            _logger.LogInformation("Get assessment request.");
            var userId = User.GetUserId();
            var settings = _settingsRepository.GetSettingsUsingUserId(userId);

            List<AssessmentItem> assessments = null;

            try
            {
                assessments = JsonConvert.DeserializeObject<List<AssessmentItem>>(settings.Assessments);
            }
            catch
            {

            }

            if (assessments == null)
                assessments = new List<AssessmentItem>();

            return Ok(assessments);
        }
    }
}