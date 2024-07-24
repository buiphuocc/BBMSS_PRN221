using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects;
using Services.Interfaces;
using BBMSRazorPages.Pages.Authentication;

namespace BBMSRazorPages.Pages.Courts
{
    [SessionRoleAuthorize("Admin", "Manager")]
    public class EditModel : PageModel
    {
        private readonly ICourtService _courtService;

        public EditModel(ICourtService courtService)
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
            Court = court;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Court.IsActive = true;
            _courtService.UpdateCourt(Court);

            return RedirectToPage("./Index");
        }
    }
}
