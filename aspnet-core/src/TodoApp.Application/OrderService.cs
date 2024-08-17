using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Dtos;
using TodoApp.EntityFrameworkCore;
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
        private readonly TodoAppDbContext _context;

        public OrderService(
            IRepository<Order, Guid> orderRepository,
            IRepository<Product, Guid> productRespository,
            IUnitOfWorkManager unitOfWorkManager,
            TodoAppDbContext context
        )
        {
            _orderRepository = orderRepository;
            _productRespository = productRespository;
            _unitOfWorkManager = unitOfWorkManager;
            _context = context;
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

        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            // Đã config Mapper giữa Product -> ProductDto nên khi Map Order -> OrderDto
            // sẽ tự động map Product -> ProductDto 
            // IdentityUser -> IdentityUserDto AutoMapping tương tự
            var order = await _context.Orders.Include(o => o.Product)
                                        .Include(o => o.Customer)
                                        .FirstOrDefaultAsync(o => o.Id == orderId);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public async Task<OrderDto> PlaceOrderAsync(Guid productId, int quantity)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
            {
                var product = await _context.Products.
                        FromSqlRaw("SELECT * FROM Products WHERE Id = {0} FOR UPDATE", productId)
                        .FirstOrDefaultAsync();

                if (product == null)
                {
                    throw new Exception("Product not found.");
                }

                if (product.Quantity < quantity)
                {
                    throw new Exception("Not enought stock available.");
                }

                // Concurrency (Tính đồng thời) Test
                await Task.Delay(3000);

                var order = new Order
                {
                    ProductId = productId,
                    CustomerId = Guid.Parse("3a146b87-a9f4-9479-4398-0881fc02ce57"),
                    OrderDate = DateTime.Now,
                    Quantity = quantity
                };

                // Vì sử dụng SqlRaw để Pessimistic Lock nên update bằng dbContext
                // Update bằng productRepository sẽ không thành công
                product.Quantity -= quantity;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                await _orderRepository.InsertAsync(order);

                await transaction.CommitAsync();

                return ObjectMapper.Map<Order, OrderDto>(order);
            }
        }
    }
}
