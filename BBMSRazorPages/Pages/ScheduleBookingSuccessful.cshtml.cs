using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using BBMSRazorPages.Models;
using Services.Models;
using NuGet.Packaging.Signing;
using System;

namespace BBMSRazorPages.Pages
{
    public class ScheduleBookingSuccessfulModel : PageModel
    {
        private readonly IBookingService bookingService;
        private readonly IVnPayService vnPayService;
        private readonly IMomoService momoService;

        [BindProperty]
        public decimal Deposit { get; set; } = 0;

        [BindProperty]
        public BBMSRazorPages.Models.ScheduleBookingModel ScheduleBookingModel { get; set; }

        [BindProperty]
        public string BookingIds { get; set; }

        [BindProperty]
        public decimal TotalPrice { get; set; }

        public ScheduleBookingSuccessfulModel(IBookingService bookingService, IVnPayService vnPayService, IMomoService momoService)
        {
            this.bookingService = bookingService;
            this.vnPayService = vnPayService;
            this.momoService = momoService;
        }

        public void OnGet(string ids)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId <= 0)
            {
                RedirectToPage("/Authentication/Login");
            }
            var idStrings = ids.Trim().Split(',');
            BookingIds = ids.Trim();
            var bookings = new List<BusinessObjects.Booking>();
            var daysOfWeek = new HashSet<DayOfWeek>();
            decimal totalPrice = 0;
            foreach(var idString in idStrings)
            {
                if(int.TryParse(idString, out var id))
                {
                    var booking = bookingService.GetBookingById(id);
                    bookings.Add(booking);
                    daysOfWeek.Add(booking.BookingDate.DayOfWeek);
                    totalPrice += booking.TotalPrice;
                }
                else
                {

                }
            }
            var bookingDates = bookings.Select(b => DateOnly.FromDateTime(b.BookingDate)).ToList();
            var bookingDatesString = "";
            foreach(var bookingDate in bookingDates)
            {
                bookingDatesString += bookingDate.ToString() + ", ";
            }
            var daysOfWeekString = "";
            foreach(var dayOfWeek in daysOfWeek)
            {
                daysOfWeekString += dayOfWeek.ToString() + ", ";
            }
            var scheduleBooking = new BBMSRazorPages.Models.ScheduleBookingModel
            {
                BookingDates = bookingDatesString,
                DaysOfWeek = daysOfWeekString,
                Court = bookings[0].Court,
                User = bookings[0].User,
                StartTime = bookings[0].StartTime,
                EndTime = bookings[0].EndTime,
                TotalPrice = totalPrice
            };
            ScheduleBookingModel = scheduleBooking;
        }

        public IActionResult OnPostVnPayPayment()
        {
            try
            {
                // Code for VnPay payment execution
                var idStrings = BookingIds.Trim().Split(',');
                var bookings = new List<BusinessObjects.Booking>();
                foreach (var idString in idStrings)
                {
                    if (int.TryParse(idString.Trim(), out var id))
                    {
                        var booking = bookingService.GetBookingById(id);
                        bookings.Add(booking);
                    }
                    else
                    {

                    }
                }
                var response = vnPayService.CreatePaymentUrlForBooking(bookings, HttpContext);
                return Redirect(response);
            }
            catch (Exception ex)
            {
                TempData["PaymentUnsuccessful"] = ex.Message;
                string currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                return LocalRedirect(currentUrl);
            }
        }

        public async Task<IActionResult> OnPostMomoPayment()
        {
            try
            {
                // Code for Momo Wallet payment execution
                var userId = HttpContext.Session.GetInt32("UserId");
                var orderInfo = new OrderInfoModel
                {
                    Amount = (double)TotalPrice,
                    OrderInfo = BookingIds,
                    UserId = (int)userId
                };

                var response = await momoService.CreatePaymentAsync(orderInfo, null);
                return Redirect(response.PayUrl);

            }
            catch (Exception ex)
            {
                TempData["PaymentUnsuccess"] = ex.Message;
                string currentUrl = HttpContext.Request.Path + HttpContext.Request.QueryString;
                return LocalRedirect(currentUrl);
            }
        }
    }
}
