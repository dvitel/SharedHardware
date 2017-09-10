using System;

namespace SharedHardware.Models
{
    public class NotificationLog
    {
        public enum NotificationType { Up, Down, Start, Done }
        public Guid Id { get; set; }
        public long ContactId { get; set; }
        public Contact Contact { get; set; }
        public NotificationType Type { get; set; }
        public bool IsSent { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }        
        public string Message { get; set; } //fail message
    }
}
