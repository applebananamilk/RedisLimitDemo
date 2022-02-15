using System;

namespace Limit.Abstractions
{
    /// <summary>
    /// 限制验证失败异常
    /// </summary>
    public class LimitValidationFailedException : Exception
    {
        public LimitValidationFailedException(string limitName, int limitCount)
            : base($"功能{limitName}已到最大使用上限{limitCount}！")
        {

        }
    }
}
