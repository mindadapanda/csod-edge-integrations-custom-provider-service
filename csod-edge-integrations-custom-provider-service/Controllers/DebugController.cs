using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using csod_edge_integrations_custom_provider_service.Data;
using Microsoft.AspNetCore.Authorization;
using csod_edge_integrations_custom_provider_service.Models.Fadv;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "Basic")]
    [Produces("application/json")]
    public class DebugController : Controller
    {
        protected BackgroundCheckDebugRepository DebugRepository;

        public DebugController(BackgroundCheckDebugRepository debugRepository)
        {
            DebugRepository = debugRepository;
        }

        [Route("api/debug/getall")]
        [HttpGet]
        public IActionResult GetAllData()
        {
            var data = DebugRepository.GetAll();
            return Ok(data);
        }
    }
}