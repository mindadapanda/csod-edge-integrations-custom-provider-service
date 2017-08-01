using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using csod_edge_integrations_custom_provider_service.Models.EdgeBackgroundCheck;
using csod_edge_integrations_custom_provider_service.Middleware;
using csod_edge_integrations_custom_provider_service.Data;
using System.Security.Claims;
using csod_edge_integrations_custom_provider_service.Models;

namespace csod_edge_integrations_custom_provider_service.Controllers
{
    [Authorize(ActiveAuthenticationSchemes = "Basic")]
    [Produces("application/json")]
    public class BackgroundCheckController : Controller
    {
        protected SettingsRepository SettingsRepository;
        protected CallbackRepository CallbackRepository;
        public BackgroundCheckController(SettingsRepository settingsRepository, CallbackRepository callbackRepository)
        {
            SettingsRepository = settingsRepository;
            CallbackRepository = callbackRepository;
        }

        [Route("api/packages")]
        [HttpGet]
        public IActionResult GetPackages()
        {
            //to do: fill out and retrieve and return packages
            var packages = new List<BackgroundCheckPackage>();
            var currentUser = this.User.Identity as ClaimsIdentity;
            var userId = int.Parse(currentUser.Claims.First(x => x.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase)).Value);
            var settings = SettingsRepository.GetSettingsUsingUserId(userId);

            var manager = new FadvManager(settings);

            packages = manager.GetPackages().ToList();

            return Ok(packages);
        }

        [Route("api/initiatebackgroundcheck")]
        [HttpPost]
        public IActionResult InitiateBackgroundCheck([FromBody]BackgroundCheckRequest request)
        {
            //to do: do the background check and populate the background check response
            var packages = new List<BackgroundCheckPackage>();
            var currentUser = this.User.Identity as ClaimsIdentity;
            var userId = int.Parse(currentUser.Claims.First(x => x.Type.Equals("id", StringComparison.CurrentCultureIgnoreCase)).Value);
            var settings = SettingsRepository.GetSettingsUsingUserId(userId);

            var manager = new FadvManager(settings);
            var callbackUrl = this.GenerateCallback(request.CallbackData, request.CallbackData.CallbackUrl, 100);

            var delimiterIndex = request.SelectedPackageId.IndexOf(";");
            var accountId = request.SelectedPackageId.Substring(0, delimiterIndex);
            var packageId = request.SelectedPackageId.Substring(delimiterIndex + 1);

            var response = manager.InitiateBackgroundCheck(request, callbackUrl, accountId, packageId);

            return Ok(response);
        }

        private string GenerateCallback(CallbackData callbackDataFromCsod, string edgeCallbackUrl, int callbackLimit = 10)
        {
            var request = HttpContext.Request;
            if (request == null)
            {
                throw new Exception("request context cannot be null");
            }
            if (string.IsNullOrWhiteSpace(edgeCallbackUrl))
            {
                throw new Exception("edge callback url cannot be empty string");
            }
            if(callbackDataFromCsod == null)
            {
                throw new Exception("callback data is null");
            }
            if (callbackLimit > 100 || callbackLimit <= 0)
            {
                callbackLimit = 100;
            }
            var publicId = Guid.NewGuid();
            var callback = new Callback()
            {
                PublicId = publicId,
                EdgeCallbackUrl = edgeCallbackUrl,
                CallbackDataFromCsod = callbackDataFromCsod,
                Limit = callbackLimit
            };
            CallbackRepository.CreateCallback(callback);

            var callbackUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}/api/callback/{publicId}";
            return callbackUrl;
        }

    }
}