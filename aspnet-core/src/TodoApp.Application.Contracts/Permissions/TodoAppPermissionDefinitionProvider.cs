using TodoApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace TodoApp.Permissions;

public class TodoAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(TodoAppPermissions.GroupName);

        var todoPermission = myGroup.AddPermission(TodoAppPermissions.Todo.Default, L("Permission:Default"));   // TodoApp.Todo
        todoPermission.AddChild(TodoAppPermissions.Todo.Create, L("Permission:Create"));    // TodoApp.Todo.Create
        todoPermission.AddChild(TodoAppPermissions.Todo.Update, L("Permission:Update"));    // TodoApp.Todo.Update
        todoPermission.AddChild(TodoAppPermissions.Todo.Delete, L("Permission:Delete"));    // TodoApp.Todo.Delete
        //Define your own permissions here. Example:
        //myGroup.AddPermission(TodoAppPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TodoAppResource>(name);
    }
}
