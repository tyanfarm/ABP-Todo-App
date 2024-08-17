using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace TodoApp.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual ProductDto? Product { get; set; }
        public virtual IdentityUserDto? Customer { get; set; }
    }
}
