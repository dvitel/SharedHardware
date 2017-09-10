using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedHardware.Models
{
    public class AccountConfirmationCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
