using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Linq;

namespace BBMSRazorPages.Pages
{
    public class MyBookingModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IBookingServiceService bookingServiceService;

        public MyBookingModel(IBookingService bookingService, IBookingServiceService bookingServiceService)
        {
            _bookingService = bookingService;
            this.bookingServiceService = bookingServiceService;
        }

        public IList<BusinessObjects.Booking> Bookings { get; set; } = new List<BusinessObjects.Booking>();
        public IList<BusinessObjects.BookingService> BookingServices { get; set; } = new List<BusinessObjects.BookingService>();

        public int TotalPages { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public IActionResult OnGet(int? pageNumber)
        {
            // Get the user ID from the session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Authentication/Login"); // Redirect to login page if not logged in
            }

            // Get bookings for the user
            var bookings = _bookingService.GetBookingsByUserId(userId.Value);
            // Sort bookings by date in descending order (most recent first)
            bookings = bookings.OrderByDescending(b => b.BookingDate).ThenByDescending(b => b.StartTime).ToList();


            // Pagination
            PageNumber = pageNumber ?? 1;
            TotalPages = (int)Math.Ceiling(bookings.Count / (double)PageSize);

            Bookings = bookings.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
            BookingServices = bookingServiceService.GetAllBookingServices();

            return Page();
        }

        public List<BookingService> GetBookingServicesByBookingId(int? id)
        {
            return bookingServiceService.GetBookingServicesByBookingId(id);
        }
    }
}
