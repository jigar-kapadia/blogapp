using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BloggingApp.Web.Pages.Account
{
    public class ForgotPasswordConfirmation : PageModel
    {
        private readonly ILogger<ForgotPasswordConfirmation> _logger;

        public ForgotPasswordConfirmation(ILogger<ForgotPasswordConfirmation> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}