using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using Volo.Abp.Application.Services;

namespace TodoApp.Services
{
    public interface IEmailSenderService : IApplicationService
    {
        Task SendEmailAsync(EmailContentDto data);
    }
}
