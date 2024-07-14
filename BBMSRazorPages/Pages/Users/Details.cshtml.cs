using System;
using System.Collections.Generic;
using System.Linq;
using BBMSRazorPages.Pages.Authentication;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Interfaces;

namespace BBMSRazorPages.Pages.Users
{
    [SessionRoleAuthorize("Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        private readonly IBookingServiceService _bookingServiceService;

        public DetailsModel(IUserService userService, IBookingService bookingService, IBookingServiceService bookingServiceService)
        {
            _userService = userService;
            _bookingService = bookingService;
            _bookingServiceService = bookingServiceService;
        }

        public User User { get; set; }
        public IList<BusinessObjects.Booking> Bookings { get; set; } = new List<BusinessObjects.Booking>();

        // Pagination properties
        public int PageSize { get; set; } = 10; // Number of items per page
        public int PageIndex { get; set; } = 1; // Current page index
        public int TotalPages { get; set; } // Total number of pages

        public IActionResult OnGet(int? id, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = _userService.GetUserById((int)id);

            if (User == null)
            {
                return NotFound();
            }

            // Get total count of bookings for the user
            var userBookings = _bookingService.GetBookingsByUserId(User.UserId);

            // Sort bookings by date in descending order (most recent first)
            userBookings = userBookings.OrderByDescending(b => b.BookingDate).ToList();

            // Calculate total pages
            TotalPages = (int)Math.Ceiling(userBookings.Count / (double)PageSize);

            // Set current page index
            PageIndex = pageIndex ?? 1;
            // Fetch bookings for the current page
            Bookings = userBookings.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            return Page();
        }
        public List<BusinessObjects.BookingService> GetBookingServicesByBookingId(int? id)
        {
            return _bookingServiceService.GetBookingServicesByBookingId(id);
        }
    }
}
