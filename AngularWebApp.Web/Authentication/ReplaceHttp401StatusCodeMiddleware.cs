using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AngularWebApp.Web.Authentication
{
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
                // Replace all 401 responses, except the ones under the /sso paths
                // which will let IIS trigger the Windows Authentication mechanisms
                if (!context.Request.Path.StartsWithSegments("/sso"))
                {
                    context.Response.StatusCode = 418;
                    context.Response.Headers["X-Original-HTTP-Status-Code"] = "401";
                }
            }
        }
    }
}