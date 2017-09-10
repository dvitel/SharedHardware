using System;

namespace SharedHardware.Models
{
    public class Credit
    {
        public Guid Id { get; set; }
        public Guid ResourceOwnerId { get; set; }
        public User ResourceOwner { get; set; }
        public Guid ResourceUserId { get; set; }
        public User ResourceUser { get; set; }
        public decimal ToPay { get; set; }
        public bool IsPaid { get; set;  }
    }
}
