using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;

namespace BBMSRazorPages.Pages.Booking
{
    public class CreateModel : PageModel
    {
        private readonly BusinessObjects.BadmintonBookingSystemContext _context;

        public CreateModel(BusinessObjects.BadmintonBookingSystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CourtId"] = new SelectList(_context.Courts, "CourtId", "CourtName");
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return Page();
        }

        [BindProperty]
        public BusinessObjects.Booking Booking { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Bookings == null || Booking == null)
            {
                return Page();
            }

            _context.Bookings.Add(Booking);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
