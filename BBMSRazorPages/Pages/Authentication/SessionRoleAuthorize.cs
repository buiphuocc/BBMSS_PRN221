using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BBMSRazorPages.Pages.Authentication
{
    public class SessionRoleAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly List<string> _roles;

        public SessionRoleAuthorize(params string[] roles)
        {
            _roles = roles.ToList();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string UserRole = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(UserRole))
            {
                context.Result = new RedirectToPageResult("/Error");
            }
            else
            {
                if (UserRole == null || !_roles.Contains(UserRole))
                {
                    context.Result = new RedirectToPageResult("/Error");
                }
            }
        }

        public static string GetRole(AuthorizationFilterContext context)
        {
            return context.HttpContext.Session.GetString("UserRole");
        }
    }
}
