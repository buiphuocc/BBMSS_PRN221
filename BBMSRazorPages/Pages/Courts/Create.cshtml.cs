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

namespace BBMSRazorPages.Pages.Courts
{
    [SessionRoleAuthorize("Admin", "Manager")]
    public class CreateModel : PageModel
    {
        private readonly ICourtService _courtService;

        public CreateModel(ICourtService courtService)
        {
            _courtService = courtService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Court Court { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || _courtService == null || Court == null)
            {
                return Page();
            }

            Court.IsActive = true;

            _courtService.AddCourt(Court);

            return RedirectToPage("./Index");
        }
    }
}
