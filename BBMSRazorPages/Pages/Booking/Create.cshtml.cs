using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using Services.Interfaces;

namespace BBMSRazorPages.Pages.Booking
{
    public class CreateModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly ICourtService _courtService;
        private readonly IUserService _userService;

        public CreateModel(IBookingService bookingService, ICourtService courtService, IUserService userService)
        {
            _bookingService = bookingService;
            _courtService = courtService;
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            ViewData["CourtId"] = new SelectList(_courtService.GetAllCourts(), "CourtId", "CourtName");
            ViewData["UserId"] = new SelectList(_userService.GetAllUsers(), "UserId", "Email");
            return Page();
        }

        [BindProperty]
        public BusinessObjects.Booking Booking { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || _bookingService == null || Booking == null)
            {
                return Page();
            }

            _bookingService.AddBooking(Booking);

            return RedirectToPage("./Index");
        }
    }
}
