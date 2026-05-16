using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.Registrations.Application.Configuration.Queries;

namespace Momentum.Modules.Registrations.Application.UserRegistrations.GetUserRegistration
{
    internal class GetUserRegistrationQueryHandler : IQueryHandler<GetUserRegistrationQuery, UserRegistrationDto>
    {
        private readonly IMainRepository _mainRepository;

        public GetUserRegistrationQueryHandler(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        public Task<UserRegistrationDto> Handle(GetUserRegistrationQuery query, CancellationToken cancellationToken)
        {
            return UserRegistrationProvider.GetById(_mainRepository, query.UserRegistrationId, cancellationToken);
        }
    }
}
