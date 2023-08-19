using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
