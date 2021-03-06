﻿using System;

namespace SharedHardware.Models
{
    public class Run
    {
        public Guid Id { get; set; }
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public byte SharedResourceId { get; set; }
        public SharedResource SharedResource { get; set; }
        public Guid ComputationId { get; set; }
        public Computation Computation;
        public DateTime StartTime { get; set; }
    }
}
