using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Models;
using Microsoft.EntityFrameworkCore;
using LiteDB;
using csod_edge_integrations_custom_provider_service.Data;
using Microsoft.Extensions.Logging;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    [Produces("application/json")]
    public class UserController : Controller
    {
        protected UserRepository UserRepository;
        protected SettingsRepository SettingsRepository;
        private readonly ILogger _logger;
        private readonly CustomCrypto _crypto;

        public UserController(UserRepository userRepository, SettingsRepository settingsRepository,
            ILogger<UserController> logger, CustomCrypto crypto)
        {
            UserRepository = userRepository;
            SettingsRepository = settingsRepository;
            _logger = logger;
            _crypto = crypto;
        }

        public IActionResult Index()
        {

            return View();
        }

        [Route("api/user")]
        [HttpPost]
        public IActionResult CreateUser([FromBody]User user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Username)
                    || string.IsNullOrWhiteSpace(user.Password))
                {
                    return BadRequest();
                }
                //make username unqiue
                var users = UserRepository.GetAll();
                if (users.Any(x => x.Username.Equals(user.Username)))
                {
                    return BadRequest();
                }
                //make sure to salt and hash the user password
                user.Password = _crypto.GenerateSaltedHash(user.Password);

                UserRepository.CreateUser(user);
            }
            catch
            {
                return BadRequest();
            }

            return new NoContentResult();
        }

        [Route("api/user/login")]
        [HttpPost]
        public IActionResult Login([FromBody]UserLoginRequest loginRequest)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginRequest.Username)
                    || string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    return BadRequest();
                }
                var user = UserRepository.GetUserByUsername(loginRequest.Username);
                if (user != null)
                {
                    if (_crypto.DoPasswordsMatch(loginRequest.Password, user.Password))
                    {
                        return Ok(user);
                    }
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, "Error Logging In.");
                return BadRequest(ex);
            }
            return BadRequest();
        }

        [Route("api/getuserandsettings")]
        [HttpPost]
        public IActionResult GetUserUsingCredentials([FromBody]UserLoginRequest loginRequest)
        {
            if (string.IsNullOrWhiteSpace(loginRequest.Username)
                || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest();
            }
            var user = UserRepository.GetUserByCredentials(loginRequest.Username, loginRequest.Password);
            //let's go fetch the settings as well
            if(user != null)
            {
                var settings = SettingsRepository.GetSettingsUsingUserId(user.Id);
                if(settings == null)
                {
                    settings = new Settings();
                }

                return Ok(new
                {
                    user = user,
                    settings = settings
                });
            }
            return BadRequest();
        }

        [Route("api/user/updatepassword")]
        [HttpPost]
        public IActionResult UpdateUserPassword([FromBody]UpdateUserPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Password)
                || string.IsNullOrWhiteSpace(request.UpdatedPassword))
            {
                return BadRequest();
            }
            var user = UserRepository.GetUserByCredentials(request.Username, request.Password);
            if (user == null)
            {
                return BadRequest();
            }
            //make sure to salt and hash the user password
            user.Password = _crypto.GenerateSaltedHash(request.UpdatedPassword);
            if (UserRepository.UpdateUser(user))
            {
                return Ok();
            }
            return BadRequest();
        }

        [Route("api/user/updateoraddsettings")]
        [HttpPost]
        public IActionResult UpdateOrAddUserSettings([FromBody]UpdateOrAddUserSettingsRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Password)
                || request.Settings == null)
            {
                return BadRequest();
            }
            var user = UserRepository.GetUserByCredentials(request.Username, request.Password);
            if(user == null)
            {
                return BadRequest();
            }
            var settings = SettingsRepository.GetSettingsUsingUserId(user.Id);
            if(settings == null)
            {
                request.Settings.InternalUserId = user.Id;
                SettingsRepository.CreateSettings(request.Settings);

                return Ok();
            }
            if(settings.Id != request.Settings.Id
                || settings.InternalUserId != user.Id)
            {
                return BadRequest();
            }
            SettingsRepository.UpdateSettings(request.Settings);

            return Ok();
        }

        [Route("api/user/gettemplate")]
        [HttpGet]
        public IActionResult GetUserTemplate()
        {
            var user = new User();
            return Ok(user);
        }
    }
}
