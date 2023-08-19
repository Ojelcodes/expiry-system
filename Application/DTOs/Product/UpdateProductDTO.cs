using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class UpdateProductDTO
    {
        [Required]
        public long ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int StockCount { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        public DateTime? ManufacturedDate { get; set; }
    }
}
