using System.Reflection;
using Momentum.Modules.Registrations.Application.Contracts;

namespace Momentum.Modules.Registrations.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(IRegistrationsModule).Assembly;
    }
}