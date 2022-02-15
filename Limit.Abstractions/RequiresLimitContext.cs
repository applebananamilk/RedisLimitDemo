namespace Limit.Abstractions
{
    /// <summary>
    /// 限制验证上下文
    /// </summary>
    public class RequiresLimitContext
    {
        /// <summary>
        /// 限制名称
        /// </summary>
        public string LimitName { get; }
        /// <summary>
        /// 默认限制时长(秒)
        /// </summary>
        public int LimitSecond { get; }
        /// <summary>
        /// 限制次数
        /// </summary>
        public int LimitCount { get; }

        // 其它

        public RequiresLimitContext(string limitName, int limitSecond, int limitCount)
        {
            LimitName = limitName;
            LimitSecond = limitSecond;
            LimitCount = limitCount;
        }
    }
}
