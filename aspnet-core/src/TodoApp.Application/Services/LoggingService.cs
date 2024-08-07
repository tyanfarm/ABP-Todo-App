using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;     // Nếu `Abp.Domain.Repositories` sẽ bị lỗi khi inject Repository

namespace TodoApp.Services
{
    public class LoggingService : ILoggingService
    {
        // 
        private readonly IRepository<LogUser, Guid> _repository;

        public LoggingService(IRepository<LogUser, Guid> repository)
        {
            _repository = repository;
        }

        public Task Log(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
