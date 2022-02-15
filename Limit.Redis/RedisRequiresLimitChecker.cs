using Limit.Abstractions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Limit.Redis
{
    public class RedisRequiresLimitChecker : IRequiresLimitChecker
    {
        protected RedisRequiresLimitOptions Options { get; }

        private IDatabaseAsync _database;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        public RedisRequiresLimitChecker(IOptions<RedisRequiresLimitOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Value;
        }

        public async Task<bool> CheckAsync(RequiresLimitContext context, CancellationToken cancellation = default)
        {
            await ConnectAsync();

            if (await _database.KeyExistsAsync(CalculateCacheKey(context)))
            {
                var result = await _database.StringGetAsync(CalculateCacheKey(context));

                return (int)result + 1 <= context.LimitCount;
            }
            else
            {
                return true;
            }
        }

        public async Task ProcessAsync(RequiresLimitContext context, CancellationToken cancellation = default)
        {
            await ConnectAsync();

            string cacheKey = CalculateCacheKey(context);

            if (await _database.KeyExistsAsync(cacheKey))
            {
                await _database.StringIncrementAsync(cacheKey);
            }
            else
            {
                await _database.StringSetAsync(cacheKey, "1", new TimeSpan(0, 0, context.LimitSecond), When.Always);
            }
        }

        protected virtual string CalculateCacheKey(RequiresLimitContext context)
        {
            return $"{Options.Prefix}f:RedisRequiresLimitChecker,ln:{context.LimitName}";
        }

        protected virtual async Task ConnectAsync(CancellationToken cancellation = default)
        {
            cancellation.ThrowIfCancellationRequested();

            if (_database != null)
            {
                return;
            }

            // 控制并发
            await _connectionLock.WaitAsync(cancellation);

            try
            {
                if (_database == null)
                {
                    var connection = await ConnectionMultiplexer.ConnectAsync(Options.Configuration);
                    _database = connection.GetDatabase();
                }
            }
            finally
            {
                _connectionLock.Release();
            }
        }
    }
}
