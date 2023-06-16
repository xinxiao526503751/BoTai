using System;

namespace Hhmocon.Mes.Repository.LoginRelated
{
    /// <summary>
    /// 公共异常类
    /// </summary>
    public class CommonException : Exception
    {

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public CommonException(string message, int code) : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; }
    }
}
