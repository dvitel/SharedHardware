using System;

namespace SharedHardware.Models
{
    public class PlatformSubscription
    {
        public enum PlatformSubscriptionType { Down, Up }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public PlatformSubscriptionType Type { get; set; }
    }
}
