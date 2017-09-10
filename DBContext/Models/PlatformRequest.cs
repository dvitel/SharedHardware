using System;

namespace SharedHardware.Models
{
    public class PlatformRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid TypeId { get; set; }
        public PlatformType Type { get; set; } //min platform
        public Guid GeolocationId { get; set; }
        public Geolocation Geolocation { get; set; }
        public double ExpectedSLA { get; set; }
        public Guid ComputationId { get; set;  }
        public Computation Computation { get; set;  }
        public string Comment { get; set; }        
    }
}
