using System;

namespace SharedHardware.Models
{
    public class Geolocation
    {
        public Guid Id { get; set; }
        public String Country { get; set; }
        public String State { get; set; }
        public String City { get; set; }
    }
}
