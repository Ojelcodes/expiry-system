using productExpiry_system.models;



namespace productExpiry_system.Interface.Repository
{
    public interface IProductRepository
    {
        public Task<List<Productmodel>> GetProductsAsync();
        Task<Productmodel> GetProductsByIdAsync(int ProductId);
        Task<int> AddProductAsync(Productmodel productmodel);
        Task<Productmodel> UpdateproductAsync(string productId, Productmodel productmodel);
        Task DeleteProductAsync(string productId);


    }
}
