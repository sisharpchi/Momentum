using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Queries;

namespace Momentum.Modules.UserAccess.Application.Emails
{
    internal class GetAllEmailsQueryHandler : IQueryHandler<GetAllEmailsQuery, List<EmailDto>>
    {
        private readonly IMainRepository _mainRepository;

        public GetAllEmailsQueryHandler(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        public async Task<List<EmailDto>> Handle(GetAllEmailsQuery query, CancellationToken cancellationToken)
        {
            return await _mainRepository
                .Set<EmailDto>()
                .AsNoTracking()
                .OrderByDescending(email => email.Date)
                .ToListAsync(cancellationToken);
        }
    }
}
