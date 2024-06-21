using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace BBMSRazorPages.Pages.Booking
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.BadmintonBookingSystemContext _context;

        public IndexModel(BusinessObjects.BadmintonBookingSystemContext context)
        {
            _context = context;
        }

        public IList<BusinessObjects.Booking> Booking { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Bookings != null)
            {
                Booking = await _context.Bookings
                .Include(b => b.Court)
                .Include(b => b.User).ToListAsync();
            }
        }
    }
}
