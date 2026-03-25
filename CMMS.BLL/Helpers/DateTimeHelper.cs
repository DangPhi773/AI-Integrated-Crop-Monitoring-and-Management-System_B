using System;

namespace CMMS.BLL.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime VnNow()
        {
            var vnTime = DateTime.UtcNow.AddHours(7);
            return DateTime.SpecifyKind(vnTime, DateTimeKind.Utc);
        }
    }
}
