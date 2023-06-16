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

using System;

namespace Hhmocon.Mes.Util.SwaggerConfig
{
    /// <summary>
    /// 系统模块枚举注释
    /// </summary>
    public class GroupInfoAttribute : Attribute
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
    }
}
