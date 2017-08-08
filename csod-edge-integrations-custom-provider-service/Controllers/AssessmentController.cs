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
            int userId = User.GetUserId();

            // Populate these with the list of assessments
            var assessments = new List<AssessmentItem>();

            return Ok(assessments);
        }
    }
}