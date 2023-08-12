using System.ComponentModel.DataAnnotations;
namespace productExpiry_system.models

{
    public class  Productmodel
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        //public DateTime UpdateAt  { get; set; }

    }
}
