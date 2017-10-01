using System;

namespace SharedHardware.Models
{
    public class PlatformUptimeSpan
    {        
        public Guid PlatformId { get; set; }
        public long SpanId { get; set; }
        public Platform Platform { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
