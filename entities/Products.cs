namespace productExpiry_system.entities
{
    public class Product
    {
        public int Id { get; set; } /*= Guid.NewGuid().ToString();*/
        public string ProductName { get; set; }
        public string Email { get; set; }

        public string Category { get; set; }
        public  int Price { get; set; }
        public DateTime Created { get; set; }
        //public DateTime UpdatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        
    }
}
