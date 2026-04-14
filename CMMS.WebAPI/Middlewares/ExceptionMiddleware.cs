using CMMS.BLL.Helpers; 
using CMMS.DAL.DTOs.Auth;
using System.Net;
using System.Text.Json;

namespace CMMS.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger; 
            _env = env; 
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new ApiResponse<string>
            {
                Success = false,
                Message = "Đã có lỗi hệ thống xảy ra. Vui lòng thử lại sau!",
                Errors = _env.IsDevelopment()
                         ? new List<string> { ex.Message, ex.StackTrace?.ToString() ?? "" }
                         : new List<string> { "Internal Server Error" }
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}