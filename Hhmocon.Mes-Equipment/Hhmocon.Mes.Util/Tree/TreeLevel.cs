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
    public class TreeLevel
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
        /// 节点深度
        /// </summary>
        public string level { get; set; }

        /// 是否被勾选0 for unchecked, 1 for partial checked, 2 for checked
        /// </summary>
        public int checkstate { get; set; }

        /// <summary>
        /// 子节点列表数据
        /// </summary>
        public List<TreeLevel> children { get; set; }
    }

    /// <summary>
    /// 这个类在添加角色权限的时候使用，暂时放这
    /// </summary>
    public class ModelLevel
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
        /// 节点深度
        /// </summary>
        public string level { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public string role_id { get; set; }

    }
}
