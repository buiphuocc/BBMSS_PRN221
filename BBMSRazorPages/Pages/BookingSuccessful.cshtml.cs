using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Services.Interfaces;
using Services;
using Services.Models;

namespace BBMSRazorPages.Pages
{
    public class BookingSuccessfulModel : PageModel
    {
        private readonly IBookingService bookingService;
        private readonly IVnPayService vnPayService;
        private readonly IMomoService momoService;

        public BookingSuccessfulModel(IBookingService bookingService, IVnPayService vnPayService, IMomoService momoService)
        {
            this.bookingService = bookingService;
            this.vnPayService = vnPayService;
            this.momoService = momoService;
        }

        [BindProperty]
        public BusinessObjects.Booking Booking { get; set; } = default!;

        //[BindProperty]
        //public decimal Deposit { get; set; } = 0;

        [BindProperty]
        public int BookingId { get; set; }

        public void OnGet(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId <= 0)
            {
                RedirectToPage("/Authentication/Login");
            }
            var booking = bookingService.GetBookingById(id);
            //Deposit = (decimal)((double)booking.TotalPrice / 2);
            Booking = booking;
        }

        public IActionResult OnPostVnPayPayment()
        {
            try
            {
                // Code for VnPay payment execution
                var newBooking = bookingService.GetBookingById(BookingId);
                var response = vnPayService.CreatePaymentUrlForBooking(new List<BusinessObjects.Booking> { newBooking }, HttpContext);
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
                var newBooking = bookingService.GetBookingById(BookingId);
                var userId = HttpContext.Session.GetInt32("UserId");
                var orderInfo = new OrderInfoModel
                {
                    Amount = (double)newBooking.TotalPrice,
                    OrderInfo = newBooking.BookingId.ToString(),
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
