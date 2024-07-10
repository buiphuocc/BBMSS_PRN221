using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects;
using Services.Interfaces;
using BBMSRazorPages.Pages.Authentication;

namespace BBMSRazorPages.Pages.Courts
{
    [SessionRoleAuthorize("Admin", "Manager")]
    public class DetailsModel : PageModel
    {
        private readonly ICourtService _courtService;

        public DetailsModel(ICourtService courtService)
        {
            _courtService = courtService;
        }

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
    }
}
