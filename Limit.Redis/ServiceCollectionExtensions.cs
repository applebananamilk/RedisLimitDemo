using Limit.Abstractions;
using Limit.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Redis功能限制验证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static void AddRedisLimitValidation(this IServiceCollection services, Action<RedisRequiresLimitOptions> options)
        {
            services.Replace(ServiceDescriptor.Singleton<IRequiresLimitChecker, RedisRequiresLimitChecker>());

            services.Configure(options);

            services.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.Filters.Add<LimitValidationAsyncActionFilter>();
            });
        }
    }
}
