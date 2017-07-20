using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Models;

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

        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }

        public IActionResult Index()
        {
            //seed db with at least 1 user
            var user = new User();
            user.Username = "csod-edge-user";
            user.Password = "csodedgeisawesome";
            user.Settings = new Settings();
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
            var user = _userContext.Users.FirstOrDefault(x => x.Id == id);
            if(user != null)
            {
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
                if(user.Settings == null)
                {
                    user.Settings = new Settings();
                }

                //add the user
                _userContext.Users.Add(user);
                _userContext.SaveChanges();
            }
            catch(Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Route("api/user/{id}")]
        [HttpPut]
        public IActionResult UpdateUser(int id, [FromBody]User updatedUser)
        {
            var user = _userContext.Users.FirstOrDefault(x => x.Id == id);
            if(user != null)
            {
                user.Username = updatedUser.Username;
                user.Password = updatedUser.Password;
                user.Settings = updatedUser.Settings;

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
            var user = _userContext.Users.FirstOrDefault(x => x.Id == id);
            if(user == null)
            {
                return NotFound();
            }

            _userContext.Users.Remove(user);
            _userContext.SaveChanges();

            return new NoContentResult();
        }
    }
}
