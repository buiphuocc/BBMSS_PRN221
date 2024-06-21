using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace BBMSRazorPages.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public DashboardModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IList<BusinessObjects.Booking> Bookings { get; set; } = new List<BusinessObjects.Booking>();

        public IActionResult OnGet()
        {
            Bookings = _bookingService.GetAllBookings();

            return Page();
        }
    }
}
