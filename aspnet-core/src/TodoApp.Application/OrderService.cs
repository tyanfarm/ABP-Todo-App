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
    public class OrderService : ApplicationService, IOrderService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Product, Guid> _productRespository; 

        public OrderService(
            IRepository<Order, Guid> orderRepository, 
            IRepository<Product, Guid> productRespository
        )
        {
            _orderRepository = orderRepository;
            _productRespository = productRespository;
        }

        public async Task<OrderDto> CreateAsync(OrderDto data)
        {
            data.OrderDate = DateTime.Now;
            var order = ObjectMapper.Map<OrderDto, Order>(data);

            var result = await _orderRepository.InsertAsync(order);

            return ObjectMapper.Map<Order, OrderDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<List<OrderDto>> GetListAsync()
        {
            var orders = await _orderRepository.GetListAsync();

            return ObjectMapper.Map<List<Order>, List<OrderDto>>(orders);
        }

        public async Task<OrderDto> PlaceOrderAsync(Guid productId, int quantity)
        {
            var product = await _productRespository.GetAsync(productId);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            if (product.Quantity < quantity)
            {
                throw new Exception("Not enought stock available.");
            }

            var order = new Order
            {
                ProductId = productId,
                OrderDate = DateTime.Now,
                Quantity = quantity
            };

            product.Quantity -= quantity;
            
            await _orderRepository.InsertAsync(order);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }
    }
}
