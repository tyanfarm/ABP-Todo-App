using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Todos;
using Xunit;

namespace TodoApp.EntityFrameworkCore.Applications.Todos
{
    [Collection(TodoAppTestConsts.CollectionDefinitionName)]
    public class EfCoreTodoAppService_Tests : TodoAppService_Tests<TodoAppEntityFrameworkCoreTestModule>
    {
    }
}
