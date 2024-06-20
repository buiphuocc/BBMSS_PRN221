using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace BBMSRazorPages.Pages.Courts
{
    public class DetailsModel : PageModel
    {
        private readonly BusinessObjects.BadmintonBookingSystemContext _context;

        public DetailsModel(BusinessObjects.BadmintonBookingSystemContext context)
        {
            _context = context;
        }

      public Court Court { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Courts == null)
            {
                return NotFound();
            }

            var court = await _context.Courts.FirstOrDefaultAsync(m => m.CourtId == id);
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
