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

namespace Hhmocon.Mes.Application.Response
{
    /// <summary>
    /// 根据事件的id
    /// 查找已经完成的相关事件流程的
    /// 确认人、确认时间
    /// 处理人、处理时间
    /// 关闭人、关闭时间
    /// </summary>
    public class FaultDealPeopleAndDealTime
    {
        /// <summary>
        /// 确认人
        /// </summary>
        public string ConfirmPeoPle;
        /// <summary>
        /// 确认时间
        /// </summary>
        public string ConfirmTime;

        /// <summary>
        /// 处理人
        /// </summary>
        public string DealPeoPle;
        /// <summary>
        /// 处理时间
        /// </summary>
        public string DealTime;

        /// <summary>
        /// 关闭人
        /// </summary>
        public string ShutPeoPle;
        /// <summary>
        /// 关闭时间
        /// </summary>
        public string ShutTime;

    }
}
