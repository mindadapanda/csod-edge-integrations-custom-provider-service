using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using csod_edge_integrations_custom_provider_service.Models;
using Newtonsoft.Json;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    [Produces("application/json")]
    public class SettingsController : Controller
    {
        //using a temporary data store, ideally this should be a repository or unit of work, etc..
        private IMemoryCache _cache;
        
        public SettingsController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [Route("api/settings/{hashCode}")]
        [HttpGet]
        public IActionResult GetUserSettings(int hashCode)
        {
            Settings settings;
            if(_cache.TryGetValue(hashCode, out settings))
            {
                return Ok(settings);
            }
            return NotFound();
        }

        [Route("api/settings/{hashCode}")]
        [HttpPost]
        public IActionResult CreateUserSettings(int hashCode, [FromBody]Settings settings)
        {

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(3));

            _cache.Set(hashCode, settings, cacheEntryOptions);

            return Ok();
        }
        
        [Route("api/settings/{hashCode}")]
        [HttpDelete]
        public IActionResult DeleteUserSettings(int hashCode)
        {
            _cache.Remove(hashCode);

            return Ok();
        }
    }
}