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

using Newtonsoft.Json.Converters;

namespace Hhmocon.Mes.Util
{
    /// <summary>
    /// json属性值转为日期
    /// </summary>
    public class DateConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// 类构造
        /// </summary>
        public DateConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
