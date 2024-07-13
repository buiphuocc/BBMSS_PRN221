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

        public SuccessfulPaymentModel(IVnPayService vnPayService, IPaymentService paymentService, IBookingService bookingService, IUserService userService)
        {
            this.vnPayService = vnPayService;
            this.paymentService = paymentService;
            this.bookingService = bookingService;
            this.userService = userService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnGetSavePaymentForBooking()
        {
            var parameters = Request.Query;
            if (parameters != null)
            {
                var response = vnPayService.BookingPaymentExecute(parameters);
                parameters.TryGetValue("userId", out var userIdString);
                if(!string.IsNullOrEmpty(userIdString))
                {
                    var userId = int.Parse(userIdString);
                    var user = userService.GetUserById(userId);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserRole", user.Role);
                }
            }
            return RedirectToPage();
        }
    }
}
