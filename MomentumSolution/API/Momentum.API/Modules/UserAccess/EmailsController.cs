using Momentum.API.Configuration.Authorization;
using Momentum.Modules.UserAccess.Application.Contracts;
using Momentum.Modules.UserAccess.Application.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Momentum.API.Modules.UserAccess
{
    [Route("api/userAccess/emails")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public EmailsController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        [NoPermissionRequired]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetEmails()
        {
            var allEmails = await _userAccessModule.ExecuteQueryAsync(new GetAllEmailsQuery());

            return Ok(allEmails);
        }
    }
}