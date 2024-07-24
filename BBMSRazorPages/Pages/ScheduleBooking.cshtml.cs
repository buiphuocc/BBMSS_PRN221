using BBMSRazorPages.Models;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Services.Models;
using Services;
using System.Linq;
using System.Text.Json;

namespace BBMSRazorPages.Pages
{
    public class ScheduleBookingModel : PageModel
    {
        private readonly IUserService userService;
        
        private readonly IServiceService serviceService;

        private readonly ICourtService courtService;

        private readonly IBookingService bookingService;

        private readonly IVnPayService vnPayService;

        private readonly IMomoService momoService;

        private readonly IEmailSender emailSender;


        public ScheduleBookingModel(IServiceService serviceService, ICourtService courtService, IBookingService bookingService, IVnPayService vnPayService, IMomoService momoService, IUserService userService, IEmailSender emailSender)
        {
            this.serviceService = serviceService;
            this.courtService = courtService;
            this.bookingService = bookingService;
            this.vnPayService = vnPayService;
            this.momoService = momoService;
            this.userService = userService;
            this.emailSender = emailSender;
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

        [BindProperty]
        public int? UserId { get; set; }

        [BindProperty]
        public List<PaymentOption> PaymentOptions { get; set; } = new List<PaymentOption>();

        [BindProperty]
        public int? SelectedPaymentOptionId { get; set; } = 0;

        public class PricingViewModel
        {
            public List<Service>? Services { get; set; }
            public List<Court>? Courts { get; set; }
        }
        public PricingViewModel Pricing { get; set; }

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
            
            var id = HttpContext.Session.GetInt32("UserId");
            UserId = id;
            PaymentOptions = new List<PaymentOption>
            {
                new PaymentOption { Id = 1, Name = "Online payment"},
                new PaymentOption { Id = 2, Name = "Pay at place"}
            };

            //dataa for menu
            var services = serviceService.GetAllServices();
            var courts = courtService.GetAllCourts();
            Pricing = new PricingViewModel
            {
                Services = services,
                Courts = courts
            };
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

        public async Task<IActionResult> OnPost()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId <= 0)
            {
                return RedirectToPage("/Authentication/Login");
            }
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

            // Handle booking
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

            // Check selected payment option
            if (SelectedPaymentOptionId == 0)
            {
                TempData["PropertyRequired"] = $"Payment option is required, please choose your suitable payment option.";
                return RedirectToPage();
            }
            string paymentOption = SelectedPaymentOptionId == 1 ? "Online payment" : "Pay at place";

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

            // Variables for email sending
            string dates = "<ul>";
            decimal emailTotalPrice = 0;
            Dictionary<string, int> EmailServiceQuantities = new();
            //

            var bookingServices = new List<BusinessObjects.BookingService>();
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
                var bookingService = new BusinessObjects.BookingService
                {
                    ServiceId = SelectedServiceIds[i],
                    Service = services.FirstOrDefault(s => s.ServiceId == SelectedServiceIds[i]),
                    Quantity = ServicesAmount[i]
                };
                bookingServices.Add(bookingService);
            }

            foreach (var bookingService in bookingServices)
            {
                var service = services.FirstOrDefault(s => s.ServiceId == bookingService.ServiceId);
                EmailServiceQuantities.Add(service.ServiceName, bookingService.Quantity);
            }

            /////
            var newBookings = new List<BusinessObjects.Booking>();
            foreach (var day in bookingDays)
            {
                dates += $"<li>{DateOnly.FromDateTime(day)}</li>";

                var booking = new BusinessObjects.Booking
                {
                    UserId = HttpContext.Session.GetInt32("UserId"),
                    CourtId = SelectedCourtId,
                    BookingDate = day,
                    StartTime = startTime,
                    EndTime = endTime,
                    TotalPrice = (decimal)((float)pricePerHour * totalHours),
                    PaymentMethod = paymentOption,
                    Status = "Pending"
                };
                var newBookingServices = new List<BusinessObjects.BookingService>();
                // Get services by selected service id
                if (!bookingServices.IsNullOrEmpty())
                {
                    foreach (var bookingService in bookingServices)
                    {
                        var newBookingService = new BusinessObjects.BookingService
                        {
                            ServiceId = bookingService.ServiceId,
                            Quantity = bookingService.Quantity,
                            Booking = booking
                        };
                        newBookingServices.Add(newBookingService);
                        var service = services.FirstOrDefault(s => s.ServiceId == bookingService.ServiceId);
                        var price = service.ServicePrice;
                        booking.TotalPrice += (price * bookingService.Quantity);
                    }
                }

                emailTotalPrice += booking.TotalPrice;
                booking.BookingServices = newBookingServices;
                if(paymentOption.Equals("Pay at place"))
                {
                    bookingService.AddBookingWithServices(booking);
                }
                newBookings.Add(booking);
            }

            // Send email to user

            //dates += "</ul>";
            //User user = userService.GetUserById((int) HttpContext.Session.GetInt32("UserId"));

            //if (user != null)
            //{
            //    var bookedCourt = court;

            //    string subject = "Badminton Court Booking Confirmation";
            //    string message =
            //        $"Dear {user.Email}, your badminton court booking has been confirmed!<br><br>" +
            //        $"<strong>Booking Details:</strong><br>" +
            //        $"Court name: {bookedCourt.CourtName}<br>" +
            //        $"On dates: {dates}, from {startTime} to {endTime}<br>";

            //    if (services != null && EmailServiceQuantities.Count > 0)
            //    {
            //        message += "<br><strong>Additional Services:</strong><br>";
            //        foreach (var service in EmailServiceQuantities)
            //        {
            //            message += $"- {service.Key}, quantity: {service.Value}.<br>";
            //        }
            //    }
            //    else
            //    {
            //        message += "<br><strong>Additional Services:</strong> None.<br>";
            //    }

            //    message +=
            //        $"<br>Total booking price: {emailTotalPrice}<br>" +
            //        $"Payment method: {paymentOption}<br><br>" +
            //        "In case the information is not correct, please contact us by replying to this email to make adjustments as soon as possible.";

            //    await emailSender.SendEmailAsync(user.Email, subject, message);

            //    Console.WriteLine("Sent email to " + user.Email);
            //}

            if (paymentOption.Equals("Online payment"))
            {
                // Get bookings for payment
                //var bookings = new List<BusinessObjects.Booking>();
                //var bookingIdsString = "";
                //for(int i = 0; i < bookingDays.Count; i++)
                //{
                //    var createdBooking = bookingService.GetBookingsByBookingDateAndCourtIdAndStartTimeAndEndTimeAndPaymentMethod(bookingDays[i], SelectedCourtId, startTime, endTime, "Online payment");
                //    if (createdBooking != null)
                //    {
                //        bookings.Add(createdBooking);
                //        if(i == bookingDays.Count - 1)
                //        {
                //            bookingIdsString += createdBooking.BookingId;
                //            continue;
                //        }
                //        bookingIdsString += createdBooking.BookingId + ",";
                //    }
                //}
                //var routesValue = new
                //{
                //    ids = bookingIdsString
                //};
                var daysOfWeekString = "";
                for(int i = 0; i < SelectedDaysOfWeek.Count; i++)
                {
                    if(i == SelectedDaysOfWeek.Count - 1)
                    {
                        daysOfWeekString += SelectedDaysOfWeek[i];
                        continue;
                    }
                    daysOfWeekString += SelectedDaysOfWeek[i] + ", ";
                }
                var selectedCourt = courtService.GetCourtById(SelectedCourtId);
                var userBook = userService.GetUserById((int)HttpContext.Session.GetInt32("UserId"));
                var bookingDatesString = "";
                for (int i = 0; i < bookingDays.Count; i++)
                {
                    if (i == bookingDays.Count - 1)
                    {
                        bookingDatesString += bookingDays[i].ToString("MM/dd/yyyy");
                        continue;
                    }
                    bookingDatesString += bookingDays[i].ToString("MM/dd/yyyy") + ", ";
                }
                var scheduleBookingModel = new ScheduleBookingsModel
                {
                    DaysOfWeek = daysOfWeekString,
                    Court = selectedCourt,
                    User = userBook,
                    BookingDates = bookingDatesString,
                    StartTime = startTime,
                    EndTime = endTime,
                    TotalPrice = emailTotalPrice,
                    BookingServices = bookingServices
                };
                var scheduleBookingModelJsonString = JsonSerializer.Serialize(scheduleBookingModel);

                return RedirectToPage("/ScheduleBookingSuccessful", new { scheduleBookingModelJsonString});

                // VnPay
                //var paymentUrl = vnPayService.CreatePaymentUrlForBooking(bookings, HttpContext);

                //return Redirect(paymentUrl);

                // Momo
                //var bookingIdsString = "";
                //decimal amount = 0;
                //for (int i = 0; i < bookings.Count; i++)
                //{
                //    amount += bookings[i].TotalPrice;
                //    if (i == bookings.Count - 1)
                //    {
                //        bookingIdsString += bookings[i].BookingId;
                //        continue;
                //    }
                //    bookingIdsString += bookings[i].BookingId + ",";
                //}
                //var orderInfo = new OrderInfoModel
                //{
                //    OrderInfo = bookingIdsString,
                //    Amount = (double)amount,
                //    UserId = (int)userId
                //};
                //var response = await momoService.CreatePaymentAsync(orderInfo, null);
                //return Redirect(response.PayUrl);
            }
            TempData["BookingSuccess"] = "Schedule booking successfully.";
            return RedirectToPage();
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
