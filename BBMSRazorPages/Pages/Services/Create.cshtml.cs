using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using Services.Interfaces;
using BBMSRazorPages.Pages.Authentication;

namespace BBMSRazorPages.Pages.Services
{
    [SessionRoleAuthorize("Admin")]
    public class CreateModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public CreateModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Service Service { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || _serviceService == null || Service == null)
            {
                return Page();
            }

            _serviceService.AddService(Service);

            return RedirectToPage("./Index");
        }
    }
}
