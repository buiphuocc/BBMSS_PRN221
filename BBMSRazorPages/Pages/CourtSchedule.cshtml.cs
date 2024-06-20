using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace BBMSRazorPages.Pages
{
    public class CourtScheduleModel : PageModel
    {
        private readonly IBookingService bookingService;

        public List<Booking> Bookings { get; set; }

        public CourtScheduleModel(IBookingService bookingService)
        {
            this.bookingService = bookingService;
        }
        public void OnGet()
        {
            Bookings = bookingService.GetAllBookings();
        }

        public bool IsTimeSlotBooked(Court court, TimeSpan slot1, TimeSpan slot2)
        {
            
            return Bookings.Any(b => slot1 >= b.StartTime && slot2 <= b.EndTime && b.Court==court);
        }
    }
}
