using System.Threading;
using System.Threading.Tasks;

namespace Limit.Abstractions
{
    public class NullRequiresLimitChecker : IRequiresLimitChecker
    {
        public Task<bool> CheckAsync(RequiresLimitContext context, CancellationToken cancellation = default)
        {
            return Task.FromResult(true);
        }

        public Task ProcessAsync(RequiresLimitContext context, CancellationToken cancellation = default)
        {
            return Task.CompletedTask;
        }
    }
}
