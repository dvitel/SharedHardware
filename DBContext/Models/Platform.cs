using System;
using System.Collections.Generic;

namespace SharedHardware.Models
{
    public class Platform
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid PlatformTypeId { get; set; }
        public PlatformType PlatformType { get; set; }
        public string DetectedOsVersion { get; set; }                
        public Guid GeolocationId { get; set; }
        public Geolocation Geolocation { get; set; }
        public string PublicIP { get; set; }
        public double ExpectedSLA { get; set; }
        public double CalculatedSLA { get; set; }        
        public long? LastUptimeId { get; set; }
        public PlatformUptimeSpan LastUptime { get; set; }
        public bool IsUp { get; set; }
        public int RunCount { get; set; }
        public int DownCount { get; set; }
        public int DoneWithoutDown { get; set; }
        public string Comment { get; set; }
        public ICollection<PlatformToPlatformTag> PlatformTags { get; set; }
    }
}
