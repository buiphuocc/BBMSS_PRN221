﻿using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BBMSRazorPages.Pages.Authentication
{
    public class SessionRoleAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public SessionRoleAuthorize(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string ContextAccount = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(ContextAccount))
            {
                context.Result = new RedirectToPageResult("/Error");
            }
            else
            {
                string UserRole = JsonConvert.DeserializeObject<string>(ContextAccount);

                if (UserRole == null || UserRole.Equals(_role))
                {
                    context.Result = new RedirectToPageResult("/Error");
                }
            }
        }
    }
}
