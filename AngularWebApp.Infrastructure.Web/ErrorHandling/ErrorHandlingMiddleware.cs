﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace AngularWebApp.Infrastructure.Web.ErrorHandling
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IActionResultExecutor<ObjectResult> executor;
        private readonly ILogger logger;
        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

        public ExceptionHandlerMiddleware(RequestDelegate next, IActionResultExecutor<ObjectResult> executor, ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.executor = executor;
            logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An unhandled exception has occurred while executing the request. Url: {context.Request.GetDisplayUrl()}. Request Data: " + GetRequestData(context));

                if (context.Response.HasStarted)
                {
                    throw;
                }

                var routeData = context.GetRouteData() ?? new RouteData();

                ClearCacheHeaders(context.Response);

                var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

                var result = new ObjectResult(new ErrorResponse(ex.Message, ex.ToString()))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };

                await executor.ExecuteAsync(actionContext, result);
            }
        }

        private static string GetRequestData(HttpContext context)
        {
            var sb = new StringBuilder();

            if (context.Request.HasFormContentType && context.Request.Form.Any())
            {
                sb.Append("Form variables:");
                foreach (var x in context.Request.Form)
                {
                    sb.AppendFormat("Key={0}, Value={1}<br/>", x.Key, x.Value);
                }
            }

            sb.AppendLine("Method: " + context.Request.Method);

            return sb.ToString();
        }

        private static void ClearCacheHeaders(HttpResponse response)
        {
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
        }
    }
}
