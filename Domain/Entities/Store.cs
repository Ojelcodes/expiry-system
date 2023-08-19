using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Store : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Product> Products { get; set; }
    }
}
