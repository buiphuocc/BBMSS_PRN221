using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Services.Interfaces;
using BBMSRazorPages.Pages.Authentication;

namespace BBMSRazorPages.Pages.Booking
{
    [SessionRoleAuthorize("Admin")]
    public class EditModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly ICourtService _courtService;
        private readonly IUserService _userService;

        public EditModel(IBookingService bookingService, ICourtService courtService, IUserService userService)
        {
            _bookingService = bookingService;
            _courtService = courtService;
            _userService = userService;
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
            Booking = booking;
            ViewData["CourtId"] = new SelectList(_courtService.GetAllCourts(), "CourtId", "CourtName");
            ViewData["UserId"] = new SelectList(_userService.GetAllUsers(), "UserId", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _bookingService.UpdateBooking(Booking);

            return RedirectToPage("./Index");
        }
    }
}
