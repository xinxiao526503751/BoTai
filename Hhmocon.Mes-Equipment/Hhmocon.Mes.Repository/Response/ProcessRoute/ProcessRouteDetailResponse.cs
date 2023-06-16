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
    /// 工艺路线工序细节回复类
    /// </summary>
    public class ProcessRouteDetailResponse
    {
        /// <summary>
        /// 工艺路线细节id
        /// </summary>
        public string process_route_detail_id;

        /// <summary>
        /// 工艺路线id
        /// </summary>
        public string process_route_id;

        /// <summary>
        /// 工序id
        /// </summary>
        public string process_id;

        /// <summary>
        /// 工序编码
        /// </summary>
        public string process_code;

        /// <summary>
        /// 工序名称
        /// </summary>
        public string process_name;

        /// <summary>
        /// 工序顺序
        /// </summary>
        public int process_seq;
    }
}
