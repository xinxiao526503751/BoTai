/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : 6/24/2021                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : FUCKTHEREGULATIONS
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Hhmocon.Mes.Database;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.String;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 应用层所有能用泛型简化的代码
    /// 具有复用性的  删改查   都可以由这个类做掉
    /// </summary>
    public  class PikachuApp
    {
        private PikachuRepository _pikachuRepository;
        private SqlHelper _sqlHelper;
        private readonly IAuth _auth;
        public PikachuApp(PikachuRepository pikachuRepository, IBaseMaterialRepository baseMaterialRepository,SqlHelper sqlHelper, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _sqlHelper = sqlHelper;
            _auth = auth;
        }

        /// <summary>
        /// 根据id获取单个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(string id, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            return _pikachuRepository.GetById<T>(id,returnFields,tran,commandTimeout,dbConnection);
        }

        /// <summary>
        /// 根据id获取全部数据，真查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetAllById<T>(string id)
        {
            return _pikachuRepository.GetAllById<T>(id);
        }


        /// <summary>
        /// 获取某一表的所有数据，假查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(PageReq req = null,IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            return _pikachuRepository.GetAll<T>(tran,commandTimeout, dbConnection);
        }
        /// <summary>
        /// 根据条件获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll<T>(PageReq req, ref long icount)
        {
            string strKey = req.key;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;
            if (!string.IsNullOrEmpty(whereStr))
            {
                whereStr = "WHERE " + whereStr;
            }

            if (!string.IsNullOrEmpty(strOrder))
            {
                ordStr = strOrder;
                if (!string.IsNullOrEmpty(strSort))
                {
                    ordStr = "ORDER BY " + ordStr + " " + strSort;
                }
                else
                {
                    ordStr = "ORDER BY " + ordStr + " ";
                }
            }
            icount = _pikachuRepository.GetCount<T>(whereStr);
            return _pikachuRepository.GetAll<T>(whereStr, ordStr);
        }


        /// <summary>
        /// 根据Ids获取全部数据
        /// </summary>
        /// <typeparam name=""></typeparam>
        /// <returns></returns>
        public List<T> GetAllByIds<T>(string[] ids)
        {
            return _pikachuRepository.GetAllByIds<T>(ids);
        }

        /// <summary>
        /// 根据Code获取单个数据(假查询)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <returns></returns>
        public T GetByCode<T>(string code)
        {
            return _pikachuRepository.GetByCode<T>(code);
        }

        /// <summary>
        /// 根据name获取单个。
        /// 警告：(建议每个模块单独写，不建议偷懒)2021/8/12
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetByName<T>(string name)
        {
            return _pikachuRepository.GetByName<T>(name);
        }

        /// <summary>
        /// 根据parentid获取所有
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<T> GetAllByParentid<T>(string parentid)
        {
            return _pikachuRepository.GetAllByParentId<T>(parentid);
        }


        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <param name="icount"></param>
        /// <returns></returns>
        public List<T> GetList<T>(PageReq req)
        {
            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;

            if (!string.IsNullOrEmpty(strOrder))
            {
                ordStr = strOrder;
                if (!string.IsNullOrEmpty(strSort))
                {
                    ordStr = "ORDER BY " + ordStr + " " + strSort;
                }
                else
                {
                    ordStr = "ORDER BY " + ordStr + " ";
                }
            }
            //假查询部分
            whereStr = SqlAssemble.Delete_Mark(whereStr);
            return _pikachuRepository.GetList<T>(iPage, iRows, whereStr, ordStr);
        }

        /// <summary>
        /// 根据条件获取分页数据，带总数统计
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <param name="icount"></param>
        /// <returns></returns>
        public List<T> GetList<T>(PageReq req, ref long icount)
        {
            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;

            if (!string.IsNullOrEmpty(strOrder))
            {
                ordStr = strOrder;
                if (!string.IsNullOrEmpty(strSort))
                {
                    ordStr = "ORDER BY " + ordStr + " " + strSort;
                }
                else
                {
                    ordStr = "ORDER BY " + ordStr + " ";
                }
            }
            //假查询部分
            whereStr = SqlAssemble.Delete_Mark(whereStr);

            icount = _pikachuRepository.GetCount<T>(whereStr);
            return _pikachuRepository.GetList<T>(iPage, iRows, whereStr, ordStr);
        }


        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update<T>(T data,string updateFields=null, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            dynamic dyn = data;
            dyn.modified_by = _auth.GetUserAccount(null);
            dyn.modified_by_name = _auth.GetUserName(null);
            return _pikachuRepository.Update(dyn, updateFields,tran,commandTimeout,dbConnection);
        }


        /// <summary>
        /// 删除数据 假删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteMask<T>(string[] id)
        {
            return _pikachuRepository.Delete_Mask<T>(id);
        }

        #region 为树的枝和叶分别是两种类型做的封装
        /// <summary>
        /// 树状查询,查找所有parentid = 参数
        /// 返回1+all
        /// 前序遍历
        /// </summary>
        /// <returns>无值返回fault，是空不是Null</returns>
        public List<T> GetRootAndBranch<T>(string _parentid,int rootneed = 1)
        {
            return _pikachuRepository.GetRootAndBranch<T>(_parentid, rootneed);
        }




        /// <summary>
        /// 查找枝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <param name="rootAndBranch"></param>
        /// <returns></returns>
        private void SearchBranch<T>(T _parent, ref List<T> rootAndBranch)
        {
            _pikachuRepository.SearchBranch<T>(_parent,ref rootAndBranch);
        }

       

        /// <summary>
        /// 根据类型id找叶子
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="_type"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        private List<T1> GetLeafs<T, T1>(T _type,ref List<T1> leafs)
        {
             List<T> root;
             List<T1> leafs_temp;
           
            //根据类型找到叶子
            leafs_temp = _pikachuRepository.GetByType<T,T1>(_type);
            if (leafs_temp != null)//如果能找到
            {
                //添加叶子
                leafs.AddRange(leafs_temp);
            }

            //查找根下面的枝
            root = _pikachuRepository.GetAllByParent(_type);

            if (root != null)
            {
                //遍历枝
                foreach (T r in root)
                {
                    GetLeafs(r,ref leafs);
                }
            }

            
            return leafs;
        }

        #endregion



        /// <summary>
        /// 将list<T>转换成list<TreeModel>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">需要转的的List</param>
        /// <param name="UseInLocationEquip">是否用在地点设备页面,默认为true</param>
        /// <returns></returns>
        public List<TreeModel> ListElementToNode<T>(List<T> list,bool UseInLocationEquip=true)
        {
      
            List<TreeModel> treeModels = new();
            string className = typeof(T).Name;
            foreach (T temp in list)
            {
                TreeModel treeModel = new();
                dynamic dynamic = temp;
                if (UseInLocationEquip)
                {
                    switch (className)
                    {
                        case "base_equipment":
                            treeModel.id = dynamic.equipment_id;
                            treeModel.label = dynamic.equipment_name;
                            treeModel.parentId = dynamic.location_id;//所有的设备都是主设备，在设备定义的工作单元页面，编号为3的窗口的添加按钮，点击后出现地点-设备树
                            break;
                        case "base_location":
                            treeModel.id = dynamic.location_id;
                            treeModel.label = dynamic.location_name;
                            treeModel.parentId = dynamic.location_parentid;
                            break;
                        case "base_warehouse": //仓库定义没有父子级别关系
                            treeModel.id = dynamic.warehouse_id;
                            treeModel.label = dynamic.warehouse_name;
                            treeModel.parentId = null;
                            break;
                        case "base_material":
                            treeModel.id = dynamic.material_id;
                            treeModel.label = dynamic.material_name;
                            //treeModel.menu_type = "1";//0是类型，1是定义
                            treeModel.parentId = dynamic.material_type_id;
                            break;
                        case "base_material_type":
                            treeModel.id = dynamic.material_type_id;
                            treeModel.label = dynamic.material_type_name;
                            //treeModel.menu_type = "0";//0是类型，1是定义
                            treeModel.parentId = dynamic.material_type_parentid;
                            break;
                        case "base_warehouse_loc":
                            treeModel.id = dynamic.warehouse_loc_id;
                            treeModel.label = dynamic.warehouse_loc_name;
                            treeModel.parentId = null;
                            break;
                    }
                }
                else 
                {
                    switch (className)
                    {
                        case "base_equipment_sub":
                            treeModel.id = dynamic.equipment_sub_id;
                            base_equipment t = _pikachuRepository.GetById<base_equipment>(treeModel.id);
                            treeModel.label = t.equipment_name;
                            treeModel.parentId = dynamic.equipment_id;//工作单元层级树
                            break;
                        case "base_location":
                            treeModel.id = dynamic.location_id;
                            treeModel.label = dynamic.location_name;
                            treeModel.parentId = dynamic.location_parentid;
                            break;
                        case "sys_dept":
                            treeModel.id = dynamic.dept_id;
                            treeModel.label = dynamic.dept_name;
                            treeModel.parentId = dynamic.parent_dept_id;
                            break;
                        case "base_fault_class":
                            treeModel.id = dynamic.fault_class_id;
                            treeModel.label = dynamic.fault_class_name;
                            treeModel.parentId = null;//事件类型只有一层，父Id为Null
                            break;

                        case "base_fault":
                            treeModel.id = dynamic.fault_id;
                            treeModel.label = dynamic.fault_name;
                            treeModel.parentId = null;//事件没有父子关系
                            break;

                        case "sys_right":
                            treeModel.id = dynamic.right_id;
                            treeModel.label = dynamic.right_name;
                            treeModel.parentId = dynamic.parent_right_id;
                            break;
                    }
                }
                
                treeModels.Add(treeModel);
            }
            return treeModels;
        }

        /// <summary>
        /// 化为List<MenuTree>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<MenuTree> ListElementToMenuNode<T>(List<T> list)
        {

            List<MenuTree> treeModels = new();
            string className = typeof(T).Name;
            foreach (T temp in list)
            {
                MenuTree treeModel = new();
                dynamic dynamic = temp;

                {
                    switch (className)
                    { 
                        case "sys_right":
                            treeModel.id = dynamic.right_id;
                            treeModel.url = dynamic.right_url;
                            treeModel.name = dynamic.right_name;
                            treeModel.menu_type = dynamic.right_type;
                            treeModel.parentId = dynamic.parent_right_id;
                            break;
                        case "sys_system":
                            treeModel.id = dynamic.sys_id;
                            treeModel.name = dynamic.sys_name;
                            treeModel.url = dynamic.sys_url;
                            treeModel.parentId = dynamic.parent_sys_id;
                            break;
                        case "sys_sub_right":
                            treeModel.id = dynamic.right_id;
                            treeModel.name = dynamic.sub_right_name;
                            treeModel.parentId = dynamic.parent_right_id;
                            break;
                        case "base_material_type":
                            treeModel.id = dynamic.material_type_id;
                            treeModel.name = dynamic.material_type_name;
                            treeModel.menu_type = "0";//0是类型，1是定义
                            treeModel.parentId = dynamic.material_type_parentid;
                            break;
                        case "base_material":
                            treeModel.id = dynamic.material_id;
                            treeModel.name = dynamic.material_name;
                            treeModel.menu_type = "1";//0是类型，1是定义
                            treeModel.parentId = dynamic.material_type_id;
                            break;
                    }
                }

                treeModels.Add(treeModel);
            }
            return treeModels;
        }

        /// <summary>
        /// 把List<MenuTree>转成树的形式
        /// </summary>
        /// <param name="Nodes"></param>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public List<MenuTree> ListToMenuTree(List<MenuTree> Nodes, string parent_id = null)
        {
            List<MenuTree> treeModels = new();

            //补充children_node
            foreach (MenuTree temp in Nodes)
            {
                if (string.IsNullOrEmpty(temp.parentId))
                {
                    continue;
                }
                else
                {
                    foreach (MenuTree temp2 in Nodes)
                    {

                        if (temp.parentId.Equals(temp2.id))
                        {
                            if (temp2.children == null)
                                temp2.children = new();
                            temp2.children.Add(temp);
                        }

                    }
                }
            }

            foreach (MenuTree temp in Nodes)
            {
                //树里面是只放根的.根的父节点默认为null,但也有可能为参数parent_id
                if (string.IsNullOrEmpty(temp.parentId) || temp.parentId.Equals(parent_id))
                {
                    treeModels.Add(temp);
                }
            }
            return treeModels;
        }

        /// <summary>
        /// 将 节点列表转化为树
        /// </summary>
        /// <param name="Nodes">List<TreeModel> Nodes</param>
        /// <param name="parent_id">根节点的父id</param>
        /// <returns></returns>
        public List<TreeModel> ListToTreeModel(List<TreeModel> Nodes,string parent_id = null)
        {
            List<TreeModel> treeModels = new();

            //补充children_node
            foreach (TreeModel temp in Nodes)
            {
                if (string.IsNullOrEmpty(temp.parentId))
                {
                    continue;
                }
                else {
                    foreach (TreeModel temp2 in Nodes)
                    {
                        
                        if (temp.parentId.Equals(temp2.id))
                        {
                            if(temp2.children==null)
                                temp2.children = new();
                            temp2.children.Add(temp);
                        }
                        
                    }
                }
            }

            foreach (TreeModel temp in Nodes)
            {
                //树里面是只放根的.根的父节点默认为null,但也有可能为参数parent_id
                if (string.IsNullOrEmpty(temp.parentId) || temp.parentId.Equals(parent_id))
                {
                    treeModels.Add(temp);
                }
            }
            return treeModels;
        }

        /// <summary>
        /// 对根进行遍历，复制每一个节点
        /// </summary>
        /// <param name="root"></param>
        /// <param name="trees"></param>
        /// <param name="treeEasy"></param>
        /// <returns></returns>
        public List<TreeEasy>  TreeModelToTreeEasy(List<TreeModel> root)
        {
            List<TreeEasy> treeEasies = new();
            foreach (var temp in root) {
                TreeEasy treeEasy = new();
                treeEasy.id = temp.id;
                treeEasy.label = temp.label;
                treeEasy.NodeType = temp.NodeType;
                treeEasy.children = new();
                if (temp.children != null)//如果子节点不为空，遍历子节点
                {
                     TreeModelToTreeEasy(temp.children,ref treeEasy);
                }
               
                treeEasies.Add(treeEasy);
            }
            return treeEasies;
        }

        public List<TreeEasy> TreeModelToTreeEasy(List<TreeModel> root,ref TreeEasy nodes_temp)
        {
            List<TreeEasy> treeEasies = new();
            foreach (var temp in root)
            {
                TreeEasy treeEasy = new();
                treeEasy.id = temp.id;
                treeEasy.label = temp.label;
                treeEasy.NodeType = temp.NodeType;
                treeEasy.children = new();
                if (temp.children != null)//如果子节点不为空，遍历子节点
                {
                    TreeModelToTreeEasy(temp.children,ref treeEasy);
                }

                if (nodes_temp != null)
                {
                    nodes_temp.children.Add(treeEasy);
                }
                treeEasies.Add(treeEasy);
            }
            return treeEasies;
        }


        /// <summary>
        /// 比上面多加一个节点深度
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public List<TreeLevel> TreeModelToTreeLevel(List<TreeModel> root)
        {
            List<TreeLevel> treeEasies = new();
        
            foreach (var temp in root)
            {
                TreeLevel treeEasy = new();
                treeEasy.id = temp.id;
                treeEasy.label = temp.label;
                treeEasy.level = temp.value;
                treeEasy.checkstate = temp.checkstate;
                treeEasy.children = new();
                if (temp.children != null)//如果子节点不为空，遍历子节点
                {
                    TreeModelToTreeLevel(temp.children, ref treeEasy);
                }

                treeEasies.Add(treeEasy);
            }
            return treeEasies;
        }

        public List<TreeLevel> TreeModelToTreeLevel(List<TreeModel> root, ref TreeLevel nodes_temp)
        {
            List<TreeLevel> treeEasies = new();
            foreach (var temp in root)
            {
                TreeLevel treeEasy = new();
                treeEasy.id = temp.id;
                treeEasy.label = temp.label;
                treeEasy.level = temp.value;
                treeEasy.checkstate = temp.checkstate;
                treeEasy.children = new();
                if (temp.children != null)//如果子节点不为空，遍历子节点
                {
                    TreeModelToTreeLevel(temp.children, ref treeEasy);
                }

                if (nodes_temp != null)
                {
                    nodes_temp.children.Add(treeEasy);
                }
                treeEasies.Add(treeEasy);
            }
            return treeEasies;
        }

        /// <summary>
        /// 一个属性 = 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field"></param>
        /// <param name="Field_value"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="dbConnection"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<T> GetByOneFeildsSql<T>(string Field, object Field_value, string returnFields = "*", IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            List<T> data = _pikachuRepository.GetByOneFeildsSql<T>(Field, Field_value, returnFields, tran, commandTimeout, dbConnection);
            return data;
        }

        /// <summary>
        /// 两个属性 = 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field1"></param>
        /// <param name="Field1_value"></param>
        /// <param name="Field2"></param>
        /// <param name="Field2_value"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public List<T> GetByTwoFeildsSql<T>(string Field1, string Field1_value, string Field2, List<string> Field2_value, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            List<T> data = _pikachuRepository.GetByTwoFeildsSql<T>(Field1, Field1_value, Field2, Field2_value, returnFields, tran, commandTimeout, dbConnection);
            return data;
        }

        /// <summary>
        /// 获取所有有着同一个字段的表名
        /// </summary>
        /// <param name="Field">字段名</param>
        /// <param name="DataBase">数据库名</param>
        /// <returns></returns>
        public List<string> GetAllChartNameHavingSameField(string Field)
        {
            List<string> ChartName = _pikachuRepository.GetAllChartNameHavingSameField(Field);
            return ChartName;
        }

        /// <summary>
        /// Type类获取类型方法(通过字符串型的类名)
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type typen(string typeName)
        {
            return _pikachuRepository.typen(typeName);
        }
    }
}
