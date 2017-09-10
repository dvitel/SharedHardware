using System;

namespace SharedHardware.Models
{
    public class ComputationDeployment
    {
        public enum ComputationDeploymentState { Ok, Failed, InProgress }
        public Guid Id { get; set; }
        public Guid ComputationId { get; set; }
        public Computation Computation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? DoneTime { get; set; }
        public ComputationDeploymentState State { get; set; }        
        public Guid SystemDeploymentId { get; set; }
        public SystemDeployment SystemDeployment { get; set; }
        public string Message { get; set; }
    }
}
