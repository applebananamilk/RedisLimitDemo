using System;

namespace Limit.Abstractions
{
    /// <summary>
    /// 限制特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RequiresLimitAttribute : Attribute
    {
        /// <summary>
        /// 限制名称
        /// </summary>
        public string LimitName { get; }
        /// <summary>
        /// 限制时长(秒)
        /// </summary>
        public int LimitSecond { get; }
        /// <summary>
        /// 限制次数
        /// </summary>
        public int LimitCount { get; }

        public RequiresLimitAttribute(string limitName, int limitSecond = 1, int limitCount = 1)
        {
            if (string.IsNullOrWhiteSpace(limitName))
            {
                throw new ArgumentNullException(nameof(limitName));
            }

            LimitName = limitName;
            LimitSecond = limitSecond;
            LimitCount = limitCount;
        }
    }
}
