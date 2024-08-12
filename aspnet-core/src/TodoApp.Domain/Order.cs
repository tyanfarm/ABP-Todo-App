using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace TodoApp
{
    public class Order : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Order()
        {
            OrderDate = DateTime.Now;
        }
    }
}
