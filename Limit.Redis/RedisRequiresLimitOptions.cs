using Microsoft.Extensions.Options;

namespace Limit.Redis
{
    public class RedisRequiresLimitOptions : IOptions<RedisRequiresLimitOptions>
    {
        /// <summary>
        /// Redis连接字符串
        /// </summary>
        public string Configuration { get; set; }
        /// <summary>
        /// Key前缀
        /// </summary>
        public string Prefix { get; set; }

        public RedisRequiresLimitOptions Value => this;
    }
}
