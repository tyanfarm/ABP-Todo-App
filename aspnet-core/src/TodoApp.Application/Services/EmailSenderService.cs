using Abp.Application.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using Volo.Abp.DependencyInjection;
using static Volo.Abp.Ui.LayoutHooks.LayoutHooks;

namespace TodoApp.Services
{
    public class EmailSenderService : ApplicationService, IEmailSenderService, IScopedDependency
    {
        private readonly EmailInfoDto _emailInfo;

        public EmailSenderService(IOptions<EmailInfoDto> emailInfo)
        {
            _emailInfo = emailInfo.Value;
        }

        public async Task SendEmailAsync(EmailContentDto data)
        {
            var sender = _emailInfo.Username;
            var password = _emailInfo.Password;

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, password)
            };

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(sender);
            mailMessage.To.Add(data.Email);
            mailMessage.Subject = data.Subject;
            mailMessage.Body = data.Body;
            mailMessage.IsBodyHtml = true;

            await client.SendMailAsync(mailMessage);
        }
    }
}
