using Application.DTOs;
using Application.DTOs.Product;
using Application.Services.Implementations;
using Application.Services.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class ProductService : BaseService , IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context, IHttpContextAccessor accessor) : base(accessor)
        {
            _context = context;
        }

        public async Task<BaseResponse> Create(CreateProductDTO create)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Name == create.Name && !x.IsDeleted && x.StoreId == StoreId);
            if (product != null) return BaseResponse.Failure("26", "Duplicate Entry - Product exists");
            product = new Product
            {
                Name = create.Name,
                Category = create.Category,
                StockCount = create.StockCount,
                ExpiryDate = create.ExpiryDate,
                ManufacturedDate = create.ManufacturedDate,
                Price = create.Price,
                StoreId = StoreId
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return BaseResponse.Success();
        }

        public async Task<BaseResponse> Update(UpdateProductDTO update)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == update.ProductId && !x.IsDeleted  && x.StoreId == StoreId);
            if (product == null) return BaseResponse.Failure("25", "No record Found - Product does not exist");
            product.Name = update.Name;
            product.Category = update.Category;
            product.StockCount = update.StockCount;
            product.ExpiryDate = update.ExpiryDate;
            product.ManufacturedDate = update.ManufacturedDate;
            product.Price = update.Price;
            product.DateUpdated = DateTime.Now;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return BaseResponse.Success();
        }

        public async Task<BaseResponse> Delete(long id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (product is null || product.StoreId != StoreId) return BaseResponse.Failure("25", "No record Found - Product does not exist");
            product.IsDeleted = true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return BaseResponse.Success();
        }

        public async Task<BaseResponse> Get(long id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (product is null || product.StoreId != StoreId) return BaseResponse.Failure("25", "No record Found - Product does not exist");
            var productDTO = new GetProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Category = product.Category,
                StockCount = product.StockCount,
                ExpiryDate = product.ExpiryDate,
                ManufacturedDate = product.ManufacturedDate,
                Price = product.Price,
                DateCreated = product.DateCreated,
                DateUpdated = product.DateUpdated
            };
            return BaseResponse<GetProductDTO>.Success(productDTO);
        }

        public PageBaseResponse<List<GetProductDTO>> List(PaginationQuery query)
        {
            var productQuery = _context.Products.Where(x => x.StoreId == StoreId && !x.IsDeleted);

            if (query.SearchText != null)
            {
                productQuery = productQuery.Where(x => x.Name.ToLower().Contains(query.SearchText.ToLower()));
            }

            var skip = (query.PageNumber - 1) * query.PageSize;

            var filteredQueryable = productQuery.Skip(skip).Take(query.PageSize);
            var recordCount = productQuery.Count();
            var pageCount = Convert.ToInt32(Math.Ceiling((double)recordCount / (double)query.PageSize));
            var pageNumber = query.PageNumber >= 1 ? query.PageNumber : (int?)null;
            var pageSize = query.PageSize >= 1 ? query.PageSize : (int?)null;

            var data = filteredQueryable.Select(x => new GetProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.Category,
                StockCount = x.StockCount,
                ExpiryDate = x.ExpiryDate,
                ManufacturedDate = x.ManufacturedDate,
                Price = x.Price,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated
            }).ToList();

            return PageBaseResponse<List<GetProductDTO>>.Success(data, pageNumber, pageSize, pageCount, recordCount, productQuery.Count());
        }
    }
}
