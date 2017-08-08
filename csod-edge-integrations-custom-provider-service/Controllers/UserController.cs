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
    //used by the UI to manage users
    //to secure this controller Filters is strongly suggested
    [Produces("application/json")]
    public class UserController : Controller
    {
        protected UserRepository UserRepository;
        protected SettingsRepository SettingsRepository;
        private ILogger _logger;
        public UserController(UserRepository userRepository, SettingsRepository settingsRepository, ILogger<UserController> logger)
        {
            UserRepository = userRepository;
            SettingsRepository = settingsRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //seed db with the default admin user
            //var user = new User();
            //user.Username = "edge-user";
            //user.Password = "csodedgeisawesome";

            //if (!_userContext.Users.Any(x => x.Username == user.Username && x.Password == user.Password))
            //{
            //    _userContext.Add(user);
            //    _userContext.SaveChanges();
            //}

            return View();
        }

        //[Route("api/users")]
        //[HttpGet]
        //public IActionResult GetAllUsers()
        //{
        //    var users = UserRepository.GetAll();

        //    return Ok(users);
        //}

        //[Route("api/user/{id}")]
        //[HttpGet]
        //public IActionResult GetUser(int id)
        //{
        //    var user = UserRepository.GetUser(id);
        //    if (user != null)
        //    {
        //        return Ok(user);
        //    }
        //    return NotFound();
        //}

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
                user.Password = UserTool.GenerateSaltedHash(user.Password);

                UserRepository.CreateUser(user);
            }
            catch
            {
                return BadRequest();
            }

            return new NoContentResult();
        }

        //[Route("api/user/{id}")]
        //[HttpPut]
        //public IActionResult UpdateUser(int id, [FromBody]User updatedUser)
        //{
        //    if(string.IsNullOrWhiteSpace(updatedUser.Username) 
        //        || string.IsNullOrWhiteSpace(updatedUser.Password)
        //        || id != updatedUser.Id)
        //    {
        //        return BadRequest();
        //    }
        //    var user = UserRepository.GetUser(id);
        //    if(user == null)
        //    {
        //        return BadRequest();
        //    }
        //    //make sure they don't update the username
        //    if(!user.Username.Equals(updatedUser.Username))
        //    {
        //        return BadRequest();
        //    }
        //    //make sure to salt and hash the user password
        //    updatedUser.Password = UserTool.GenerateSaltedHash(updatedUser.Password);
        //    UserRepository.UpdateUser(updatedUser);
        //    return new NoContentResult();
        //}

        //[Route("api/user/{id}")]
        //[HttpDelete]
        //public IActionResult DeleteUser(int id)
        //{
        //    UserRepository.DeleteUser(id);
        //    return new NoContentResult();
        //}

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
                    if (UserTool.DoPasswordsMatch(loginRequest.Password, user.Password))
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
            user.Password = UserTool.GenerateSaltedHash(request.UpdatedPassword);
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
