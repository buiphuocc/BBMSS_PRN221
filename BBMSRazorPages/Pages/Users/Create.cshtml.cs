﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using BBMSRazorPages.Pages.Authentication;
using Services.Interfaces;
using System.Text.RegularExpressions;

namespace BBMSRazorPages.Pages.Users
{
    [SessionRoleAuthorize("Admin")]
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;

        public CreateModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || _userService == null || User == null)
            {
                if (User.Username.Length < 2)
                {
                    TempData["InvalidData"] = "Username must be more than 1 and less than 5 character";
                }
                if (!Regex.Match(User.Phone, "^([0-9]{10})$").Success)
                {
                    TempData["InvalidData"] = "Invalid phone number format";
                }
                if (!User.Role.Equals("Admin") && !User.Role.Equals("Manager") && !User.Role.Equals("Customer"))
                {
                    TempData["InvalidData"] = "Invalid Role";
                }
                return Page();
            }
            _userService.AddUser(User);

            return RedirectToPage("./Index");
        }
    }
}
