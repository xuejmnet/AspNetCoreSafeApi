using System;

namespace AspNetCoreSafe.Common
{
    public class UtcTime
    {
        private static readonly long UtcStartTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        private UtcTime()
        {
        }

        public static long CurrentTimeMillis() => (DateTime.UtcNow.Ticks - UtcTime.UtcStartTicks) / 10000L;
    }
}
