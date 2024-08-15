using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace TodoApp
{
    public class OrderService : ApplicationService, IOrderService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Product, Guid> _productRespository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OrderService(
            IRepository<Order, Guid> orderRepository, 
            IRepository<Product, Guid> productRespository,
            IUnitOfWorkManager unitOfWorkManager
        )
        {
            _orderRepository = orderRepository;
            _productRespository = productRespository;
            _unitOfWorkManager = unitOfWorkManager;
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
           using (var uow = _unitOfWorkManager.Begin(
               requiresNew: true, isTransactional: true
           ))
           {
                try
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

                    await Task.Delay(3000);

                    var order = new Order
                    {
                        ProductId = productId,
                        OrderDate = DateTime.Now,
                        Quantity = quantity
                    };

                    product.Quantity -= quantity;

                    // ABP có Change Tracking nên không cần Update _productRepository
                    await _orderRepository.InsertAsync(order);

                    await uow.CompleteAsync();

                    return ObjectMapper.Map<Order, OrderDto>(order);
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}", ex);
                }
           }
        }
    }
}
