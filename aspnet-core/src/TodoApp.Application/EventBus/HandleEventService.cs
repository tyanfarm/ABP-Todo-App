using Abp.Dependency;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace TodoApp.EventBus
{
    public class HandleEventService
        : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>,
        ITransientDependency
    {
        private readonly ILogger<IdentityUser> _logger;

        public HandleEventService(ILogger<IdentityUser> logger)
        {
            _logger = logger;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
            var userName = eventData.Entity.UserName;
            var email = eventData.Entity.Email;
            System.Diagnostics.Debug.Print($"{userName} & {email} is registered");
        }
    }
}
