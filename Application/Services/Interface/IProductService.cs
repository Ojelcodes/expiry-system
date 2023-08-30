using Application.DTOs;
using Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IProductService
    {
        BaseResponse<List<GetProductDTO>> AboutToExpire();
        BaseResponse<List<GetProductDTO>> AboutToRunOutOfStock();
        Task<BaseResponse> Create(CreateProductDTO create);
        Task<BaseResponse> Delete(long id);
        Task<BaseResponse> Get(long id);
        PageBaseResponse<List<GetProductDTO>> List(PaginationQuery query);
        Task<BaseResponse> Update(UpdateProductDTO update);
    }
}
