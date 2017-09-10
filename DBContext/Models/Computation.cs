using System;
using System.Collections.Generic;

namespace SharedHardware.Models
{
    public class Computation
    {
        public Guid Id { get; set; }
        public string BundleUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid? LastDeploymentId { get; set; }
        public ComputationDeployment LastDeployment { get; set;  }
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public Guid PlatformTagId { get; set; }
        public PlatformTag PlatformTag { get; set; }
        public string EntryPoint { get; set; }
    }
}
