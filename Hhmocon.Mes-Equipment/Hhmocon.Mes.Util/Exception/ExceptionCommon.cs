
using System;

namespace Hhmocon.Mes.Util
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionCommon : Exception
    {
        private readonly int _code;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public ExceptionCommon(string message, int code)
            : base(message)
        {
            _code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Code => _code;

    }
}
