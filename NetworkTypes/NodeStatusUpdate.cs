using System;
using System.Collections.Generic;
using System.Text;

namespace SharedHardware.NetworkTypes
{
    public class SharedResourceStatusUpdate
    {
        public byte SharedResourceId { get; set; }
        public Guid ActiveComputationId { get; set; }
        public string ActiveComputationOutput { get; set; }
        public string ActiveComputationErrorOutput { get; set; }
    }
    public class NodeStatusUpdate
    {
        public Guid PlatformId { get; set; }          
        public SharedResourceStatusUpdate[] BusySharedResources { get; set; }
        public byte[] FreeSharedResources { get; set; }
    }

    public class SharedResourceStatusUpdateResult
    {
        public byte SharedResourceId { get; set; }
        public Guid ComputationId { get; set; }
        public string ComputationUrl { get; set; }
    }

    public class NodeStatusUpdateResult
    {
        public SharedResourceStatusUpdateResult[] ToStart { get; set; }
    }
}
