/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : 6/24/2021                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : FUCKTHEREGULATIONS
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using System.ComponentModel;

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    public class PageReq2
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        [DefaultValue(1)]
        public int page { get; set; }
        /// <summary>
        /// 每页数据条数
        /// </summary>
        [DefaultValue(10)]
        public int rows { get; set; }

        /// <summary>
        /// 组合条件
        /// </summary>
        [DefaultValue("")]
        public string key { get; set; }

        /// <summary>
        /// 排序规则  ASC  DESC  
        /// </summary>

        [DefaultValue("ASC")]


        public string sort { get; set; }  //asc  desc

        /// <summary>
        /// 排序字段 多个中间用“,”隔开
        /// </summary>
        [DefaultValue("create_time")]
        public string order { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public PageReq2()
        {
            page = 1;
            rows = 10;
        }
    }
}