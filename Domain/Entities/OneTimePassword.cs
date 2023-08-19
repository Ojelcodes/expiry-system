using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("one_time_passwords")]
    public class OneTimePassword : BaseEntity
    {
        public string OTP { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Status { get; set; }
        public string Action { get; set; }
        public long ApplicationUserId { get; set; }
    }
}
