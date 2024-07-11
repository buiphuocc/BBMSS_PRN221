using BusinessObjects;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IServiceService serviceService;
        private readonly IBookingServiceService bookingServiceService;

        public List<BusinessObjects.Booking> Bookings { get; set; }
        public List<Court> Courts { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
        public List<Service> Services { get; set; }



        [BindProperty]
        public DateTime SelectedDate { get; set; }

        public string BookingDate { get; set; }

        [BindProperty]
        public string PaymentMethod { get; set; }

        [BindProperty]
        public int CourtId { get; set; }

        [BindProperty]
        public TimeSpan StartTime { get; set; }

        [BindProperty]
        public TimeSpan EndTime { get; set; }

        [BindProperty]
        public DateTime DateForm { get; set; }

        [BindProperty]
        public List<int> SelectedServices { get; set; }


        [BindProperty]
        public Dictionary<int, int> ServiceQuantities { get; set; }

        public CourtScheduleModel(IBookingService bookingService, ICourtService courtService, IServiceService serviceService, IBookingServiceService bookingServiceService)
        {
            this.bookingService = bookingService;
            this.courtService = courtService;
            this.serviceService = serviceService;
            this.bookingServiceService = bookingServiceService;
        }

        public void OnGet(DateTime bookingDate, string message)
        {
            Message = message;
            BookingDate = bookingDate.ToString("yyyy-MM-dd");
            SelectedDate = bookingDate;
            Bookings = bookingService.GetBookingsByBookingDate(SelectedDate);
            Courts = courtService.GetAllCourts();
            UserId = HttpContext.Session.GetInt32("UserId");
            Services = serviceService.GetAllServices();
        }

        public bool IsTimeSlotBooked(Court court, TimeSpan slot1, TimeSpan slot2)
        {
            List<BusinessObjects.Booking> bookings = bookingService.GetBookingsByBookingDate(SelectedDate);
            return Bookings.Any(b => slot1 >= b.StartTime && slot2 <= b.EndTime && b.Court.CourtId == court.CourtId);
        }

        public IActionResult OnPost()
        {
            UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId != null)
            {
                Bookings = bookingService.GetBookingsByBookingDate(DateForm);
                TimeSpan workingStart = new TimeSpan(5, 0, 0);
                TimeSpan workingEnd = new TimeSpan(23, 0, 0);
                UserId = HttpContext.Session.GetInt32("UserId");

                if (ModelState.IsValid)
                {
                    bool isStartTimeInRange = workingStart <= StartTime && StartTime < workingEnd;
                    bool isEndTimeInRange = workingStart < EndTime && EndTime <= workingEnd;
                    if (!isStartTimeInRange || !isEndTimeInRange || (StartTime > EndTime))
                    {
                        ModelState.AddModelError(string.Empty, "Not a valid time range");
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "Not a valid time range" });
                    }
                    // Additional validation logic for 30-minute intervals
                    if ((EndTime - StartTime).TotalMinutes < 30 ||
                        StartTime.Minutes % 30 != 0 ||
                        EndTime.Minutes % 30 != 0)
                    {
                        ModelState.AddModelError(string.Empty, "Time slots must be in 30-minute intervals.");
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "Time slots must be in 30-minute intervals." });
                    }


                    bool inTimeRange = Bookings.Any(b => ((b.StartTime <= StartTime && StartTime <= b.EndTime) || (b.StartTime <= EndTime && EndTime <= b.EndTime)) && b.CourtId == CourtId);
                    Court court = courtService.GetCourtById(CourtId);
                    bool isBooked = Bookings.Any(b => inTimeRange && b.Court.CourtId == CourtId);
                    if (isBooked)
                    {
                        ModelState.AddModelError(string.Empty, "This time range is booked");
                        return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "This time range and court is booked" });
                    }

                    TimeSpan slotDuration = new TimeSpan(0, 30, 0); // 30 minutes
                    int totalSlots = (int)Math.Ceiling((EndTime - StartTime).TotalMinutes / slotDuration.TotalMinutes);
                    var newBooking = new BusinessObjects.Booking
                    {
                        CourtId = CourtId,
                        StartTime = StartTime,
                        EndTime = EndTime,
                        BookingDate = DateForm,
                        PaymentMethod = PaymentMethod,
                        UserId = UserId,
                        TotalPrice = totalSlots * court.PricePerHour
                    };

                    bookingService.AddBooking(newBooking);

                    
                    if (!SelectedServices.IsNullOrEmpty())
                    {
                        decimal totalServicePrice = 0;
                        foreach (var serviceId in SelectedServices)
                        {
                            if (ServiceQuantities.TryGetValue(serviceId, out int quantity))
                            {
                                var bookingService = new BusinessObjects.BookingService
                                {
                                    BookingId = newBooking.BookingId,
                                    ServiceId = serviceId,
                                    Quantity = quantity
                                };
                                totalServicePrice += (quantity * serviceService.GetServiceById(serviceId).ServicePrice);
                                bookingServiceService.AddBookingService(bookingService);
                            }

                        }
                        newBooking.TotalPrice += totalServicePrice;
                        bookingService.UpdateBooking(newBooking);
                    }
                    
                    return RedirectToPage("/CourtSchedule", new { bookingDate = DateForm, message = "Booked Successfully" });
                }
                // If we got this far, something failed; redisplay form
                Bookings = bookingService.GetAllBookings();
                Courts = courtService.GetAllCourts();
                return Page();
            }
            return RedirectToPage("/Authentication/Login");
        }
    }
}
