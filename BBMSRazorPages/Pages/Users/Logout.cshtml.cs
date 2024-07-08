using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BBMSRazorPages.Pages.Users
{
    public class LogoutModel : PageModel
    {

        public IActionResult OnPost()
        {
            HttpContext?.Session.Clear();
            return RedirectToPage("/Index");  // Redirect to desired page after logout
        }
    }
}
