using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BloggingApp.Persistence.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace BloggingApp.Web.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("~/connect/token"), IgnoreAntiforgeryToken]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();

            if (request is null) throw new InvalidOperationException("OIDC request not found");

            if (request.IsPasswordGrantType())
            {
                var user = await _userManager.FindByNameAsync(request.Username);

                if (user == null && !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    _logger.LogWarning("Invalid username or password for user: {Username}", request.Username);
                    return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                //Claims
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);

                identity.AddClaim(Claims.Subject, user.Id.ToString());
                identity.AddClaim(Claims.Name, user.UserName);

                var principal = new ClaimsPrincipal(identity);
                principal.SetScopes(request.GetScopes());
                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                // return BadRequest(new { error = "invalid_grant", error_description = "The specified grant type is not supported." });
            }

            return BadRequest();
        }
    }
}