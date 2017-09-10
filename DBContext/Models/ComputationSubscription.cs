using System;

namespace SharedHardware.Models
{
    public class ComputationSubscription
    {
        public enum ComputationSubscriptionType { Start, Done }
        public Guid ComputationId { get; set; }
        public Computation Computation { get; set; }
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public ComputationSubscriptionType Type { get; set; }
    }
}
