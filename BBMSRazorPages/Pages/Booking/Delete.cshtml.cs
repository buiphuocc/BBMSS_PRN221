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

namespace BBMSRazorPages.Pages.Booking
{
    [SessionRoleAuthorize("Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public DeleteModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [BindProperty]
      public BusinessObjects.Booking Booking { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null || _bookingService == null)
            {
                return NotFound();
            }

            var booking = _bookingService.GetBookingById((int)id);

            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                Booking = booking;
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null || _bookingService == null)
            {
                return NotFound();
            }

            _bookingService.DeleteBooking((int)id);

            return RedirectToPage("./Index");
        }
    }
}
