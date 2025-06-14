using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BloggingApp.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BloggingApp.Web.Pages.Account
{
    public class ForgotPassword : PageModel
    {
        private readonly ILogger<ForgotPassword> _logger;
        private readonly UserManager<AppUser> _userManager;

        public ForgotPassword(ILogger<ForgotPassword> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _logger.LogInformation("Password reset requested for email: {Email}", Input.Email);

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // To prevent user enumeration
                return RedirectToPage("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new { email = user.Email, token },
                Request.Scheme
            );

            // Simulate sending email (in real app, send this via email)
            Console.WriteLine($"Password reset link: {resetUrl}");

            // Simulate sending an email or other actions
            // await _emailService.SendPasswordResetEmailAsync(Input.Email);

            return RedirectToPage("ForgotPasswordConfirmation");
        }
    }
}