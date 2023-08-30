using Application.DTOs;
using Application.DTOs.Product;
using Application.Services.Implementation;
using Application.Services.Interface;
using BusinessZoneCentral_UserAPI.Controllers;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace productExpiry_system.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        /// <summary>
        /// Get Filtered Product
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        [HttpGet("[action]")]
        [Authorize]
        public ActionResult List([FromQuery] PaginationQuery query)
        {
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(_productService.List(query));
        }

        /// <summary>
        /// Get About to expire Product
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        [HttpGet("[action]")]
        [Authorize]
        public ActionResult AboutToExpire()
        {
            return HandleResult(_productService.AboutToExpire());
        }

        /// <summary>
        /// Get about to run out of stock Product
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        [HttpGet("[action]")]
        [Authorize]
        public ActionResult AboutToRunOutOfStock()
        {
            return HandleResult(_productService.AboutToRunOutOfStock());
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(404, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> Get(long id)
        {
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _productService.Get(id));
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="create"></param>
        /// <returns></returns>
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(404, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> Create(CreateProductDTO create)
        {
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _productService.Create(create));
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(404, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> Update(UpdateProductDTO update)
        {
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _productService.Update(update));
        }
        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(400, Type = typeof(BaseResponse))]
        [ProducesResponseType(404, Type = typeof(BaseResponse))]
        [ProducesResponseType(200, Type = typeof(BaseResponse))]
        [Authorize]
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!ModelState.IsValid) return BadRequest(ResponseHelper.BuildResponse("30", ModelState));
            return HandleResult(await _productService.Delete(id));
        }
    }
}
