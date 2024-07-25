using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using BBMSRazorPages.Models;
using Services.Models;
using NuGet.Packaging.Signing;
using System;
using System.Text.Json;

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
        public ScheduleBookingsModel ScheduleBookingModel { get; set; }

        [BindProperty]
        public string BookingIds { get; set; }

        [BindProperty]
        public decimal TotalPrice { get; set; }

        [BindProperty]
        public string ScheduleBookingModelJsonString { get; set; }

        [BindProperty]
        public int NumberOfDates { get; set; }

        public ScheduleBookingSuccessfulModel(IBookingService bookingService, IVnPayService vnPayService, IMomoService momoService)
        {
            this.bookingService = bookingService;
            this.vnPayService = vnPayService;
            this.momoService = momoService;
        }

        public void OnGet(string scheduleBookingModelJsonString)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId <= 0)
            {
                RedirectToPage("/Authentication/Login");
            }
            if (string.IsNullOrEmpty(scheduleBookingModelJsonString))
            {
                RedirectToPage("/ScheduleBooking");
            }

            ScheduleBookingsModel scheduleBookingModel = JsonSerializer.Deserialize<ScheduleBookingsModel>(scheduleBookingModelJsonString);
            ScheduleBookingModel = scheduleBookingModel;
            ScheduleBookingModelJsonString = scheduleBookingModelJsonString;
            var bookingDates = scheduleBookingModel.BookingDates.Trim().Split(", ");
            NumberOfDates = bookingDates.Length;
        }

        public IActionResult OnPostVnPayPayment()
        {
            try
            {
                // Code for VnPay payment execution
                ScheduleBookingsModel scheduleBookingModel = JsonSerializer.Deserialize<ScheduleBookingsModel>(ScheduleBookingModelJsonString);
                var response = vnPayService.CreatePaymentUrl(scheduleBookingModel.User.UserId, scheduleBookingModel, HttpContext);
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
