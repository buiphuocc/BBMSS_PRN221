using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace BBMSRazorPages.Pages.Users
{
    public class UserDetailsModel : PageModel
    {
		private readonly IUserService userService;

        public UserDetailsModel(IUserService userService)
        {
            this.userService = userService;
        }

        [BindProperty]
        public User UserDetail { get; set; }
        public string Message { get; set; }

        public void OnGet(int id, string message)
        {
            UserDetail = userService.GetUserById(id);
			Message = message;
        }

		public IActionResult OnPostUpdate(int id)
		{
			User updateUser = userService.GetUserById(id);
			updateUser.Username = UserDetail.Username;
			updateUser.Email = UserDetail.Email;
			updateUser.Phone = UserDetail.Phone;
			userService.UpdateUser(updateUser);
			Message = "Update Successfully";
			return RedirectToPage("/Users/UserDetails", new {message = Message});
		}

		public void OnPostDelete(int id)
		{
			UserDetail = userService.GetUserById(id);
		}
	}
}
