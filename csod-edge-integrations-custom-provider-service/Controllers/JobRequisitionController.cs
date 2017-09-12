using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Models;
using CsodATSCoreLib;
using CsodATSCoreLib.Models;
using Microsoft.AspNetCore.Mvc;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    [Produces("application/json")]
    public class JobRequisitionController : Controller
    {
        protected UserRepository _userRepository;
        protected SettingsRepository _settingsRepository;

        public JobRequisitionController(UserRepository userRepository, SettingsRepository settingsRepository)
        {
            _userRepository = userRepository;
            _settingsRepository = settingsRepository;
        }

        [Route("api/jobrequisitions")]
        [HttpPost]
        public IActionResult GetJobRequisitions([FromBody]UserLoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Username)
            || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest();
            }
            var user = _userRepository.GetUserByCredentials(loginRequest.Username, loginRequest.Password);
            if (user != null)
            {
                var settings = _settingsRepository.GetSettingsUsingUserId(user.Id);
                if (settings != null)
                {

                    CsodATSClient client = new CsodATSClient(settings.EndpointUrl, "ca", settings.ApiKey, settings.ApiSecret);

                    var jobrequisitions = client.GetJobRequisitions(10, 1, JobRequisitionStatus.Open);
                    return Ok(new
                    {
                        user = user,
                        jobrequisitions = jobrequisitions
                    });
                }
            }
            return BadRequest();
        }
    }
}
