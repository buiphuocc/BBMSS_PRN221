using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBMSRazorPages.Pages.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace BBMSRazorPages.Pages.Booking
{
    [SessionRoleAuthorize("Admin")]
    public class IndexModel : PageModel
    {
        private readonly IBookingService _service;

        public IndexModel(IBookingService service)
        {
            _service = service;
        }

        public IList<BusinessObjects.Booking> Bookings { get;set; } = default!;

        public void OnGet()
        {
            if (_service != null)
            {
                Bookings = _service.GetAllBookings();
            }
        }
    }
}
