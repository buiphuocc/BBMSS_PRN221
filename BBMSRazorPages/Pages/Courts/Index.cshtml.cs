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
    public class IndexModel : PageModel
    {
        private readonly ICourtService _courtService;

        public IndexModel(ICourtService courtService)
        {
            _courtService = courtService;
        }

        public IList<Court> Courts { get;set; } = default!;

        public void OnGet()
        {
            if (_courtService != null)
            {
                Courts = _courtService.GetAllCourts();
            }
        }
    }
}
