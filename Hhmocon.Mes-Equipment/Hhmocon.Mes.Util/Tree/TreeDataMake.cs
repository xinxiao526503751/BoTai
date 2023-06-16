using System.Collections.Generic;

namespace Hhmocon.Mes.Util
{
    /// <summary>
    /// 描 述：树结构数据
    /// </summary>
    public static class TreeDataMake
    {
        /// <summary>
        /// 树形数据转化
        /// </summary>
        /// <param name="list">数据</param>
        /// <returns></returns>
        public static List<TreeModel> ToTree(this List<TreeModel> list, string parentId = "")
        {
            Dictionary<string, List<TreeModel>> childrenMap = new Dictionary<string, List<TreeModel>>();
            Dictionary<string, TreeModel> parentMap = new Dictionary<string, TreeModel>();
            List<TreeModel> res = new List<TreeModel>();

            //首先按照
            foreach (TreeModel node in list)
            {
                node.hasChildren = false;
                node.complete = true;
                // 注册子节点映射表
                if (!childrenMap.ContainsKey(node.parentId))
                {
                    childrenMap.Add(node.parentId, new List<TreeModel>());
                }
                else if (parentMap.ContainsKey(node.parentId))
                {
                    parentMap[node.parentId].hasChildren = true;
                }
                childrenMap[node.parentId].Add(node);
                // 注册父节点映射节点表
                parentMap.Add(node.id, node);

                // 查找自己的子节点
                if (!childrenMap.ContainsKey(node.id))
                {
                    childrenMap.Add(node.id, new List<TreeModel>());
                }
                else
                {
                    node.hasChildren = true;
                }
                node.children = childrenMap[node.id];
            }

            if (string.IsNullOrEmpty(parentId))
            {
                // 获取祖先数据列表
                foreach (KeyValuePair<string, List<TreeModel>> item in childrenMap)
                {
                    if (!parentMap.ContainsKey(item.Key))
                    {
                        res.AddRange(item.Value);
                    }
                }
            }
            else
            {
                if (childrenMap.ContainsKey(parentId))
                {
                    return childrenMap[parentId];
                }
                else
                {
                    return new List<TreeModel>();
                }
            }
            return res;
        }

        /// <summary>
        /// 树形数据转化
        /// </summary>
        /// <param name="list">数据</param>
        /// <returns></returns>
        public static List<TreeModelEx<T>> ToTree<T>(this List<TreeModelEx<T>> list, string parentId = "") where T : class
        {
            Dictionary<string, List<TreeModelEx<T>>> childrenMap = new Dictionary<string, List<TreeModelEx<T>>>();
            Dictionary<string, TreeModelEx<T>> parentMap = new Dictionary<string, TreeModelEx<T>>();
            List<TreeModelEx<T>> res = new List<TreeModelEx<T>>();

            //首先按照
            foreach (TreeModelEx<T> node in list)
            {
                // 注册子节点映射表
                if (!childrenMap.ContainsKey(node.parentId))
                {
                    childrenMap.Add(node.parentId, new List<TreeModelEx<T>>());
                }
                childrenMap[node.parentId].Add(node);
                // 注册父节点映射节点表
                parentMap.Add(node.id, node);

                // 查找自己的子节点
                if (!childrenMap.ContainsKey(node.id))
                {
                    childrenMap.Add(node.id, new List<TreeModelEx<T>>());
                }

                node.children = childrenMap[node.id];
            }


            // 获取祖先数据列表

            if (string.IsNullOrEmpty(parentId))
            {
                foreach (KeyValuePair<string, List<TreeModelEx<T>>> item in childrenMap)
                {
                    if (!parentMap.ContainsKey(item.Key))
                    {
                        res.AddRange(item.Value);
                    }
                }
            }
            else
            {
                if (parentMap.ContainsKey(parentId))
                {
                    res.Add(parentMap[parentId]);
                }
            }
            return res;
        }



        /// <summary>
        /// 树形数据转化
        /// </summary>
        /// <param name="list">数据</param>
        /// <returns></returns>
        public static List<MenuTree> ToTree(this List<MenuTree> list, string parentId = "")
        {
            Dictionary<string, List<MenuTree>> childrenMap = new Dictionary<string, List<MenuTree>>();
            Dictionary<string, MenuTree> parentMap = new Dictionary<string, MenuTree>();
            List<MenuTree> res = new List<MenuTree>();

            //首先按照
            foreach (MenuTree node in list)
            {
                // 注册子节点映射表
                if (!childrenMap.ContainsKey(node.parentId))
                {
                    childrenMap.Add(node.parentId, new List<MenuTree>());
                }

                childrenMap[node.parentId].Add(node);
                // 注册父节点映射节点表
                parentMap.Add(node.id, node);

                // 查找自己的子节点
                if (!childrenMap.ContainsKey(node.id))
                {
                    childrenMap.Add(node.id, new List<MenuTree>());
                }

                node.children = childrenMap[node.id];
            }

            if (string.IsNullOrEmpty(parentId))
            {
                // 获取祖先数据列表
                foreach (KeyValuePair<string, List<MenuTree>> item in childrenMap)
                {
                    if (!parentMap.ContainsKey(item.Key))
                    {
                        res.AddRange(item.Value);
                    }
                }
            }
            else
            {
                if (childrenMap.ContainsKey(parentId))
                {
                    return childrenMap[parentId];
                }
                else
                {
                    return new List<MenuTree>();
                }
            }
            return res;
        }



        /// <summary>
        /// 树形数据转化
        /// </summary>
        /// <param name="list">数据</param>
        /// <returns></returns>
        public static List<MenuTreeEx> ToTree(this List<MenuTreeEx> list, string parentId = "")
        {
            Dictionary<string, List<MenuTreeEx>> childrenMap = new Dictionary<string, List<MenuTreeEx>>();
            Dictionary<string, MenuTreeEx> parentMap = new Dictionary<string, MenuTreeEx>();
            List<MenuTreeEx> res = new List<MenuTreeEx>();

            //首先按照
            foreach (MenuTreeEx node in list)
            {
                // 注册子节点映射表
                if (!childrenMap.ContainsKey(node.parentId))
                {
                    childrenMap.Add(node.parentId, new List<MenuTreeEx>());
                }

                childrenMap[node.parentId].Add(node);
                // 注册父节点映射节点表
                parentMap.Add(node.id, node);

                // 查找自己的子节点
                if (!childrenMap.ContainsKey(node.id))
                {
                    childrenMap.Add(node.id, new List<MenuTreeEx>());
                }

                node.Children = childrenMap[node.id];
            }

            if (string.IsNullOrEmpty(parentId))
            {
                // 获取祖先数据列表
                foreach (KeyValuePair<string, List<MenuTreeEx>> item in childrenMap)
                {
                    if (!parentMap.ContainsKey(item.Key))
                    {
                        res.AddRange(item.Value);
                    }
                }
            }
            else
            {
                if (childrenMap.ContainsKey(parentId))
                {
                    return childrenMap[parentId];
                }
                else
                {
                    return new List<MenuTreeEx>();
                }
            }
            return res;
        }

    }
}
