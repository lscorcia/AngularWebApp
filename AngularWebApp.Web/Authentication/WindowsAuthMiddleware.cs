using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace AngularWebApp.Web.Authentication
{
    public class WindowsAuthMiddleware
    {
        private readonly RequestDelegate next;

        public WindowsAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await context.ChallengeAsync(IISDefaults.AuthenticationScheme);
                return;
            }

            await next(context);
        }
    }
}
