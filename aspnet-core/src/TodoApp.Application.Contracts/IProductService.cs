using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using Volo.Abp.Application.Services;

namespace TodoApp
{
    public interface IProductService : IApplicationService
    {
        Task<List<ProductDto>> GetListAsync();
        Task<ProductDto> CreateAsync(ProductDto data);
        Task DeleteAsync(Guid id);
    }
}
