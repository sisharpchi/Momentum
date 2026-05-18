using MediatR;

namespace Momentum.Modules.Habits.Application.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>
{
    Guid Id { get; }
}
