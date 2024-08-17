using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;

namespace TodoApp
{
    public class Order : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }

        // Khai báo virtual để EF core hỗ trợ lazy loading
        public virtual Product? Product { get; set; }
        public virtual IdentityUser? Customer { get; set; }  
    }
}
