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

        public SuccessfulPaymentModel(IVnPayService vnPayService, IPaymentService paymentService, IBookingService bookingService)
        {
            this.vnPayService = vnPayService;
            this.paymentService = paymentService;
            this.bookingService = bookingService;
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
                
            }
            return RedirectToPage();
        }
    }
}
