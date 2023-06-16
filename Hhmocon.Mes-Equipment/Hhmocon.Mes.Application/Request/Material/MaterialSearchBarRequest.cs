/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using System.ComponentModel;

namespace Hhmocon.Mes.Application.Request
{
    public class MaterialSearchBarRequest
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        [DefaultValue(1)]
        public int Page { get; set; }
        /// <summary>
        /// 每页数据条数
        /// </summary>
        [DefaultValue(10)]
        public int Rows { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// name或者cn_name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 物料类型id
        /// </summary>
        public string Type_Id { get; set; }
    }
}
