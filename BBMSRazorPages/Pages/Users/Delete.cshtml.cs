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
using Microsoft.IdentityModel.Tokens;
using Services;

namespace BBMSRazorPages.Pages.Users
{
    [SessionRoleAuthorize("Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IUserService userService;
        private readonly IBookingService bookingService;

        public DeleteModel(IUserService userService, IBookingService bookingService)
        {
            this.userService = userService;
            this.bookingService = bookingService;
        }

        [BindProperty]
      public User User { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null || userService == null)
            {
                return NotFound();
            }

            var user = userService.GetUserById((int)id);

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
            try
            {
                string message = "";
                if (id == null || userService == null)
                {
                    return NotFound();
                }
                if (!bookingService.GetBookingsByUserId((int) id).IsNullOrEmpty())
                {
                    message = "Cannot delete because there are bookings of this user.";
                    return RedirectToPage("./Delete", new {id, message });
                }
                else
                {
                    userService.DeleteUser((int)id);
                }



                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
