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

using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;
using System.Data;

namespace Hhmocon.Mes.Repository
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class SysUserRepository : ISysUserRepository, IDependency
    {
        /// <summary>
        /// 根据Name获取.无数据返回null
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public List<sys_user> GetByName(string name, IDbTransaction tran = null, int? commandTimeout = null, IDbConnection dbConnection = null)
        {
            IDbConnection conn = dbConnection ?? SqlServerDbHelper.GetConn();
            List<sys_user> data = conn.GetByOneFeildsSql<sys_user>("user_name", name, "*", tran, commandTimeout);
            if (data.Count == 0)
            {
                return null;
            }

            return data;
        }

        /// <summary>
        /// 将sys_user列表转化成Node
        /// parent_id=Dept_id;
        /// 用来在生成树时和部门Nodes衔接
        /// </summary>
        /// <returns></returns>
        public List<TreeModel> ListElementToNodeLinkWithDept(List<sys_user> list)
        {
            List<TreeModel> treeModels = new();
            foreach (sys_user temp in list)
            {
                TreeModel treeModel = new();

                treeModel.id = temp.user_id;
                treeModel.label = temp.user_name;
                treeModel.parentId = temp.dept_id;//用户没有父子关系，用户衔接部门
                treeModel.NodeType = "user";

                treeModels.Add(treeModel);
            }
            return treeModels;
        }



    }
}
