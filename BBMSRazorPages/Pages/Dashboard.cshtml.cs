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
        private readonly ICourtService _courtService;

        public DashboardModel(IBookingService bookingService, IBookingServiceService bookingServiceService,ICourtService courtService)
        {
            _bookingService = bookingService;
            this.bookingServiceService = bookingServiceService;
            _courtService = courtService;
        }

        public IList<BusinessObjects.Booking> Bookings { get; set; } = new List<BusinessObjects.Booking>();
        public IList<BusinessObjects.BookingService> BookingServices { get; set; } = new List<BusinessObjects.BookingService>();
        public IList<BusinessObjects.Court> Courts { get; set; } = new List<BusinessObjects.Court>();

        public IActionResult OnGet()
        {
            Bookings = _bookingService.GetAllBookings();
            Courts = _courtService.GetAllCourts();
            BookingServices = bookingServiceService.GetAllBookingServices();
            return Page();
        }

        public List<BookingService> GetBookingServicesByBookingId(int? id)
        {
            return bookingServiceService.GetBookingServicesByBookingId(id);
        }
    }
}
