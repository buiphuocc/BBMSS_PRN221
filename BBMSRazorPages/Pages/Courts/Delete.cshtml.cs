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

namespace BBMSRazorPages.Pages.Courts
{
    [SessionRoleAuthorize("Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ICourtService _courtService;
        private readonly IBookingService bookingService;

        public DeleteModel(ICourtService courtService, IBookingService bookingService)
        {
            _courtService = courtService;
            this.bookingService = bookingService;
        }


        [BindProperty]
      public Court Court { get; set; } = default!;

        public IActionResult OnGet(int? id, string message)
        {
            if (id == null || _courtService == null)
            {
                return NotFound();
            }

            var court = _courtService.GetCourtById((int)id);

            if (court == null)
            {
                return NotFound();
            }
            else
            {
                Court = court;
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            try
            {
                string message= "";
                if (id == null || _courtService == null)
                {
                    return NotFound();
                }
                if (!bookingService.GetBookingsByCourtId(id).IsNullOrEmpty())
                {
                    message = "Cannot delete because there are bookings of the court";
                }
                else
                {
                    _courtService.DeleteCourt((int)id);
                }

                

                return RedirectToPage("./Index", new {message});
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
