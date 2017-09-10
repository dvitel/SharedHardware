using System;

namespace SharedHardware.Models
{
    public class SystemDeployment
    {
        public enum SystemDeploymentState { Ok, Failed, InProgress }
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public SystemDeploymentState State { get; set; }
        public string Message { get; set; }
    }
}
