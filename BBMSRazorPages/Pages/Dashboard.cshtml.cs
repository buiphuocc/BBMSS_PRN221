using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace BBMSRazorPages.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IBookingServiceService bookingServiceService;

        public DashboardModel(IBookingService bookingService, IBookingServiceService bookingServiceService)
        {
            _bookingService = bookingService;
            this.bookingServiceService = bookingServiceService;
        }

        public IList<BusinessObjects.Booking> Bookings { get; set; } = new List<BusinessObjects.Booking>();
        public IList<BusinessObjects.BookingService> BookingServices { get; set; } = new List<BusinessObjects.BookingService>();

        public IActionResult OnGet()
        {
            Bookings = _bookingService.GetAllBookings();
            BookingServices = bookingServiceService.GetAllBookingServices();
            return Page();
        }

        public List<BookingService> GetBookingServicesByBookingId(int? id)
        {
            return bookingServiceService.GetBookingServicesByBookingId(id);
        }
    }
}
