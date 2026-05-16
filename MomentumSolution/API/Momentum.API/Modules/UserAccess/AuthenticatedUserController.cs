using Microsoft.AspNetCore.Mvc;
using Momentum.API.Configuration.Authorization;
using Momentum.Modules.UserAccess.Application.Authorization.GetAuthenticatedUserPermissions;
using Momentum.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using Momentum.Modules.UserAccess.Application.Contracts;
using Momentum.Modules.UserAccess.Application.Users.GetAuthenticatedUser;
using Momentum.Modules.UserAccess.Application.Users.GetUser;

namespace Momentum.API.Modules.UserAccess
{
    [Route("api/userAccess/authenticatedUser")]
    [ApiController]
    public class AuthenticatedUserController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public AuthenticatedUserController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        [NoPermissionRequired]
        [HttpGet("")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUser()
        {
            var user = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserQuery());

            return Ok(user);
        }

        [NoPermissionRequired]
        [HttpGet("permissions")]
        [ProducesResponseType(typeof(List<UserPermissionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthenticatedUserPermissions()
        {
            var permissions = await _userAccessModule.ExecuteQueryAsync(new GetAuthenticatedUserPermissionsQuery());

            return Ok(permissions);
        }
    }
}