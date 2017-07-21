using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Models;
using Microsoft.EntityFrameworkCore;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    //used by the UI to manage users and the associated settings with a user
    //this is your basic auth username/password -> setting controller
    //to secure this controller Filters is strongly suggested
    //like [Authorize]
    public class UserController : Controller
    {
        //using a temporary data store, ideally this should be a repository or unit of work, etc..
        private readonly UserContext _userContext;
        //private IMemoryCache _cache;
        //private readonly string _usersKey = "usersKey";

        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }

        public IActionResult Index()
        {
            //seed db with at least 1 user
            var user = new User();
            user.Username = "edge-user";
            user.Password = "csodedgeisawesome";
            user.Settings = new Settings();

            if (!_userContext.Users.Any(x => x.Username == user.Username && x.Password == user.Password))
            {
                _userContext.Add(user);
                _userContext.SaveChanges();
            }

            return View();
        }

        [Route("api/users")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userContext.Users.ToList();

            return Ok(users);
        }

        [Route("api/user/{id}")]
        [HttpGet]
        public IActionResult GetUser(int id)
        {
            var user = _userContext.Users.Include(x => x.Settings).FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                if (user.Settings == null)
                {
                    user.Settings = new Settings();
                }
                return Ok(user);
            }
            return NotFound();
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

                //add the user
                _userContext.Users.Add(user);
                _userContext.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }

            return new NoContentResult();
        }

        [Route("api/user/{id}")]
        [HttpPut]
        public IActionResult UpdateUser(int id, [FromBody]User updatedUser)
        {
            var user = _userContext.Users.Include(x => x.Settings).FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                user.Username = updatedUser.Username;
                user.Password = updatedUser.Password;
                //user.Settings = updatedUser.Settings;

                //doing this because foreign key relationships on EF doesn't work as expected
                //user.Settings = updatedUser.Settings causes EF to think a new settings is created and thus will throw a tracking error
                if(user.Settings != null)
                {
                    //manually updating the values :(
                    var setting = _userContext.Settings.FirstOrDefault(x => x.Id == updatedUser.Settings.Id);
                    setting.VendorUrl = updatedUser.Settings.VendorUrl;
                    setting.VendorUserIdForUser = updatedUser.Settings.VendorUserIdForUser;

                    _userContext.Settings.Update(setting);
                }
                else
                {
                    user.Settings = updatedUser.Settings;
                }
                
                _userContext.Users.Update(user);
                _userContext.SaveChanges();

                return new NoContentResult();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("api/user/{id}")]
        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            var user = _userContext.Users.Include(x=> x.Settings).FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _userContext.Users.Remove(user);
            _userContext.SaveChanges();

            return new NoContentResult();
        }

        [Route("api/user/gettemplate")]
        [HttpGet]
        public IActionResult GetUserTemplate()
        {
            var user = new User();
            user.Settings = new Settings();

            return Ok(user);
        }
    }
}
