using System.Reflection;
using Momentum.Modules.UserAccess.Application.Contracts;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration
{
    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(IUserAccessModule).Assembly;
    }
}