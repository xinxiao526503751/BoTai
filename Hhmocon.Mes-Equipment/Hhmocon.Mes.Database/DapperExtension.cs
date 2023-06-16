using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System;
using System.Reflection;
using Hhmocon.Mes.Util.String;

namespace Hhmocon.Mes.DataBase
{
    public static partial class DapperExtension
    {

        #region common method for ado.net

        /// <summary>
        /// 根据传入的sql语句和参数获取数据库表
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(this IDbConnection conn, string sql, object param = null, IDbTransaction tran = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            
            using (IDataReader reader = conn.ExecuteReader(sql, param, tran, commandTimeout, commandType))
            {
                DataTable dt = new DataTable();
                
                //用IDataReader填充DataTable所需的数据
                dt.Load(reader);
                return dt;
            }
        }

   

        #endregion

        #region method (Insert Update Delete)

        public static int Insert<T>(this IDbConnection conn, T model, IDbTransaction tran = null, int? commandTimeout = null)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var builder = BuilderFactory.GetBuilder(conn);
                string sql = builder.GetInsertSql<T>();
                return conn.Execute(sql, model, tran, commandTimeout);
            }
            catch (Exception)
            {
                throw;
            }
   
        }

  
        /// <summary>
        /// 更新表，参数只能为整个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="model"></param>
        /// <param name="updateFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int Update<T>(this IDbConnection conn, T model, string updateFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetUpdateSql<T>(updateFields);
            return conn.Execute(sql, model, tran, commandTimeout);
        }

        public static int UpdateTwoKey<T>(this IDbConnection conn, T model, string updateFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Execute(builder.GetUpdateSqlTwoKey<T>(updateFields), model, tran, commandTimeout);
        }

        

        public static int UpdateByWhere<T>(this IDbConnection conn, T model, string where, string updateFields, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Execute(builder.GetUpdateByWhereSql<T>(where, updateFields), model, tran, commandTimeout);
        }

        public static int InsertOrUpdate<T>(this IDbConnection conn, T model, string updateFields = null, bool update = true, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            int effectRow = 0;
            dynamic total = conn.ExecuteScalar<dynamic>(builder.GetExistsKeySql<T>(), model, tran, commandTimeout);
            if (total > 0)
            {
                if (update)
                {
                    effectRow += Update(conn, model, updateFields, tran, commandTimeout);
                }
            }
            else
            {
                effectRow += Insert(conn, model, tran, commandTimeout);
            }

            return effectRow;
        }


        

        /// <summary>
        /// 对于双Key  的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="model"></param>
        /// <param name="updateFields"></param>
        /// <param name="update"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int InsertOrUpdateByTwoKey<T>(this IDbConnection conn, T model, string updateFields = null, bool update = true, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            int effectRow = 0;
            dynamic total = conn.ExecuteScalar<dynamic>(builder.GetExistsTwoKeySql<T>(), model, tran, commandTimeout);
            if (total > 0)
            {
                if (update)
                {
                    effectRow += UpdateTwoKey(conn, model, updateFields, tran, commandTimeout);
                }
            }
            else
            {
                effectRow += Insert(conn, model, tran, commandTimeout);
            }

            return effectRow;
        }

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="id"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int Delete<T>(this IDbConnection conn, object id, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Execute(builder.GetDeleteByIdSql<T>(), new { id }, tran, commandTimeout);
        }

        /// <summary>
        /// 通过Id删除对象，真删除,可批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="ids"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int DeleteByIds<T>(this IDbConnection conn, object ids, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            if (CommonUtil.ObjectIsEmpty(ids))
                return 0;
            var builder = BuilderFactory.GetBuilder(conn);
            DynamicParameters dpar = new DynamicParameters();

            //添加参数到动态参数列表
            dpar.Add("@ids", ids);
            string sql = builder.GetDeleteByIdsSql<T>();

            return conn.Execute(sql, dpar, tran, commandTimeout);
        }

    /// <summary>
    /// 通过条件删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="conn"></param>
    /// <param name="where"></param>
    /// <param name="param"></param>
    /// <param name="tran"></param>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    public static int DeleteByWhere<T>(this IDbConnection conn, string where, object param, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Execute(builder.GetDeleteByWhereSql<T>(where), param, tran, commandTimeout);
        }

        /// <summary>
        /// 通过 属性 = 参数 真删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="where"></param>
        /// <param name="param"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int DeleteByOneField<T>(this IDbConnection conn, string Field, object Field_value, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetDeleteByOneField<T>(Field);
            sql = DeleteMark(sql);
            return conn.Execute(sql, new { Field = Field_value }, tran, commandTimeout) ;
        }

        /// <summary>
        /// 通过两个属性 = 参数真删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="Field1"></param>
        /// <param name="Field1_value"></param>
        /// <param name="Field2"></param>
        /// <param name="Field_value2"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int DeleteByTwoField<T>(this IDbConnection conn, string Field1, string[] Field1_value, string Field2,string Field_value2, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetDeleteByTwoField<T>(Field1, Field2);
            sql = DeleteMark(sql);
            return conn.Execute(sql, new { Field1 = Field1_value ,Field2 = Field_value2 }, tran, commandTimeout);
        }


        public static int DeleteAll<T>(this IDbConnection conn, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Execute(builder.GetDeleteAllSql<T>(), null, tran, commandTimeout);

        }

        #endregion

        #region method (Query)


        public static long GetTotal<T>(this IDbConnection conn, string where = null, object param = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.ExecuteScalar<long>(builder.GetTotalSql<T>(where), param, tran, commandTimeout);
        }
        public static long GetTotalQuery<T>(this IDbConnection conn, string query, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.ExecuteScalar<long>(builder.GetTotalQuerySql<T>(query), null, tran, commandTimeout);
        }

        public static IEnumerable<T> GetAll<T>(this IDbConnection conn, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetAllSqlWithout<T>(returnFields, orderBy);
            return conn.Query<T>(sql, null, tran, true, commandTimeout);
        }
        /// <summary>
        /// 没有假删除功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="returnFields"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetAll2<T>(this IDbConnection conn, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetAllSql<T>(returnFields, orderBy);
            return conn.Query<T>(sql, null, tran, true, commandTimeout);
        }
        public static IEnumerable<dynamic> GetAllDynamic<T>(this IDbConnection conn, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Query(builder.GetAllSql<T>(returnFields, orderBy), null, tran, true, commandTimeout);
        }

        /// <summary>
        /// 扩展IDbConnection接口下的方法
        /// 根据id查找单个，只获取delete_mark！=0的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="id"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static T GetById<T>(this IDbConnection conn, object id, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            //如果数据库状态是关闭，就打开数据库
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            //根据上下文的类型获取不同的sql语句生成器
            var builder = BuilderFactory.GetBuilder(conn);
            string getByIdSql = builder.GetByIdSql<T>(returnFields);

            getByIdSql = getByIdSql + " AND DELETE_MARK = '0'";

            //传入sql语句和参数，执行
            return conn.QueryFirstOrDefault<T>(getByIdSql, new { id = id }, tran, commandTimeout);
        }


        /// <summary>
        /// 根据Code查找单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="code"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static T GetByCode<T>(this IDbConnection conn, object code, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            //如果数据库状态是关闭，就打开数据库
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            //根据上下文的类型获取不同的sql语句生成器
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetByCodeSql<T>(returnFields);
            sql += " and delete_mark = '0'";
            return conn.QueryFirstOrDefault<T>(sql, new { code = code }, tran, commandTimeout);
        }


        public static T GetByTwoId<T>(this IDbConnection conn, object id,object secondid, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {

            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.QueryFirstOrDefault<T>(builder.GetByTwoIdSql<T>(returnFields), new { id = id, secondid = secondid },  tran, commandTimeout); ;
        }

        /// <summary>
        /// 一个属性 = 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="Field1"></param>
        /// <param name="Field2"></param>
        /// <param name="Field1_value"></param>
        /// <param name="Field2_value"></param>
        /// <param name="returnFields">返回的字段，默认为*</param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static List<T> GetByOneFeildsSql<T>(this IDbConnection conn, string Field,object Field_value,string returnFields = "*", IDbTransaction tran = null, int? commandTimeout = 5)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetByOneFeildSql<T>(Field, returnFields);
            DynamicParameters dpar = new();
            sql += "AND DELETE_MARK = '0'";
            dpar.Add("@Field", Field_value);
            return conn.Query<T>(sql, dpar, tran,true,commandTimeout).ToList();
        }

        /// <summary>
        /// 一个属性 in 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="Field"></param>
        /// <param name="Field_value"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static List<T> GetByOneFeildSqlIn<T>(this IDbConnection conn, string Field, object Field_value, string returnFields = "*", IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetByOneFeildSqlIn<T>(Field, returnFields);
            DynamicParameters dpar = new();
            sql += "AND DELETE_MARK = '0'";
            dpar.Add("@Field", Field_value);
            return conn.Query<T>(sql, dpar, tran, true, commandTimeout).ToList();
        }

        /// <summary>
        /// 追加and delete_mark = '0'
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public static string DeleteMark(string whereStr)
        {
            whereStr += " AND DELETE_MARK = '0'";
            return whereStr;
        }


   
        /// <summary>
        /// 两个属性=参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="id"></param>
        /// <param name="secondid"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetByTwoFeildsSql<T>(this IDbConnection conn,string Field1,string Field1_value,string Field2, List<string> Field2_value, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {

            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            DynamicParameters dpar = new DynamicParameters();
            dpar.Add("@Field1", Field1_value);
            dpar.Add("@Field2", Field2_value);
            string sql = builder.GetByTwoFeildsSql<T>(Field1, Field2, returnFields);
            sql = DeleteMark(sql);
            return conn.Query<T>(sql, dpar, transaction: tran);
        }

        /// <summary>
        /// 三个属性=参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="Field1"></param>
        /// <param name="Field1_value"></param>
        /// <param name="Field2"></param>
        /// <param name="Field2_value"></param>
        /// <param name="Field3"></param>
        /// <param name="Field3_value"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetByThreeFeildsSql<T>(this IDbConnection conn, string Field1, string Field1_value, string Field2, List<string> Field2_value,string Field3,List<string> Field3_value, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {

            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            DynamicParameters dpar = new DynamicParameters();
            dpar.Add("@Field1", Field1_value);
            dpar.Add("@Field2", Field2_value);
            dpar.Add("@Field3", Field3_value);
            string sql = builder.GetByThreeFeildsSql<T>(Field1, Field2,Field3, returnFields);
            sql = DeleteMark(sql);
            return conn.Query<T>(sql, dpar);
        }

        public static dynamic GetByIdDynamic<T>(this IDbConnection conn, object id, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {

            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.QueryFirstOrDefault(builder.GetByIdSql<T>(returnFields), new { id = id }, tran, commandTimeout);
        }

        /// <summary>
        /// 根据ids查找全部，假查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="ids"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetByIds<T>(this IDbConnection conn, object ids, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            if (CommonUtil.ObjectIsEmpty(ids))
            return Enumerable.Empty<T>();
            var builder = BuilderFactory.GetBuilder(conn);
            DynamicParameters dpar = new DynamicParameters();
            dpar.Add("@ids", ids);
            string sql = builder.GetByIdsSql<T>(returnFields);

            return conn.Query<T>(sql, dpar, tran, true, commandTimeout);
        }





        public static IEnumerable<dynamic> GetByIdsDynamic<T>(this IDbConnection conn, object ids, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            if (CommonUtil.ObjectIsEmpty(ids))
                return Enumerable.Empty<dynamic>();
            var builder = BuilderFactory.GetBuilder(conn);
            DynamicParameters dpar = new DynamicParameters();
            dpar.Add("@ids", ids);
            return conn.Query(builder.GetByIdsSql<T>(returnFields), dpar, tran, true, commandTimeout);
        }

        /// <summary>
        /// 通过多个id查找,可选择返回的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="ids"></param>
        /// <param name="field"></param>
        /// <param name="returnFields"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetByIdsWithField<T>(this IDbConnection conn,string field,object ids, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            DynamicParameters dpar = new DynamicParameters();
            dpar.Add("@ids", ids);
            string sql = builder.GetByIdsWithFieldSql<T>(field, returnFields);
            sql = DeleteMark(sql);
            return conn.Query<T>(sql, dpar, tran, true, commandTimeout);
        }

        public static IEnumerable<dynamic> GetByIdsWithFieldDynamic<T>(this IDbConnection conn, object ids, string field, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            if (CommonUtil.ObjectIsEmpty(ids))
                return Enumerable.Empty<dynamic>();
            var builder = BuilderFactory.GetBuilder(conn);
            DynamicParameters dpar = new DynamicParameters();
            dpar.Add("@ids", ids);
            return conn.Query(builder.GetByIdsWithFieldSql<T>(field, returnFields), dpar, tran, true, commandTimeout);
        }

        /// <summary>
        /// 根据Where查询 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="where"></param>
        /// <param name="param"></param>
        /// <param name="returnFields"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns>注意返回的是new()过后的内容，不会有Null</returns>
        public static IEnumerable<T> GetByWhere<T>(this IDbConnection conn, string where, object param = null, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetByWhereSql<T>(where, returnFields, orderBy);

            return conn.Query<T>(sql, param, tran, true, commandTimeout);
        }

        public static IEnumerable<dynamic> GetByWhereDynamic<T>(this IDbConnection conn, string where, object param = null, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Query(builder.GetByWhereSql<T>(where, returnFields, orderBy), param, tran, true, commandTimeout);
        }

        public static T GetByWhereFirst<T>(this IDbConnection conn, string where, object param = null, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetByWhereFirstSql<T>(where, returnFields);
            return conn.QueryFirstOrDefault<T>(sql, param, tran, commandTimeout);
        }

        public static dynamic GetByWhereFirstDynamic<T>(this IDbConnection conn, string where, object param = null, string returnFields = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.QueryFirstOrDefault(builder.GetByWhereFirstSql<T>(where, returnFields), param, tran, commandTimeout);
        }

        public static IEnumerable<T> GetBySkipTake<T>(this IDbConnection conn, int skip, int take, string where = null, object param = null, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Query<T>(builder.GetBySkipTakeSql<T>(skip, take, where, returnFields, orderBy), param, tran, true, commandTimeout);
        }

        public static IEnumerable<dynamic> GetBySkipTakeDynamic<T>(this IDbConnection conn, int skip, int take, string where = null, object param = null, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Query(builder.GetBySkipTakeSql<T>(skip, take, where, returnFields, orderBy), param, tran, true, commandTimeout);
        }

        /// <summary>
        /// 通过分页参数查找数据.没有则返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <param name="param"></param>
        /// <param name="returnFields"></param>
        /// <param name="orderBy"></param>
        /// <param name="tran"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetByPageIndex<T>(this IDbConnection conn, int pageIndex, int pageSize, string where = null, object param = null, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetByPageIndexSql<T>(pageIndex, pageSize, where, returnFields, orderBy);

            return conn.Query<T>(sql, param, tran, true, commandTimeout);
        }

        public static IEnumerable<dynamic> GetByPageIndexDynamic<T>(this IDbConnection conn, int pageIndex, int pageSize, string where = null, object param = null, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            string sql = builder.GetByPageIndexSql<T>(pageIndex, pageSize, where, returnFields, orderBy);
            return conn.Query(sql, param, tran, true, commandTimeout);
        }

        public static IEnumerable<T> GetByPageIndex<T>(this IDbConnection conn, string query, int pageIndex, int pageSize, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var builder = BuilderFactory.GetBuilder(conn);
            return conn.Query<T>(builder.GetByPageIndexSql<T>(query, pageIndex, pageSize), null, tran, true, commandTimeout);
        }

        public static PageEntity<dynamic> GetPageDynamic<T>(this IDbConnection conn, int pageIndex, int pageSize, string where = null, object param = null, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            PageEntity<dynamic> pageEntity = new PageEntity<dynamic>();
            pageEntity.Total = GetTotal<T>(conn, where, param, tran, commandTimeout);
            if (pageEntity.Total > 0)
                pageEntity.Data = GetByPageIndexDynamic<T>(conn, pageIndex, pageSize, where, param, returnFields, orderBy, tran, commandTimeout);
            else
                pageEntity.Data = Enumerable.Empty<dynamic>();
            return pageEntity;
        }

        public static PageEntity<T> GetPage<T>(this IDbConnection conn, int pageIndex, int pageSize, string where = null, object param = null, string returnFields = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null)
        {
            PageEntity<T> pageEntity = new PageEntity<T>();
            pageEntity.Total = GetTotal<T>(conn, where, param, tran, commandTimeout);
            if (pageEntity.Total > 0)
                pageEntity.Data = GetByPageIndex<T>(conn, pageIndex, pageSize, where, param, returnFields, orderBy, tran, commandTimeout);
            else
                pageEntity.Data = Enumerable.Empty<T>();
            return pageEntity;
        }

        /* created by @samuelrvg - 13/04/2020 */
        public static dynamic GetPage<T>(this IDbConnection conn, string query, int pageIndex, int pageSize, IDbTransaction tran = null, int? commandTimeout = null)
        {
            PageEntity<T> pageEntity = new PageEntity<T>();
            pageEntity.Total = GetTotalQuery<T>(conn, query, tran, commandTimeout);
            if (pageEntity.Total > 0)
                pageEntity.Data = GetByPageIndex<T>(conn, query, pageIndex, pageSize, tran, commandTimeout);
            else
                pageEntity.Data = Enumerable.Empty<T>();
            return pageEntity;
        }

        #endregion
    }
}
