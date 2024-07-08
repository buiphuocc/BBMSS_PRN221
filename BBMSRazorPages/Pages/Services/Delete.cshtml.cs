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

namespace BBMSRazorPages.Pages.Services
{
    [SessionRoleAuthorize("Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public DeleteModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [BindProperty]
      public Service Service { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null || _serviceService == null)
            {
                return NotFound();
            }

            var service = _serviceService.GetServiceById((int)id);

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
            if (id == null || _serviceService == null)
            {
                return NotFound();
            }

            _serviceService.DeleteService((int)id);

            return RedirectToPage("./Index");
        }
    }
}
