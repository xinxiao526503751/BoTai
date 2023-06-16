using System.Collections.Generic;

namespace Hhmocon.Mes.Util
{
    /// <summary>
    /// 描 述：树结构数据
    /// </summary>
    public class TreeModelEx<T> where T : class
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 父级节点ID
        /// </summary>
        public string parentId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int nodetype { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// 子节点列表数据
        /// </summary>
        public List<TreeModelEx<T>> children { get; set; }
    }
}
