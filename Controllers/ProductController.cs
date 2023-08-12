using Microsoft.AspNetCore.Mvc;
using productExpiry_system.Interface.Repository;
using productExpiry_system.models;


namespace productExpiry_system.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var Allproducts = await _productRepository.GetProductsAsync();
           
            return Ok(Allproducts);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var products = await _productRepository.GetProductsByIdAsync(id);
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> AddnewProduct([FromBody] Productmodel productmodel)
        {
           var product = await _productRepository.AddProductAsync(productmodel);
            //if (productmodel.ExpiryDate > DateTime.UtcNow)
            //{
            //    return ("product ={0} is about to expire", productmodel.ProductName);
            //}
          
            return Ok(product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] Productmodel productmodel, [FromRoute] string id)
        {
            var products = await _productRepository.UpdateproductAsync(id, productmodel);
            return Ok(products);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct ([FromRoute] string id)
        {
            await _productRepository.DeleteProductAsync(id);
            return Ok();
        }
    }
}
