using System.Security.Claims;

namespace Momentum.Modules.UserAccess.Application.Authentication.Authenticate
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; } = null!;

        public bool IsActive { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public List<Claim> Claims { get; set; } = [];

        public string Password { get; set; } = null!;
    }
}
