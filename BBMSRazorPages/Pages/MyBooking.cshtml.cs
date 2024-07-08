using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
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


        public IActionResult OnGet()
        {
            // Get the user ID from the session
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Authentication/Login"); // Redirect to login page if not logged in
            }

            // Get bookings for the user
            Bookings = _bookingService.GetBookingsByUserId(userId.Value);
            BookingServices = bookingServiceService.GetAllBookingServices();

            return Page();
        }
        public List<BookingService> GetBookingServicesByBookingId(int? id)
        {
            return bookingServiceService.GetBookingServicesByBookingId(id);
        }

    }
}
