using System;
using System.Collections.Generic;
using System.Text;

namespace SharedHardware.Models
{
    public class PlatformToPlatformTag
    {
        public Guid PlatformId { get; set; }
        public Platform Platform { get; set; }
        public long PlatformTagId { get; set; }
        public PlatformTag PlatformTag { get; set; }
    }
}
