using System;
using System.Collections.Generic;

namespace SharedHardware.Models
{
    public class Schedule
    {
        public enum ScheduleType { OneTime, Regular, Cron }
        public Guid Id { get; set; }
        public ScheduleType Type { get; set; }
        public DateTime? StartDateTime { get; set; }  // StartDateTime or CronSchedule should be set
        public string CronSchedule { get; set; }
        public TimeSpan Interval { get; set; }
        //public ICollection<SharedResource> SharedResources { get; set; }
    }
}
