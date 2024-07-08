using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Services.Interfaces;
using BBMSRazorPages.Pages.Authentication;

namespace BBMSRazorPages.Pages.Users
{
    [SessionRoleAuthorize("Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
      public User User { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null || _userService == null)
            {
                return NotFound();
            }

            var user = _userService.GetUserById((int)id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null || _userService == null)
            {
                return NotFound();
            }

            _userService.DeleteUser((int)id);

            return RedirectToPage("./Index");
        }
    }
}
