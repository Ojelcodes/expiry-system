namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public  decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public int StockCount { get; set; }
        public int StoreId { get; set; }
    }
}
