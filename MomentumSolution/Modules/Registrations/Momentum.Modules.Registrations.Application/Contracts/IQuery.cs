using MediatR;

namespace Momentum.Modules.Registrations.Application.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}