using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace BBMSRazorPages.Pages
{
    public class SuccessfulPaymentModel : PageModel
    {

        private readonly IVnPayService vnPayService;

        private readonly IPaymentService paymentService;

        private readonly IBookingService bookingService;

        private readonly IUserService userService;

        private readonly IMomoService momoService;

        public SuccessfulPaymentModel(IVnPayService vnPayService, IPaymentService paymentService, IBookingService bookingService, IUserService userService, IMomoService momoService)
        {
            this.vnPayService = vnPayService;
            this.paymentService = paymentService;
            this.bookingService = bookingService;
            this.userService = userService;
            this.momoService = momoService;
        }

        public IActionResult OnGet()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            if (id == null || id <= 0)
            {
                RedirectToPage("/Authentication/Login");
            }
            var parameters = Request.Query;
            if (parameters != null)
            {
                parameters.TryGetValue("bookingDatesString", out var bookingDatesString);
                if(string.IsNullOrEmpty(bookingDatesString) || string.IsNullOrWhiteSpace(bookingDatesString))
                {
                    return RedirectToPage("/ScheduleBooking");
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnGetSavePaymentForBooking()
        {
            try
            {
                var parameters = Request.Query;
                if (parameters != null)
                {
                    var response = await vnPayService.PaymentExecute(parameters);
                    parameters.TryGetValue("userId", out var userIdString);
                    if (!string.IsNullOrEmpty(userIdString))
                    {
                        var userId = int.Parse(userIdString);
                        var user = userService.GetUserById(userId);
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        HttpContext.Session.SetString("UserRole", user.Role);
                    }

                }
                return Page();
            }
            catch (Exception ex)
            {
                TempData["PaymentUnsucessful"] = "Payment for your booking is unsucess.";
                return RedirectToPage("/ScheduleBooking");
            }
        }
    }
}
