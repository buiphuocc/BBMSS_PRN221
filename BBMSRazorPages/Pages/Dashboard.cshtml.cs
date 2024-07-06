using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace BBMSRazorPages.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IBookingService bookingService;
        private readonly IBookingServiceService bookingServiceService;
        private readonly ICourtService courtService;

        public IList<BusinessObjects.Booking> Bookings { get; set; } = new List<BusinessObjects.Booking>();
        public IList<BusinessObjects.BookingService> BookingServices { get; set; } = new List<BusinessObjects.BookingService>();
        public IList<BusinessObjects.Court> Courts { get; set; } = new List<BusinessObjects.Court>();


        public DashboardModel(IBookingService bookingService, IBookingServiceService bookingServiceService,ICourtService courtService)
        {
            this.bookingService = bookingService;
            this.bookingServiceService = bookingServiceService;
            this.courtService = courtService;
        }


        public IActionResult OnGet()
        {
            Bookings = bookingService.GetAllBookings();
            Courts = courtService.GetAllCourts();
            BookingServices = bookingServiceService.GetAllBookingServices();
            return Page();
        }

        public List<BookingService> GetBookingServicesByBookingId(int? id)
        {
            return bookingServiceService.GetBookingServicesByBookingId(id);
        }
    }
}
