using System;

namespace SharedHardware.Models
{
    public class PlatformEventLog
    {
        public enum PlatformEventType { Up, Down, SystemDeploy, ComputationDeploy, ComputationStart, ComputationEnd, ComputationFailed }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public long EventId { get; set; }
        public PlatformEventType Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
    }
}
