using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util.AutofacManager;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Repository.Repository
{
    public class BaseMaterialRepository : IBaseMaterialRepository, IDependency
    {
        //SqlAssemble _sqlAssemble;
        //public BaseMaterialRepository(SqlAssemble sqlAssemble)
        //{
        //    _sqlAssemble = sqlAssemble;
        //}

        /// <summary>
        /// 根据物料类型Id查找全部物料,根据material_id排序
        /// </summary>
        /// <param name="material_type_id"></param>
        /// <returns></returns>
        public List<base_material> GetbyMaterialTypeId(string material_type_Id)
        {
            string sql = string.Format("material_type_Id = '{0}'", material_type_Id);
            string sortstr = "order by material_id";

            //假查询条件装配
            //sql = _sqlAssemble.Delete_Mark(sql);

            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<base_material> data = conn.GetByWhere<base_material>(where: sql, orderBy: sortstr).ToList();

                return data;
            }
        }




        /// <summary>
        /// 插入表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Insert(base_material obj)
        {
            //取ID 
            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                int iret = conn.Insert(obj);
                return iret > 0;
            }
        }



        /// <summary>
        /// 根据物料类型code查找全部物料,根据material_code排序
        /// </summary>
        /// <param name="material_type_id"></param>
        /// <returns></returns>
        public List<base_material> GetbyMaterialTypeCodes(string material_type_code)
        {

            string sql = string.Format("WHERE [material_type_code] = '{0}' AND [delete_mark]=0", material_type_code);

            string sortstr = "ORDER BY  [material_type_code]";

            //假查询条件装配
            //sql = _sqlAssemble.Delete_Mark(sql);

            using (System.Data.IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                List<base_material> data = conn.GetByWhere<base_material>(where: sql, orderBy: sortstr).ToList();

                return data;
            }
        }






    }
}
