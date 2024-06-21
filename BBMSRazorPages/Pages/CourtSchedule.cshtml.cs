using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BBMSRazorPages.Pages
{
    public class CourtScheduleModel : PageModel
    {
        private readonly IBookingService bookingService;
        private readonly ICourtService courtService;

        public List<BusinessObjects.Booking> Bookings { get; set; }
        public List<Court> Courts { get; set; }

        [BindProperty]
        public int CourtId { get; set; }

        [BindProperty]
        public TimeSpan StartTime { get; set; }

        [BindProperty]
        public TimeSpan EndTime { get; set; }

        public CourtScheduleModel(IBookingService bookingService, ICourtService courtService)
        {
            this.bookingService = bookingService;
            this.courtService = courtService;
        }

        public void OnGet()
        {
            Bookings = bookingService.GetAllBookings();
            Courts = courtService.GetAllCourts();
        }

        public bool IsTimeSlotBooked(Court court, TimeSpan slot1, TimeSpan slot2)
        {
            return Bookings.Any(b => slot1 >= b.StartTime && slot2 <= b.EndTime && b.Court == court);
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Additional validation logic for 30-minute intervals
                if ((EndTime - StartTime).TotalMinutes < 30 ||
                    StartTime.Minutes % 30 != 0 ||
                    EndTime.Minutes % 30 != 0)
                {
                    ModelState.AddModelError(string.Empty, "Time slots must be in 30-minute intervals.");
                    return Page();
                }

                var newBooking = new BusinessObjects.Booking
                {
                    CourtId = CourtId,
                    StartTime = StartTime,
                    EndTime = EndTime,
                };

                bookingService.AddBooking(newBooking);
                return RedirectToPage();
            }

            // If we got this far, something failed; redisplay form
            Bookings = bookingService.GetAllBookings();
            Courts = courtService.GetAllCourts();
            return Page();
        }
    }
}
