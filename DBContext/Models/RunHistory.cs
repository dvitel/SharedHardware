using System;
using System.Collections.Generic;
using System.Text;

namespace SharedHardware.Models
{
    public class RunHistory
    {
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public Guid SharedResourceId { get; set; }
        public SharedResource SharedResource { get; set; }
        public Guid ComputationId { get; set; }
        public Computation Computation;
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DoneTime { get; set; }
    }
}
