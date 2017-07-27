using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using csod_edge_integrations_custom_provider_service.Models.BackgroundCheck;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "Basic")]
    [Produces("application/json")]
    public class BackgroundCheckController : Controller
    {
        [Route("api/packages")]
        [HttpGet]
        public IActionResult GetPackages()
        {
            //to do: fill out and retrieve and return packages

            var packages = new List<BackgroundCheckPackage>();
            return Ok(packages);
        }

        [Route("api/initiatebackgroundcheck")]
        [HttpPost]
        public IActionResult InitiateBackgroundCheck([FromBody]BackgroundCheckRequest request)
        {
            //to do: do the background check and populate the background check response

            //to do: populate this object with details
            var response = new BackgroundCheckResponse();

            return Ok(response);
        }
    }
}