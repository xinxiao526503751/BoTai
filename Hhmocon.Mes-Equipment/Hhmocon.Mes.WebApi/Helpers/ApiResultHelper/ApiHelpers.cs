using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hhmocon.Mes.WebApi.Helpers.ApiResultHelper
{
    /// <summary>
    /// 返回值帮助类
    /// </summary>
    public class ApiHelpers
    {
        /// <summary>
        /// 请求成功后返回的数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult Success(dynamic data)
        {
            return new ApiResult
            {
                Code = 200,
                Data = data,
                Msg = "操作成功",
                Total = 1
            };
        }
        /// <summary>
        /// 能返回总数值的帮助类
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult ReturnCountAndSuccess(dynamic data)
        {
            return new ApiResult
            {
                Code = 200,
                Data = data,
                Msg = "操作成功",
                Total = data.Count
            };
        }

        /// <summary>
        /// 请求错误时返回的数据
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResult Error(string msg)
        {
            return new ApiResult
            {
                Code = 500,
                Data = null,
                Msg = msg,
                Total = 0
            };
        }
    }
}
