//using Momentum.API.Configuration.Authorization;
//using Momentum.Modules.Registrations.Application.Contracts;
//using Momentum.Modules.Registrations.Application.UserRegistrations.ConfirmUserRegistration;
//using Momentum.Modules.Registrations.Application.UserRegistrations.RegisterNewUser;
//using Momentum.Modules.UserAccess.Application.Contracts;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace CompanyName.MyMeetings.API.Modules.UserAccess
//{
//    [Route("userAccess/[controller]")]
//    [ApiController]
//    public class UserRegistrationsController : ControllerBase
//    {
//        private readonly IRegistrationsModule _registrationsModule;

//        public UserRegistrationsController(IRegistrationsModule registrationsModule)
//        {
//            _registrationsModule = registrationsModule;
//        }

//        [NoPermissionRequired]
//        [AllowAnonymous]
//        [HttpPost("")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<IActionResult> RegisterNewUser(RegisterNewUserRequest request)
//        {
//            await _registrationsModule.ExecuteCommandAsync(new RegisterNewUserCommand(
//                request.Login,
//                request.Password,
//                request.Email,
//                request.FirstName,
//                request.LastName,
//                request.ConfirmLink));

//            return Ok();
//        }

//        [NoPermissionRequired]
//        [AllowAnonymous]
//        [HttpPatch("{userRegistrationId}/confirm")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<IActionResult> ConfirmRegistration(Guid userRegistrationId)
//        {
//            await _registrationsModule.ExecuteCommandAsync(new ConfirmUserRegistrationCommand(userRegistrationId));

//            return Ok();
//        }
//    }
//}