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
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.BadmintonBookingSystemContext _context;

        public IndexModel(BusinessObjects.BadmintonBookingSystemContext context)
        {
            _context = context;
        }

        public IList<Court> Court { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Courts != null)
            {
                Court = await _context.Courts.ToListAsync();
            }
        }
    }
}
