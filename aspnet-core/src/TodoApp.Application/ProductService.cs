using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TodoApp
{
    public class ProductService : ApplicationService, IProductService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductService(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> CreateAsync(ProductDto data)
        {
            var product = ObjectMapper.Map<ProductDto, Product>(data);

            var result = await _productRepository.InsertAsync(product);

            return ObjectMapper.Map<Product, ProductDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        public async Task<List<ProductDto>> GetListAsync()
        {
            var products = await _productRepository.GetListAsync();

            return ObjectMapper.Map<List<Product>, List<ProductDto>>(products); 
        }
    }
}
