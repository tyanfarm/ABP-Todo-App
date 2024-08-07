using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace TodoApp
{
    public class LogUser : Entity<Guid>
    {
        public string UserId { get; set; }
        public DateTime ExecutionTime { get; set; }

        public LogUser() 
        {
            ExecutionTime = DateTime.Now;
        }
    }
}
