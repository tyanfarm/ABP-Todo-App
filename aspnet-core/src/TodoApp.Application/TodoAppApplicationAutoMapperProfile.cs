using AutoMapper;
using TodoApp.Dtos;
using Volo.Abp.Identity;

namespace TodoApp;

public class TodoAppApplicationAutoMapperProfile : Profile
{
    public TodoAppApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<TodoItem, TodoItemDto>();
        CreateMap<IdentityUser, IdentityUserDto>();
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
    }
}
