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
    [Produces("application/json")]
    public class SettingsController : Controller
    {
        protected SettingsRepository SettingsRepository;

        public SettingsController(SettingsRepository settingsRepository)
        {
            SettingsRepository = settingsRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "user");
        }
        

        [Route("api/settings/gettemplate")]
        [HttpGet]
        public IActionResult GetSettingsTemplate()
        {
            var settings = new Settings();
            return Ok(settings);
        }

    }
}