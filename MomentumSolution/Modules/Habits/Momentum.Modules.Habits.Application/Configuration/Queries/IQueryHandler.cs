using MediatR;
using Momentum.Modules.Habits.Application.Contracts;

namespace Momentum.Modules.Habits.Application.Configuration.Queries;

public interface IQueryHandler<in TQuery, TResult> :
    IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}
