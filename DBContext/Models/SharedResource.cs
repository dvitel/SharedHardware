using System;

namespace SharedHardware.Models
{
    public class SharedResource
    {
        public enum Per { Minute, Hour, Day } //TODO: extend
                                              //TODO: automatically check availability of existing interpretators
                                              //OS - file system and process launch
                                              //IIS, Apache - site hosting 

        public Guid PlatformId { get; set; }
        public byte ResourceId { get; set; }        
        public Platform Platform { get; set; }
        public DateTime CreationDate { get; set; }        
        public DateTime UpdateDate { get; set; }
        public int? CpuCoreNumber { get; set; }
        public int? CpuPercentage { get; set; }
        public int? Ram { get; set; }
        public int? DiskSpace { get; set; }
        public decimal? Price { get; set; } //Price and/or MinAuctionPrice should be set - in other case it is Free
        public decimal? MinAuctionPrice { get; set; } //if set - hardware is participated in auction too
        public Per Period { get; set; }
        public Guid? AvailabilityTimeId { get; set; }
        public Schedule AvailabilityTime { get; set; }
        public bool IsActive { get; set; } //becomes unactive when schedule passes
        //public bool Enabled 
        //System will create a user with limited rights for specific directory 
        //CPU and RAM limits will be controlled by Job Objects (Windows) 
        //Disk Size is controled by Disk Quotas

        //TODO: 
        //GPU limits
        //NIC limits
    }
}
