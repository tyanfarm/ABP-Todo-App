using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace TodoApp.Services
{
    public interface ILoggingService 
    {
        Task Log(string userId);
    }
}
