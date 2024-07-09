using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Services.Interfaces;
using BBMSRazorPages.Pages.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace BBMSRazorPages.Pages.Services
{
    [SessionRoleAuthorize("Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IServiceService serviceService;
        private readonly IBookingServiceService bookingServiceService;

        public DeleteModel(IServiceService serviceService, IBookingServiceService bookingServiceService)
        {
            this.serviceService = serviceService;
            this.bookingServiceService = bookingServiceService;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null || serviceService == null)
            {
                return NotFound();
            }

            var service = serviceService.GetServiceById((int)id);

            if (service == null)
            {
                return NotFound();
            }
            else
            {
                Service = service;
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            string message = "";
            if (id == null || serviceService == null)
            {
                return NotFound();
            }
            if (!bookingServiceService.GetBookingServicesByServiceId(id).IsNullOrEmpty())
            {
                message = "Cannot delete because there are Bookings of it";
            }
            else
            {
                serviceService.DeleteService((int)id);
            }

            

            return RedirectToPage("./Index", new {message});
        }
    }
}
