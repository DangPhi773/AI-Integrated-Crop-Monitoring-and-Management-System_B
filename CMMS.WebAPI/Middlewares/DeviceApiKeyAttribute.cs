using CMMS.BLL.Helpers;
using CMMS.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMMS.WebAPI.Middlewares
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class DeviceApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public const string HeaderName = "X-Device-Key";
        public const string ContextKey = "AuthenticatedDevice";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var rawKey) ||
                string.IsNullOrWhiteSpace(rawKey))
            {
                context.Result = new UnauthorizedObjectResult(new { message = $"Thiếu header {HeaderName}." });
                return;
            }

            string hash;
            try
            {
                hash = DeviceApiKeyHelper.HashKey(rawKey.ToString());
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Device key không hợp lệ." });
                return;
            }

            var repo = context.HttpContext.RequestServices.GetRequiredService<IIotDeviceRepository>();
            var device = await repo.GetByApiKeyHashAsync(hash);

            if (device == null)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Device key không hợp lệ." });
                return;
            }

            if (!string.IsNullOrEmpty(device.Status) &&
                !device.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Device đã bị khoá." });
                return;
            }

            context.HttpContext.Items[ContextKey] = device;
            await next();
        }
    }
}
