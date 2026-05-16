namespace Momentum.Modules.UserAccess.Application.Authorization.GetUserPermissions
{
    public class UserPermissionDto
    {
        public Guid UserId { get; set; }

        public string Code { get; set; } = null!;
    }
}
