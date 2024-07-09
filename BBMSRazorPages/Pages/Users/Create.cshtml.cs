using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using BBMSRazorPages.Pages.Authentication;
using Services.Interfaces;

namespace BBMSRazorPages.Pages.Users
{
    //[SessionRoleAuthorize("Admin")]
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;

        public CreateModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || _userService == null || User == null)
            {
                return Page();
            }
            _userService.AddUser(User);

            return RedirectToPage("/Authentication/Login");
        }
    }
}
