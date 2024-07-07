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

namespace BBMSRazorPages.Pages.Courts
{
    [SessionRoleAuthorize("Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ICourtService _courtService;

        public DeleteModel(ICourtService courtService)
        {
            _courtService = courtService;
        }

        [BindProperty]
      public Court Court { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null || _courtService == null)
            {
                return NotFound();
            }

            var court = _courtService.GetCourtById((int)id);

            if (court == null)
            {
                return NotFound();
            }
            else
            {
                Court = court;
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null || _courtService == null)
            {
                return NotFound();
            }

            _courtService.DeleteCourt((int)id);

            return RedirectToPage("./Index");
        }
    }
}
