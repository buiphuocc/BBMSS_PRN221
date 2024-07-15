using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Interfaces;
using System.Text.RegularExpressions;

namespace BBMSRazorPages.Pages.Authentication
{
    public class RegistrationModel : PageModel
    {
        private readonly IUserService _userService;
        [BindProperty]
        public User User { get; set; } = default!;
        public RegistrationModel(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || _userService == null || User == null)
            {
                if(User.Username.Length < 2)
                {
                    TempData["InvalidData"] = "Username must be more than 1 and less than 5 character";
                }
                if (!Regex.Match(User.Phone, "^([0-9]{10})$").Success)
                {
                    TempData["InvalidData"] = "Invalid phone number format";
                }
                return Page();
            }
            _userService.AddUser(User);

            return RedirectToPage("/Authentication/Login");
        }
    }
}
