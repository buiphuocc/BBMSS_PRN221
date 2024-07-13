using BBMSRazorPages.Models;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BBMSRazorPages.Pages
{
    public class ScheduleBookingModel : PageModel
    {
        private readonly IServiceService serviceService;

        private readonly ICourtService courtService;

        private readonly IBookingService bookingService;

        private readonly IVnPayService vnPayService;



        public ScheduleBookingModel(IServiceService serviceService, ICourtService courtService, IBookingService bookingService, IVnPayService vnPayService)
        {
            this.serviceService = serviceService;
            this.courtService = courtService;
            this.bookingService = bookingService;
            this.vnPayService = vnPayService;
        }

        [BindProperty]
        public int Month { get; set; } = 0;

        [BindProperty]
        public int Year { get; set; } = 0;

        public int DaysInCurrentMonth { get; set; }

        public TimeOnly StartTime { get; set; } = new TimeOnly(5, 0);

        public TimeOnly EndTime { get; set; } = new TimeOnly(23, 0);

        public List<string> TimesList { get; set; } = new List<string>();

        [BindProperty]
        public string FromTime { get; set; }

        [BindProperty]
        public string ToTime { get; set; }

        [BindProperty]
        public List<string> DaysOfWeek { get; set; } = new List<string>
        {
            DayOfWeek.Monday.ToString(),
            DayOfWeek.Tuesday.ToString(),
            DayOfWeek.Wednesday.ToString(),
            DayOfWeek.Thursday.ToString(),
            DayOfWeek.Friday.ToString(),
            DayOfWeek.Saturday.ToString(),
            DayOfWeek.Sunday.ToString(),
        };

        [BindProperty]
        public List<string> AvailableDaysOfWeek { get; set; } = new List<string>();

        [BindProperty]
        public List<string> SelectedDaysOfWeek { get; set; } = new List<string>();

        [BindProperty]
        public List<int> SelectedServiceIds { get; set; } = new List<int>();

        [BindProperty]
        public List<int> ServicesAmount { get; set; } = new List<int>();

        [BindProperty]
        public List<Court> AvailableCourts { get; set; } = new List<Court>();

        [BindProperty]
        public int SelectedCourtId { get; set; } = 0;

        public int? UserId { get; set; }
        public void OnGet()
        {
            var currentDate = DateTime.Now;
            var currentTime = TimeOnly.FromDateTime(currentDate);
            Month = currentDate.Month;
            Year = currentDate.Year;
            DaysInCurrentMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            var timesList = new List<string>();
            for (int i = StartTime.Hour; i <= EndTime.Hour; i++)
            {
                var time = new TimeOnly(i, 0);
                timesList.Add(time.ToString("HH:mm"));
                if (i == EndTime.Hour)
                    continue;
                var halfTime = new TimeOnly(i, 30);
                timesList.Add(halfTime.ToString("HH:mm"));
            }
            TimesList = timesList;
            UserId = HttpContext.Session.GetInt32("UserId");
        }

        public IActionResult OnGetUpdateDaysOfWeek(int month, int year, string fromTime, string toTime)
        {
            // Update the DaysOfWeek property
            AvailableDaysOfWeek.Add("Thursday");
            AvailableDaysOfWeek.Add("Tuesday");
            AvailableDaysOfWeek.Add("Saturday");

            int selectedMonth = month;
            int selectedYear = year;

            // Return the updated partial view
            return Partial("_DaysOfWeekPartial", this);
        }

        public IActionResult OnGetAvailableCourts(int month, int year, string fromTime, string toTime)
        {
            // Update the DaysOfWeek property
            AvailableDaysOfWeek.Add("Thursday");
            AvailableDaysOfWeek.Add("Tuesday");
            AvailableDaysOfWeek.Add("Saturday");

            int selectedMonth = month;
            int selectedYear = year;

            // Return the updated partial view
            return Partial("_AvailableCourtsPartial", this);
        }

        public IActionResult OnGetResetDaysOfWeek()
        {
            // Update the DaysOfWeek property
            AvailableDaysOfWeek = new List<string>();

            // Return the updated partial view
            return Partial("_DaysOfWeekPartial", this);
        }

        public IActionResult OnPost()
        {
            // Get month, year and take the number of days in month
            if (Year == 0)
            {
                TempData["PropertyRequired"] = $"Year is required.";
                return RedirectToPage();
            }

            if (Month == 0)
            {
                TempData["PropertyRequired"] = $"Month is required.";
                return RedirectToPage();
            }

            var daysInMonth = DateTime.DaysInMonth(Year, Month);

            // Get from time and to time
            if (string.IsNullOrWhiteSpace(FromTime) || string.IsNullOrEmpty(FromTime))
            {
                TempData["PropertyRequired"] = $"From time is required.";
                return RedirectToPage();
            }
            if (string.IsNullOrWhiteSpace(ToTime) || string.IsNullOrEmpty(ToTime))
            {
                TempData["PropertyRequired"] = $"To time is required.";
                return RedirectToPage();
            }
            var fromTimeParts = FromTime.Split(":");
            var startTime = new TimeSpan(int.Parse(fromTimeParts[0]), int.Parse(fromTimeParts[1]), 0);
            var toTimeParts = ToTime.Split(":");
            var endTime = new TimeSpan(int.Parse(toTimeParts[0]), int.Parse(toTimeParts[1]), 0);

            // Get booking days by selected days of week
            if (SelectedDaysOfWeek.IsNullOrEmpty())
            {
                TempData["PropertyRequired"] = $"Days of week is required, please choose at least 1.";
                return RedirectToPage();
            }
            var bookingDays = new List<DateTime>();
            for (int i = 1; i <= daysInMonth; i++)
            {
                var day = new DateTime(Year, Month, i);
                if (SelectedDaysOfWeek.Contains(day.DayOfWeek.ToString().Trim()))
                {
                    bookingDays.Add(day);
                }
            }

            // Get court by selected court id
            var court = courtService.GetCourtById(SelectedCourtId);
            if (court == null)
            {
                TempData["PropertyRequired"] = $"Court with id {SelectedCourtId} not found.";
                return RedirectToPage();
            }

            // Get price per hour of court
            var pricePerHour = court.PricePerHour;
            var difference = endTime - startTime;
            float totalHours = (float)difference.TotalHours;

            // Create booking for each day
            var services = new List<Service>();
            foreach (var id in SelectedServiceIds)
            {
                var service = serviceService.GetServiceById(id);
                services.Add(service);
            }
            var userId = UserId;
            foreach (var day in bookingDays)
            {
                var booking = new BusinessObjects.Booking
                {
                    UserId = HttpContext.Session.GetInt32("UserId"),
                    CourtId = SelectedCourtId,
                    BookingDate = day,
                    StartTime = startTime,
                    EndTime = endTime,
                    TotalPrice = (decimal)((float)pricePerHour * totalHours),
                    PaymentMethod = "VnPay",
                    Status = "Pending"
                };

                // Get services by selected service id
                var bookingServices = new List<BookingService>();
                for (int i = 0; i < SelectedServiceIds.Count; i++)
                {
                    if (!bookingServices.IsNullOrEmpty())
                    {
                        var bookingServiceIds = bookingServices.Select(bs => bs.ServiceId).ToList();
                        if (bookingServiceIds.Contains(SelectedServiceIds[i]))
                        {
                            var existedBookingService = bookingServices.FirstOrDefault(bs => bs.ServiceId == SelectedServiceIds[i]);
                            existedBookingService.Quantity += ServicesAmount[i];
                            continue;
                        }
                    }
                    var bookingService = new BookingService
                    {
                        ServiceId = SelectedServiceIds[i],
                        Quantity = ServicesAmount[i],
                        Booking = booking
                    };
                    bookingServices.Add(bookingService);
                }
                if (!bookingServices.IsNullOrEmpty())
                {
                    foreach (var bookingService in bookingServices)
                    {
                        var service = services.FirstOrDefault(s => s.ServiceId == bookingService.ServiceId);
                        var price = service.ServicePrice;
                        booking.TotalPrice += (price * bookingService.Quantity);
                    }
                }
                booking.BookingServices = bookingServices;
                bookingService.AddBookingWithServices(booking);
            }
            // Get bookings for payment
            var bookings = new List<BusinessObjects.Booking>();
            foreach (var day in bookingDays)
            {
                var createdBooking = bookingService.GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(day, SelectedCourtId, startTime, endTime, "VnPay");
                if (createdBooking != null)
                {
                    bookings.Add(createdBooking);
                }
            }
            var paymentUrl = vnPayService.CreatePaymentUrlForBooking(bookings, HttpContext);

            return Redirect(paymentUrl);
        }

        public IActionResult OnGetSetTimes(string fromTime, string toTime)
        {
            FromTime = fromTime;
            ToTime = toTime;
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetServiceOptions()
        {
            var services = serviceService.GetAllServices();
            return new JsonResult(services);
        }

        public IActionResult OnGetUpdateAvailableCourts(int month, int year, string fromTime, string toTime, string daysOfWeek)
        {
            var courts = courtService.GetAllCourts();
            AvailableCourts = courts;
            var daysOfWeekValue = daysOfWeek.Split('=')[1];
            var daysOfWeekString = daysOfWeekValue.Split(',');
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var bookingDays = new List<DateTime>();
            var bookingWeekDays = new List<DateTime>();
            for (int i = 1; i <= daysInMonth; i++)
            {
                var day = new DateTime(year, month, i);
                if (daysOfWeekString.Contains(day.DayOfWeek.ToString().Trim()))
                {
                    bookingDays.Add(day);
                }
            }
            for (int j = 1; j <= 7; j++)
            {
                var weekDay = new DateTime(year, month, j);
                if (daysOfWeekString.Contains(weekDay.DayOfWeek.ToString()))
                {
                    bookingWeekDays.Add(weekDay);
                }
            }
            var fromTimeParts = fromTime.Split(":");
            var startTime = new TimeSpan(int.Parse(fromTimeParts[0]), int.Parse(fromTimeParts[1]), 0);
            var toTimeParts = toTime.Split(":");
            var endTime = new TimeSpan(int.Parse(toTimeParts[0]), int.Parse(toTimeParts[1]), 0);
            var occupiedCourtIds = new HashSet<int>();
            foreach (var day in bookingWeekDays)
            {
                var bookings = bookingService.GetBookingsByDateAndStartTimeAndEndTime(day, startTime, endTime);

                foreach (var booking in bookings)
                {
                    occupiedCourtIds.Add(booking.Court.CourtId);
                }
            }
            var availableCourt = new List<Court>();
            foreach (var court in courts)
            {
                if (occupiedCourtIds.Contains(court.CourtId))
                {
                    continue;
                }
                availableCourt.Add(court);
            }
            AvailableCourts = availableCourt;
            return Partial("_AvailableCourtsPartial", this);
        }

        public IActionResult OnGetResetAvailableCourts()
        {
            AvailableCourts = new List<Court>();
            return Partial("_AvailableCourtsPartial", this);
        }
    }
}
