using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Threading.Tasks;

namespace Limit.Abstractions
{
    /// <summary>
    /// 限制验证过滤器
    /// </summary>
    public class LimitValidationAsyncActionFilter : IAsyncActionFilter
    {
        public IRequiresLimitChecker RequiresLimitChecker { get; }

        public LimitValidationAsyncActionFilter(IRequiresLimitChecker requiresLimitChecker)
        {
            RequiresLimitChecker = requiresLimitChecker;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 获取特性
            var limitAttribute = GetRequiresLimitAttribute(GetMethodInfo(context));

            if (limitAttribute == null)
            {
                await next();
                return;
            }

            // 组装上下文
            var requiresLimitContext = new RequiresLimitContext(limitAttribute.LimitName, limitAttribute.LimitSecond, limitAttribute.LimitCount);

            // 检查
            await PreCheckAsync(requiresLimitContext);

            // 执行方法
            await next();

            // 次数自增
            await PostCheckAsync(requiresLimitContext);
        }

        protected virtual MethodInfo GetMethodInfo(ActionExecutingContext context)
        {
            return (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo;
        }

        /// <summary>
        /// 获取限制特性
        /// </summary>
        /// <returns></returns>
        protected virtual RequiresLimitAttribute GetRequiresLimitAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttribute<RequiresLimitAttribute>();
        }

        /// <summary>
        /// 验证之前
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual async Task PreCheckAsync(RequiresLimitContext context)
        {
            bool isAllowed = await RequiresLimitChecker.CheckAsync(context);
            if (!isAllowed)
            {
                throw new LimitValidationFailedException(context.LimitName, context.LimitCount);
            }
        }

        /// <summary>
        /// 验证之后
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual async Task PostCheckAsync(RequiresLimitContext context)
        {
            await RequiresLimitChecker.ProcessAsync(context);
        }
    }
}
