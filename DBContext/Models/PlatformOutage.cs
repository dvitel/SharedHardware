using System;

namespace SharedHardware.Models
{
    public class PlatformOutage
    {        
        public Guid PlatformId { get; set; }
        public long OutageId { get; set; }
        public Platform Platform { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
