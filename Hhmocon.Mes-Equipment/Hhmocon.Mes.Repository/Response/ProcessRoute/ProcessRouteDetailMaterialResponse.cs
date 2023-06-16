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

namespace Hhmocon.Mes.Repository.Response
{
    /// <summary>
    /// 工艺路线工序细节物料回复类
    /// </summary>
    public class ProcessRouteDetailMaterialResponse
    {
        /// <summary>
        /// 物料名称
        /// </summary>
        public string material_name;

        /// <summary>
        /// 物料id
        /// </summary>
        public string material_id;

        /// <summary>
        /// 物料编码
        /// </summary>
        public string material_code;

        /// <summary>
        /// 数量
        /// </summary>
        public int qty;

        /// <summary>
        /// 是否主物料
        /// </summary>
        public int is_main;
    }
}
