using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using Volo.Abp.Application.Services;

namespace TodoApp
{
    public interface IOrderService : IApplicationService
    {
        Task<OrderDto> PlaceOrderAsync(Guid productId, int quantity);
        Task<List<OrderDto>> GetListAsync();
        Task<OrderDto> CreateAsync(OrderDto data);
        Task DeleteAsync(Guid id);
    }
}
