using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using csod_edge_integrations_custom_provider_service.Models;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    //used by the UI to manage users
    //supports create, delete, and read of user/users
    public class UserController : Controller
    {
        //using a temporary data store, ideally this should be a repository or unit of work, etc..
        private IMemoryCache _cache;
        private readonly string _usersKey = "users";

        public UserController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [Route("api/users")]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            HashSet<User> users = this.GetAllUsersFromCache();

            return Ok(users);
        }

        [Route("api/user/{hashCode}")]
        [HttpGet]
        public IActionResult GetUser(int hashCode)
        {
            var users = this.GetAllUsersFromCache();
            foreach (var user in users)
            {
                if (user.HashCode == hashCode)
                {
                    return Ok(user);
                }
            }
            return NotFound();
        }

        [Route("api/user/{hashCode}")]
        [HttpPost]
        public IActionResult CreateUser(int hashCode, [FromBody]User user)
        {
            if(string.IsNullOrWhiteSpace(user.Username)
                || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest();
            }

            var users = this.GetAllUsersFromCache();
            if (!users.Contains(user))
            {
                users.Add(user);
            }
            this.UpdateUsers(users);
            return Ok();
        }

        [Route("api/user/{hashCode}")]
        [HttpDelete]
        public IActionResult DeleteUser(int hashCode)
        {
            var users = this.GetAllUsersFromCache();
            foreach(var user in users)
            {
                if(user.HashCode == hashCode)
                {
                    users.Remove(user);
                    //also remove the associated settings as well
                    this.UpdateUsers(users);

                    return Ok();
                }
            }
            return NotFound();
        }

        private HashSet<User> GetAllUsersFromCache()
        {
            var users = new HashSet<User>();
            _cache.TryGetValue(_usersKey, out users);

            return users;
        }

        private void UpdateUsers(HashSet<User> user)
        {
            //this is a potential bug where update users can be called and used by N number of users and some might run into collisions when setting the cache
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(3));

            _cache.Set(_usersKey, user, cacheEntryOptions);
        }
    }
}
