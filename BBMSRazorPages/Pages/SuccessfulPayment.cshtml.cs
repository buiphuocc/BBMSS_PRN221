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
            try
            {
                var id = HttpContext.Session.GetInt32("UserId");
                if (id == null || id <= 0)
                {
                    RedirectToPage("/Authentication/Login");
                }
                var parameters = Request.Query;                  
                if (parameters != null)
                {
                    var response = momoService.PaymentExecuteAsync(parameters);
                }
                return Page();
            }
            catch (Exception ex)
            {
                TempData["PaymentUnsuccessful"] = ex.Message;
                var idsString = ex.Message.Trim().Split(':')[1];
                var idStrings = idsString.Trim().Split(',');
                if(idStrings.Length == 1)
                {
                    var routeValue = new
                    {
                        id = int.Parse(idStrings[0])
                    };
                    return RedirectToPage("/BookingSuccessful", routeValue);
                }
                var routesValue = new
                {
                    ids = idsString
                };
                return RedirectToPage("/ScheduleBookingSuccessful", routesValue);
            }
        }

        public IActionResult OnGetSavePaymentForBooking()
        {
            try
            {
                var parameters = Request.Query;
                if (parameters != null)
                {
                    var response = vnPayService.PaymentExecute(parameters);
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
            catch(Exception ex)
            {
                TempData["PaymentUnsuccessful"] = ex.Message;
                var idsString = ex.Message.Trim().Split(':')[1];
                var idStrings = idsString.Trim().Split(',');
                if (idStrings.Length == 1)
                {
                    var routeValue = new
                    {
                        id = int.Parse(idStrings[0])
                    };
                    return RedirectToPage("/BookingSuccessful", routeValue);
                }
                var routesValue = new
                {
                    ids = idsString
                };
                return RedirectToPage("/ScheduleBookingSuccessful", routesValue);
            }
        }
    }
}
