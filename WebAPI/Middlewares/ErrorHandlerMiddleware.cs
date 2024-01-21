using Serilog;
using System.Data.Common;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middlewares
{
   
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _delegate;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _delegate = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _delegate.Invoke(context);
            }
            catch (DbException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize("Internal Server"));
            }
            catch (Exception ex)
            {

                Log.Error(ex,"UnexpectedError");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize("Internal Server"));
            }
        }
    }
}
