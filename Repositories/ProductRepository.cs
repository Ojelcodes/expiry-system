using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productExpiry_system.entities;
using productExpiry_system.Interface.Repository;
using productExpiry_system.models;
using System.Linq;

using System.Threading.Tasks;

namespace productExpiry_system.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DBproduct _dBproducts;
        public ProductRepository(DBproduct dBproducts)
        {
            _dBproducts = dBproducts;
        }
        public async Task<List<Productmodel>> GetProductsAsync()
        {
            var products = await _dBproducts.Products.Select(x => new Productmodel()
            {
                ProductName = x.ProductName,
                Category = x.Category,
                Price = x.Price,
                ExpiryDate = x.ExpiryDate,
            }
           ).ToListAsync();
            return products;

        }
        public async Task<Productmodel> GetProductsByIdAsync(int ProductId)
        {
            var product = await _dBproducts.Products.Where(x => x.Id == ProductId).Select(x => new Productmodel()
            {
                ProductId = x.Id,
                ProductName = x.ProductName,
                Price = x.Price,
                Category = x.Category,
                ExpiryDate = x.ExpiryDate

            }).FirstOrDefaultAsync();


            return product;

        }
        public async Task<int> AddProductAsync(Productmodel productmodel)
        {
            var product = new Product();
/*            product.Id = productmodel.ProductId;*/
            product.ProductName = productmodel.ProductName;
            product.Email = productmodel.Email;
            product.Category = productmodel.Category;
            product.Price = productmodel.Price;
            product.ExpiryDate = productmodel.ExpiryDate;
            _dBproducts.Products.Add(product);
            await _dBproducts.SaveChangesAsync();
            return product.Id;

        }
        public async Task<Productmodel> UpdateproductAsync(string productId, Productmodel productmodel)
        {
            var updateitem = await _dBproducts.Products.FindAsync(productId);
            bool saved = false;
            if (updateitem != null)
            {
                productmodel.ProductName = updateitem.ProductName;
                productmodel.Category = updateitem.Category;
                productmodel.ExpiryDate = updateitem.ExpiryDate;
                productmodel.Price = updateitem.Price;

                saved = (await _dBproducts.SaveChangesAsync()) > 0;
                //if (saved)
                //{
                //    productmodel.UpdateAt = updateitem.UpdatedAt;
                //}
            }
            return productmodel;
           
        }
        public async Task DeleteProductAsync(string productId)
        {
            var RemoveItem = await _dBproducts.Products.FindAsync(productId);
            _dBproducts.Products.Remove(RemoveItem);
            await _dBproducts.SaveChangesAsync();

        }
        //public async Task CheckForExpiryasnc(Productmodel productmodel)
        //{
        //    var Expiredate = await _dBproducts.Products .Select(x => new Productmodel()
        //    {
        //        //ProductName = x.ProductName,
        //        //Category = x.Category,
        //        ExpiryDate = x.ExpiryDate,
        //    }
        //   ).ToListAsync();
        //  DateTime  Date = Expiredate.ToString;
        //    DateTime CurrentDate = DateTime.Now.Date;
        //    //string  numberoftDate = (Date- CurrentDate).To

        //    string RemaingDays = (Expiredate - CurrentDate).TotalDays.ToString();
        //    if (int.Parse(RemaingDays)
        //    {

        //    }
        //    return products;
        //}
    }
        
                
               
            
            

        

    
}

