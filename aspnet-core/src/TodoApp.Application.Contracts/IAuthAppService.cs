using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace TodoApp
{
    public interface IAuthAppService : IApplicationService
    {
        Task<string> LoginAsync(UserLoginDto input);
        Task<IdentityUserDto> RegisterAsync(UserRegisterDto input);
    }
}
