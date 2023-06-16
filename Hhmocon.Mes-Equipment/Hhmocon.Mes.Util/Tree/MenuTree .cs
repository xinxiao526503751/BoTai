using System.Collections.Generic;

namespace Hhmocon.Mes.Util
{
    /// <summary>
    /// 描 述：树结构数据
    /// </summary>
    public class MenuTree
    {
        /// <summary>
        /// 菜单id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 打开方式（menuItem页签 menuBlank新窗口） 
        /// </summary>
        public string target { get; set; }

        /// <summary> 
        /// 菜单类型（M目录 C菜单 F按钮） 
        /// </summary> 
        public string menu_type { get; set; }
        /// <summary>
        /// 显示图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string perms { get; set; }

        /// <summary> 
        /// 菜单状态（1显示 ;0隐藏） 
        /// </summary> 
        public int visible { get; set; }

        /// <summary>
        /// 子节点列表数据
        /// </summary>
        public List<MenuTree> children { get; set; }
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public string parentId { get; set; }
    }

}
