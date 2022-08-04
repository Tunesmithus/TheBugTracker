using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TheBugTracker.Models;

namespace TheBugTracker.Areas.Identity.Pages.Account
{
    public class DemoUserModel : PageModel
    {
        private readonly SignInManager<BTUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ILogger<LoginModel> _logger;

        public DemoUserModel(SignInManager<BTUser> signInManager, IConfiguration config, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _config = config;
            _logger = logger;
        }


        [BindProperty]
        public string DemoUser { get; set; }


        public async Task OnGetAsync( string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync(string user)
        {

            string returnUrl = "~/Home/Dashboard";
            string email = "";
            string password = "";

            if (user == "admin")
            {
                email = _config["DemoLogin:DemoAdminUsername"];
                password = _config["DemoLogin:DemoAdminPassword"];
            }
            else if(user == "pm")
            {
                email = _config["DemoLogin:DemoProjectManagerUsername"];
                password = _config["DemoLogin:DemoProjectManagerPassword"];
            }
            else if (user == "dev")
            {
                email = _config["DemoLogin:DemoDeveloperUsername"];
                password = _config["DemoLogin:DemoDeveloperPassword"];
            }

            else if (user == "submitter")
            {
                email = _config["DemoLogin:DemoSubmitterUsername"];
                password = _config["DemoLogin:DemoSubmitterPassword"];
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = false });
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }

            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }


        }
    }
}
