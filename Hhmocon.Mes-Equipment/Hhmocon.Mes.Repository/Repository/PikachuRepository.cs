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

using Dapper;
using Hhmocon.Mes.Database;
using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 仓储层所有能用泛型简化的代码
    /// 所有对数据的增删改查都可以由这个类做掉
    /// </summary>
    public class PikachuRepository
    {
        private readonly SqlHelper _sqlHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlHelper"></param>
        public PikachuRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        /// <summary>
        /// 根据id获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(string id, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = 5, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            T data = conn.GetById<T>(id, returnFields, tran, commandTimeout);
            if (dbConnection == null)
            {
                conn.Close();
            }

            return data;

        }


        /// <summary>
        /// 根据id获取全部数据,假查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetAllById<T>(string id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<T> data = conn.GetByIds<T>(id).ToList();
                return data;
            }
        }

        /// <summary>
        /// 获取所有数据不分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetAll<T>(IDbTransaction tran = null, int? commandTimeout = 5, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();

            List<T> data = conn.GetAll<T>(tran: tran, commandTimeout: commandTimeout).ToList();
            if (dbConnection == null)
            {
                conn.Close();
            }
            return data;
        }
        public List<T> GetAll2<T>(IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();

            List<T> data = conn.GetAll2<T>(tran: tran, commandTimeout: commandTimeout).ToList();
            if (dbConnection == null)
            {
                conn.Close();
            }

            return data;
        }
        /// <summary>
        /// 获取所有带查询功能
        /// </summary>
        /// <param name="wherestr"></param>
        /// <param name="sortstr"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(string wherestr, string sortstr)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<T> listdata = null;
                listdata = conn.GetByWhere<T>(where: wherestr, orderBy: sortstr).ToList();
                return listdata;
            }
        }

        /// <summary>
        /// 根据Ids获取全部数据，假查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetAllByIds<T>(string[] ids)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<T> data = conn.GetByIds<T>(ids).ToList();
                return data;
            }
        }


        /// <summary>
        /// 根据code获取数据,没有数据则返回Null
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public T GetByCode<T>(string code, IDbTransaction tran = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            T data = conn.GetByCode<T>(code);

            if (dbConnection == null)
            {
                conn.Close();
            }

            return data;
        }

        /// <summary>
        /// 根据Name获取,取第一个
        /// 警告：该方法于sys模块之后不再被建议使用。停止更新。21/8/12
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetByName<T>(string name, IDbTransaction tran = null, IDbConnection dbConnection = null)
        {
            using (IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn())
            {
                T data;
                string chartName = typeof(T).Name;
                string sql = _sqlHelper.GetByName<T>(name);
                List<T> temp = GetbySql<T>(sql);
                if (temp.Count != 0)
                {
                    data = GetbySql<T>(sql)[0];
                }
                else
                {
                    data = default;
                }

                return data;
            }
        }

        /// <summary>
        /// 获取当前条件下的数据记录数
        /// </summary>
        /// <param name="wherestr"></param>
        /// <returns></returns>
        public long GetCount<T>(string wherestr)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                return conn.GetTotal<T>(wherestr);
            }
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="wherestr"></param>
        /// <param name="sortstr"></param>
        /// <returns></returns>
        public List<T> GetList<T>(int page, int rows, string wherestr, string sortstr)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<T> listdata = null;
                listdata = conn.GetByPageIndex<T>(page, rows, where: wherestr, orderBy: sortstr).ToList();
                return listdata;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Insert<T>(T obj, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            int iret = conn.Insert(obj, tran, commandTimeout);
            if (dbConnection == null)
            {
                conn.Close();
            }

            return iret > 0;
        }


        /// <summary>
        /// 跟新新增数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertOrUpdate<T>(T data, string updateFields = null, bool update = true, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            int iret = conn.InsertOrUpdate(data, updateFields, update: true, tran, commandTimeout);
            if (dbConnection == null)
            {
                conn.Close();
            }

            return iret > 0;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update<T>(T data, string updateFields = null, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            int iret = conn.Update(data, updateFields, tran, commandTimeout);
            if (dbConnection == null)
            {
                conn.Close();
            }

            return iret > 0;
        }

        /// <summary>
        /// 根据id数组 假删除 相应的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public bool Delete_Mask<T>(string[] ids, IDbTransaction tran = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            List<T> listdata = conn.GetByIds<T>(ids, tran: tran).ToList();
            int iret = 0;
            try
            {
                foreach (dynamic temp in listdata)
                {
                    temp.delete_mark = 1;

                    //这里用了强制转换，可能有风险，后续改进
                    conn.Update((T)temp, tran: tran);

                    iret++;
                }
                if (dbConnection == null)
                {
                    conn.Close();
                }

                return iret > 0;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }


        /// <summary>
        /// 通过sql查询内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetbySql<T>(string sql)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<T> data = conn.GetByWhere<T>(where: sql).ToList();

                return data;
            }
        }

        /// <summary>
        /// 通过sql和sortstr查询内容，返回列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="sortstr"></param>
        /// <returns></returns>
        public List<T> GetbySql<T>(string sql, string sortstr)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<T> data = conn.GetByWhere<T>(where: sql, orderBy: sortstr).ToList();

                return data;
            }
        }

        /// <summary>
        /// 通过sql和sortstr查询内容，返回列表,并限制返回哪些字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="sortstr"></param>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public List<T> GetbySql<T>(string sql, string sortstr, string returnFields)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<T> data = conn.GetByWhere<T>(where: sql, orderBy: sortstr, returnFields: returnFields).ToList();

                return data;
            }
        }

        /// <summary>
        /// 找到所有TypeId等于参数的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_type_id"></param>
        /// <returns></returns>
        public List<T> GetByTypeId<T>(string _type_id)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByTypeId<T>(_type_id);
                List<T> data = conn.GetByWhere<T>(where: sql).ToList();

                return data;
            }
        }

        /// <summary>
        /// 找到所有TypeId等于参数的数据,动态解析type_id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="_type"></param>
        /// <returns></returns>
        public List<T1> GetByType<T, T1>(T _type)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByType(_type);
                List<T1> data = conn.GetByWhere<T1>(where: sql).ToList();

                return data;
            }
        }


        /// <summary>
        /// 找到所有parent_name = 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent_name"></param>
        /// <returns></returns>
        public List<T> GetAllByParentName<T>(string parent_name)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByTypeParentName<T>(parent_name);
                List<T> data = conn.GetByWhere<T>(where: sql).ToList();

                return data;
            }
        }

        /// <summary>
        /// 找到所有parent_name = 参数.动态解析父名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<T> GetAllByParentName<T>(T obj)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByTypeParentName(obj);
                List<T> data = conn.GetByWhere<T>(where: sql).ToList();
                return data;
            }
        }

        /// <summary>
        /// 找到所有parent_id = 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent_id"></param>
        /// <param name="tran"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public List<T> GetAllByParentId<T>(string parent_id, IDbTransaction tran = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();

            string sql = _sqlHelper.GetByParentId<T>(parent_id);
            List<T> data = conn.GetByWhere<T>(where: sql, tran: tran).ToList();

            if (dbConnection == null)
            {
                conn.Close();
            }

            return data;

        }

        /// <summary>
        /// 找到所有parent_id = 参数.动态解析id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<T> GetAllByParent<T>(T obj)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                string sql = _sqlHelper.GetByParent(obj);
                List<T> data = conn.GetByWhere<T>(where: sql).ToList();
                return data;
            }
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
                            {
                                temp2.children = new();
                            }

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
        /// 一个属性 = 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field1"></param>
        /// <param name="Field1_value"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public List<T> GetByOneFeildsSql<T>(string Field1, object Field1_value, string returnFields = "*", IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();

            List<T> data = conn.GetByOneFeildsSql<T>(Field1, Field1_value, returnFields, tran, commandTimeout).ToList();
            if (dbConnection == null)
            {
                conn.Close();
            }

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
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            List<T> data = conn.GetByTwoFeildsSql<T>(Field1, Field1_value, Field2, Field2_value, returnFields, tran, commandTimeout).ToList();
            if (dbConnection == null)
            {
                conn.Close();
            }

            return data;
        }

        /// <summary>
        /// 三个属性=参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field1"></param>
        /// <param name="Field1_value"></param>
        /// <param name="Field2"></param>
        /// <param name="Field2_value"></param>
        /// <param name="Field3"></param>
        /// <param name="Field3_value"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public List<T> GetByThreeFeildsSql<T>(string Field1, string Field1_value, string Field2, List<string> Field2_value, string Field3, List<string> Field3_value, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();

            List<T> data = conn.GetByThreeFeildsSql<T>(Field1, Field1_value, Field2, Field2_value, Field3, Field3_value, returnFields, tran, commandTimeout).ToList();
            if (dbConnection == null)
            {
                conn.Close();
            }

            return data;
        }

        /// <summary>
        /// 获取一个数据库下所有拥有同一字段的表名
        /// </summary>
        /// <param name="Field">字段名</param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public List<string> GetAllChartNameHavingSameField(string Field, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            string sql = $"select object_name(id) as objName,Name as colName from syscolumns " +
                $"where (name = '{Field}') and id in (select id from sysobjects where xtype = 'u') order by objname";

            List<string> chartNames = conn.Query<string>(sql, transaction: tran, buffered: true, commandTimeout: commandTimeout).ToList();

            if (dbConnection == null)
            {
                conn.Close();
            }
            return chartNames;
        }

        /// <summary>
        /// Type类获取类型方法(通过字符串型的类名)
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Type typen(string typeName)
        {
            Type type = null;
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            //找assembly下有无同名Type
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);//用全名判断
                if (type != null)
                {
                    return type;
                }
            }
            //找assembly下有无同名Type
            for (int i = 0; (i < assemblyArrayLength); ++i)
            {
                Type[] typeArray = assemblyArray[i].GetTypes();
                int typeArrayLength = typeArray.Length;
                for (int j = 0; j < typeArrayLength; ++j)
                {
                    if (typeArray[j].Name.Equals(typeName))//用简称Name判断
                    {
                        return typeArray[j];
                    }
                }
            }
            return type;
        }


        /// <summary>
        /// 查找枝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <param name="rootAndBranch"></param>
        /// <returns></returns>
        public void SearchBranch<T>(T _parent, ref List<T> rootAndBranch)
        {
            List<T> branch = GetAllByParent(_parent);

            if (branch != null)
            {
                foreach (T temp in branch)
                {
                    rootAndBranch.Add(temp);
                    SearchBranch(temp, ref rootAndBranch);
                }
            }

        }

        #region 为树的枝和叶分别是两种类型做的封装
        /// <summary>
        /// 树状查询,查找所有parentid = 参数
        /// 返回1+all
        /// 前序遍历
        /// </summary>
        /// <returns>无值返回fault，是空不是Null</returns>
        public List<T> GetRootAndBranch<T>(string _parentid, int rootneed = 1)
        {
            List<T> rootAndBranch = new();
            List<T> root;
            if (string.IsNullOrEmpty(_parentid))
            {
                //查找父名为空的根
                root = GetAllByParentId<T>(null);
            }
            else
            {
                T temp = GetById<T>(_parentid);
                if (temp != null && rootneed == 1)
                {
                    //首先把根添加
                    rootAndBranch.Add(temp);
                }
                //查找根下面的枝
                root = GetAllByParentId<T>(_parentid);
            }

            if (root.Count != 0)
            {
                //遍历枝
                foreach (T r in root)
                {
                    rootAndBranch.Add(r);//添加枝
                    SearchBranch(r, ref rootAndBranch);//查找下级枝
                }
            }
            return rootAndBranch;
        }
        #endregion


    }

}
