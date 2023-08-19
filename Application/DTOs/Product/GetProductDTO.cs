using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class GetProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int StockCount { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime DateCreated { get; set; } 
        public DateTime? DateUpdated { get; set; }
    }
}
