using System.Threading;
using System.Threading.Tasks;

namespace Limit.Abstractions
{
    public interface IRequiresLimitChecker
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<bool> CheckAsync(RequiresLimitContext context, CancellationToken cancellation = default);

        /// <summary>
        /// 进行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task ProcessAsync(RequiresLimitContext context, CancellationToken cancellation = default);
    }
}
