namespace Momentum.Modules.UserAccess.Application.Users.GetUser
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
