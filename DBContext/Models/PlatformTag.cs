using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SharedHardware.Models
{
    public class PlatformTag
    {
        public long Id { get; set; }
        public string Name { get; set; } //PHP, PowerShell, Bash, Exe, Java, Cmd
        public string Description { get; set; }
        public ICollection<Computation> Computations { get; set; }
        public ICollection<PlatformToPlatformTag> PlatformTags { get; set; }
    }
}
