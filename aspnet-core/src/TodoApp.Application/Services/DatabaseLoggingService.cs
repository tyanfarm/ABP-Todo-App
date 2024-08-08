using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;     // Nếu `Abp.Domain.Repositories` sẽ bị lỗi khi inject Repository

namespace TodoApp.Services
{
    public class DatabaseLoggingService : ILoggingService
    {
        private readonly IRepository<LogUser, Guid> _logRepository;

        public DatabaseLoggingService(IRepository<LogUser, Guid> logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Log(string userId, string serviceName)
        {
            var logUser = new LogUser { UserId = userId, ServiceName = serviceName };

            await _logRepository.InsertAsync(logUser);
        }
    }
}
