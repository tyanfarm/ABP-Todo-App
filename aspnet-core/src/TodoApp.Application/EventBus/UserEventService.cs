using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using TodoApp.Dtos.Events;
using TodoApp.Services;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace TodoApp.EventBus
{
    public class UserEventService
        : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>,         // Publisher
        ILocalEventHandler<UserEvent>,
        ITransientDependency
    {
        private readonly IEmailSenderService _emailSender;
        private readonly IEnumerable<ILoggingService> _loggingService;

        public UserEventService(IEmailSenderService emailSender, IEnumerable<ILoggingService> loggingService)
        {
            _emailSender = emailSender;
            _loggingService = loggingService;
        }

        // Mỗi HandleEvent sẽ tương ứng với TEvent được đăng ký ở ILocalEventHandler phía trên

        // Subcriber - Gửi email báo đăng ký thành công
        public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
            var email = eventData.Entity.Email;

            var emailBody = @"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Sample Email with Button</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            margin: 0;
                            padding: 20px;
                            background-color: #f4f4f4;
                        }
                        .container {
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            padding: 20px;
                            border: 1px solid #dddddd;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }
                        h1 {
                            color: #333333;
                        }
                        p {
                            color: #666666;
                        }
                        .button {
                            display: inline-block;
                            background-color: #4CAF50;
                            color: white !important;
                            text-decoration: none;
                            padding: 10px 20px;
                            border-radius: 4px;
                            text-align: center;
                            font-size: 16px;
                            transition: background-color 0.3s ease;
                        }
                        .button:hover {
                            background-color: #45a049;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Welcome to Our Service</h1>
                        <p>Dear User,</p>
                        <p>Thank you for joining our service. We are excited to have you on board.</p>
                        <a href='#URL#' class='button'>Click Here</a>
                        <p>Best regards,<br>Tyanipo</p>
                    </div>
                </body>
                </html>";

            // Thay thế link vào chuỗi string
            var body = emailBody.Replace("#URL#", "https://www.google.com.vn");

            // Topic of mail
            var subject = "Welcome user";

            var emailContent = new EmailContentDto
            {
                Email = email,
                Subject = subject,
                Body = body
            };

            //System.Diagnostics.Debug.WriteLine($"{emailContent.Email} & {emailContent.Subject}");
            try
            {
                await _emailSender.SendEmailAsync(emailContent);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        // Subcriber - Log các service của User Authentication
        public async Task HandleEventAsync(UserEvent eventData)
        {
            var userId = eventData.UserId.ToString();
            var serviceName = eventData.ServiceName;

            foreach (var service in _loggingService)
            {
                await service.Log(userId, serviceName);
            }
        }
    }
}
