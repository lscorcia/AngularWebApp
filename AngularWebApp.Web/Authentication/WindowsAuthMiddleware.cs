using System;
using System.Collections.Generic;
using System.IO;
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

    public class ReplaceHttp401StatusCodeMiddleware
    {
        private readonly RequestDelegate next;

        public ReplaceHttp401StatusCodeMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await next(context);

            if (context.Response.StatusCode == 401)
            {
                if (!context.Request.Path.StartsWithSegments("/sso/"))
                {
                    context.Response.StatusCode = 418;
                    context.Response.Headers["X-Original-HTTP-Status-Code"] = "401";
                }
            }
        }
    }
}
