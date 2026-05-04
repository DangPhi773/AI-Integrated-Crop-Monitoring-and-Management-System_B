using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace CMMS.WebAPI.Hubs;

public static class HubExtensions
{
    public static Guid GetUserId(this HubCallerContext context)
    {
        var id = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
    }

    public static string? GetRole(this HubCallerContext context)
        => context.User?.FindFirst(ClaimTypes.Role)?.Value;
}
