using Momentum.Modules.UserAccess.Application.Contracts;
using Momentum.Modules.UserAccess.Application.Users.GetUser;

namespace Momentum.Modules.UserAccess.Application.Users.GetAuthenticatedUser
{
    public class GetAuthenticatedUserQuery : QueryBase<UserDto>
    {
        public GetAuthenticatedUserQuery()
        {
        }
    }
}