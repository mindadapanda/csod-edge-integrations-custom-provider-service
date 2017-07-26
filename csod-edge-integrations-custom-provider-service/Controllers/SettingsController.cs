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
            return View();
        }

        [Route("api/settings")]
        [HttpGet]
        public IActionResult GetAllSettings()
        {
            var settings = SettingsRepository.GetAll();
            return Ok(settings);
        }

        [Route("api/settings/{id}")]
        [HttpGet]
        public IActionResult GetSettings(int id)
        {
            var settings = SettingsRepository.GetSettings(id);
            if(settings != null)
            {
                return Ok(settings);
            }
            return NotFound();
        }

        [Route("api/settingsUsingHashCode/{hashCode}")]
        [HttpGet]
        public IActionResult GetSettingsUsingHashCode(int hashCode)
        {
            var settings = SettingsRepository.GetSettingsUsingHashCode(hashCode);
            if(settings != null)
            {
                return Ok(settings);
            }
            return NotFound();
        }

        [Route("api/settings")]
        [HttpPost]
        public IActionResult CreateSettings([FromBody]Settings settings)
        {
            try
            {
                //check if there are any other settings tied to the user, we don't want to associate more than 1 setting to a user
                var setting = SettingsRepository.GetSettingsUsingHashCode(settings.UserHashCode);
                if(settings != null)
                {
                    return BadRequest();
                }

                SettingsRepository.CreateSettings(settings);
            }
            catch
            {
                return BadRequest();
            }
            return new NoContentResult();
        }

        [Route("api/settings/{id}")]
        [HttpPut]
        public IActionResult UpdateSettings([FromBody]Settings settings)
        {
            var settingsFromDb = SettingsRepository.GetSettings(settings.Id);
            if(settingsFromDb != null)
            {
                //only update if the settings exists in the db and if the user hash code has not changed
                //we don't want to have orphaned data or allow people to disassociate a user with a settings
                if(settingsFromDb.UserHashCode == settings.UserHashCode)
                {
                    SettingsRepository.UpdateSettings(settings);
                    return new NoContentResult();
                }
            }
            return BadRequest();
        }

        [Route("api/settings/{id}")]
        [HttpDelete]
        public IActionResult DeleteSettings(int id)
        {
            SettingsRepository.DeleteSettings(id);
            return BadRequest();
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