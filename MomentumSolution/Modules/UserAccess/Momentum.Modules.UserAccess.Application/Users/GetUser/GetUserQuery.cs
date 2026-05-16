using Momentum.Modules.UserAccess.Application.Contracts;

namespace Momentum.Modules.UserAccess.Application.Users.GetUser
{
    public class GetUserQuery : QueryBase<UserDto>
    {
        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}