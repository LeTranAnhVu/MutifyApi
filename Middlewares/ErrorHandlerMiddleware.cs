using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mutify.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        public async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (ex == null) return;

            var error = new
            {
                Message = ex.InnerException.Message,
               StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.Response.ContentType = "application/json; charset=utf-8";

           await context.Response.WriteAsJsonAsync(error);
        }
    }
}