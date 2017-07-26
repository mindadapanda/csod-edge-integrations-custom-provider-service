using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Models;
using Microsoft.EntityFrameworkCore;
using LiteDB;
using csod_edge_integrations_custom_provider_service.Data;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    //used by the UI to manage users
    //to secure this controller Filters is strongly suggested
    //like [Authorize]
    [Produces("application/json")]
    public class UserController : Controller
    {
        protected UserRepository UserRepository;

        public UserController(UserRepository userRepository)
        {
            UserRepository = userRepository;
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

        [Route("api/users")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = UserRepository.GetAll();

            return Ok(users);
        }

        [Route("api/user/{id}")]
        [HttpGet]
        public IActionResult GetUser(int id)
        {
            var user = UserRepository.GetUser(id);
            if (user != null)
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

        [Route("api/user/{id}")]
        [HttpPut]
        public IActionResult UpdateUser(int id, [FromBody]User updatedUser)
        {
            if(string.IsNullOrWhiteSpace(updatedUser.Username) 
                || string.IsNullOrWhiteSpace(updatedUser.Password)
                || id != updatedUser.Id)
            {
                return BadRequest();
            }
            var user = UserRepository.GetUser(id);
            if(user == null)
            {
                return BadRequest();
            }
            //make sure they don't update the username
            if(!user.Username.Equals(updatedUser.Username))
            {
                return BadRequest();
            }
            //make sure to salt and hash the user password
            updatedUser.Password = UserTool.GenerateSaltedHash(updatedUser.Password);
            UserRepository.UpdateUser(updatedUser);
            return new NoContentResult();
        }

        [Route("api/user/{id}")]
        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            UserRepository.DeleteUser(id);
            return new NoContentResult();
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
