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

using System.Collections.Generic;

namespace Hhmocon.Mes.Util
{
    public class TreeEasy
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 节点提示
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 子节点列表数据
        /// </summary>
        public List<TreeEasy> children { get; set; }

        /// <summary>
        /// 结点类型  "location""equipment"   "sys_dept""user"  用在sysparmtype的时候对应的是type_default,0代表false,1代表true
        /// </summary>
        public string NodeType { get; set; }
    }
}
