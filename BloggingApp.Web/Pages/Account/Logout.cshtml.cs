using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BloggingApp.Web.Pages.Account
{
    public class Logout : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;

        public Logout(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task OnPostAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}