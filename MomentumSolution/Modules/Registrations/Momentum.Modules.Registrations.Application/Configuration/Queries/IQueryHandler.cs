using Momentum.Modules.Registrations.Application.Contracts;
using MediatR;

namespace Momentum.Modules.Registrations.Application.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}