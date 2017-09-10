using System;

namespace SharedHardware.Models
{
    public class Contact
    {
        public enum ContactType { Email, SMS }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public String Name { get; set; }
        public ContactType Type { get; set; }
        public string Address { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Confirmed { get; set; }
        public bool IsMain { get; set; }
    }
}
