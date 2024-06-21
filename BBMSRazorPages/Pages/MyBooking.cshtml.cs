using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
namespace BBMSRazorPages.Pages
{
    public class MyBookingModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public MyBookingModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IList<BusinessObjects.Booking> Bookings { get; set; } = new List<BusinessObjects.Booking>();

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

            return Page();
        }
    }
}
