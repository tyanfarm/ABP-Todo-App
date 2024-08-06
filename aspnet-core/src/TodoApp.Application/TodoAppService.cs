using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Dtos;
using TodoApp.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TodoApp
{
    public class TodoAppService : ApplicationService, ITodoAppService
    {
        private readonly IRepository<TodoItem, Guid> _repository;

        public TodoAppService(IRepository<TodoItem, Guid> repository)
        {
            _repository = repository;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<TodoItemDto> CreateAsync(string text)
        {
            var item = await _repository.InsertAsync(
                new TodoItem { Text = text }
            );

            return ObjectMapper.Map<TodoItem, TodoItemDto>(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync( id );
        }

        //[Authorize(AuthenticationSchemes = "Bearer", Policy ="")]
        //[Authorize(TodoAppPermissions.Todo.Default)]
        public async Task<List<TodoItemDto>> GetListAsync()
        {
            var items = await _repository.GetListAsync();

            return ObjectMapper.Map<List<TodoItem>, List<TodoItemDto>>(items);
        }

        public async Task<TodoItemDto> UpdateAsync(Guid id, string text)
        {
            var item = await _repository.SingleOrDefaultAsync(x => x.Id == id );

            if (item == null) {
                return null;
            }

            item.Text = text;
            await _repository.UpdateAsync( item );

            return ObjectMapper.Map<TodoItem, TodoItemDto>(item);
        }
    }
}
