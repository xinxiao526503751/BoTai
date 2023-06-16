using System;
using System.Linq;
using System.Text;

namespace Hhmocon.Mes.DataBase
{
    /// <summary>
    /// sql
    /// </summary>
    internal class SqlServerBuilder : ISqlBuilder
    {

        #region common
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="table"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="where"></param>
        /// <param name="returnFields"></param>
        /// <param name="orderBy"></param>
        private static void InitPage(StringBuilder sb, TableEntity table, int skip, int take, string where, string returnFields, string orderBy)
        {
            if (string.IsNullOrEmpty(returnFields))
            {
                returnFields = table.AllFields;
            }

            if (string.IsNullOrEmpty(orderBy))
            {
                if (!string.IsNullOrEmpty(table.KeyName))
                {
                    orderBy = string.Format("ORDER BY {0}", table.KeyName);
                }
                else
                {
                    orderBy = string.Format("ORDER BY {0}", table.AllFieldList.First());
                }
            }
            //  sb.AppendFormat("SELECT * FROM  (SELECT A.*,ROWNUM RN FROM (SELECT {0} FROM {1} {2} {3}) A  WHERE ROWNUM <= {4}) WHERE RN > {5}", returnFields, table.TableName, where, orderBy, skip + take, skip);

            sb.AppendFormat("SELECT * FROM   (SELECT ROW_NUMBER() Over({3}) AS rowNum,* FROM (SELECT {0} FROM {1} {2}) AS T ) AS N WHERE rowNum>{5} AND rowNum<= {4}", returnFields, table.TableName, where, orderBy, skip + take, skip);
        }

        #endregion


        public string GetInsertSql<T>()
        {
            string strtemp = SqlServerCache.GetTableEntity<T>().InsertSql;
            return strtemp;// OracleCache.GetTableEntity<T>().InsertSql;
        }

        public string GetInsertReturnIdSql<T>(string sequence = null)
        {
            if (string.IsNullOrEmpty(sequence))
            {
                throw new Exception("oracle [sequence] can't no be null or empty");
            }

            return (SqlServerCache.GetTableEntity<T>().InsertReturnIdSql).Replace("```seq```", sequence);
        }

        public string GetUpdateSql<T>(string updateFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);
            if (string.IsNullOrEmpty(updateFields))
            {
                return table.UpdateSql;
            }
            return CommonUtil.CreateUpdateSql(table, updateFields, "@");
        }

        public string GetUpdateSqlTwoKey<T>(string updateFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);
            if (string.IsNullOrEmpty(updateFields))
            {
                return table.UpdateSql;
            }
            return CommonUtil.CreateUpdateTwoKeySql(table, updateFields, "@");
        }


        public string GetUpdateByWhereSql<T>(string where, string updateFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            return CommonUtil.CreateUpdateByWhereSql(table, where, updateFields, "@");
        }

        public string GetExistsKeySql<T>()
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);
            return string.Format("SELECT COUNT(1) FROM {0} WHERE {1}=@{1}", table.TableName, table.KeyName);
        }


        public string GetExistsTwoKeySql<T>()
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);
            return string.Format("SELECT COUNT(1) FROM {0} WHERE {1}=@{1} AND {2}=@{2}", table.TableName, table.KeyName, table.SecondKeyName);
        }

        /// <summary>
        /// 获取通过主键删除的sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetDeleteByIdSql<T>()
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);
            return table.DeleteByIdSql;
        }

        /// <summary>
        /// 获取通过主键删除的sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetDeleteByIdsSql<T>()
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);
            return table.DeleteByIdsSql;
        }

        /// <summary>
        /// 通过条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public string GetDeleteByWhereSql<T>(string where)
        {
            return GetDeleteAllSql<T>() + where;
        }

        /// <summary>
        /// 通过一个属性删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public string GetDeleteByOneField<T>(string Field)
        {
            return string.Format(GetDeleteAllSql<T>() + "where {0} = @Field ", Field);
        }

        /// <summary>
        /// 通过两个属性删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field"></param>
        /// <returns></returns>
        public string GetDeleteByTwoField<T>(string Field1, string Field2)
        {
            return string.Format(GetDeleteAllSql<T>() + "where {0} in @Field1 and {1} = @Field2", Field1, Field2);
        }

        public string GetDeleteAllSql<T>()
        {
            return SqlServerCache.GetTableEntity<T>().DeleteAllSql;
        }



        public string GetTotalSql<T>(string where)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();

            string sql = string.Format("SELECT COUNT(1) FROM {0} {1}", table.TableName, where);


            return sql;
        }

        public string GetTotalQuerySql<T>(string query)
        {
            return string.Format("SELECT COUNT(1) FROM ({0})", query);
        }

        public string GetAllSql<T>(string returnFields, string orderBy)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            if (string.IsNullOrEmpty(returnFields))
            {
                return table.GetAllSql + orderBy;
            }
            else
            {
                return string.Format("SELECT {0} FROM {1} {2}", returnFields, table.TableName, orderBy);
            }
        }

        /// <summary>
        /// 过滤假删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnFields"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public string GetAllSqlWithout<T>(string returnFields, string orderBy)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            if (string.IsNullOrEmpty(returnFields))
            {
                return table.GetAllSql + "WHERE DELETE_MARK = '0'" + orderBy;
            }
            else
            {
                return string.Format("SELECT {0} FROM {1} {2} WHERE DELETE_MARK = '0' ", returnFields, table.TableName, orderBy);
            }
        }

        /// <summary>
        /// 通过id获取的sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public string GetByIdSql<T>(string returnFields)
        {
            //根据传入的类型根据句柄从字典中获取一个表实体类
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            //检查表实体类的KeyName属性是否为空
            CommonUtil.CheckTableKey(table);

            //如果记录数returnFields为空
            if (string.IsNullOrEmpty(returnFields))
            {
                //返回通过Id获取该类型的sql语句
                return table.GetByIdSql;
            }
            else
            {
                //返回 查找KeyName=id 的sql语句
                return string.Format("SELECT {0} FROM {1} WHERE {2}=@id", returnFields, table.TableName, table.KeyName);
            }
        }


        /// <summary>
        /// 通过Code获取的sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public string GetByCodeSql<T>(string returnFields)
        {
            //根据传入的类型根据句柄从字典中获取一个表实体类
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            //检查表实体类的KeyName属性是否为空
            CommonUtil.CheckTableKey(table);

            //如果记录数returnFields为空
            if (string.IsNullOrEmpty(returnFields))
            {
                //返回通过Id获取该类型的sql语句
                return table.GetByCodeSql;
            }
            else
            {
                //返回 查找Code=code 的sql语句
                return string.Format("SELECT {0} FROM {1} WHERE {2}=@id", returnFields, table.TableName, table.Code);
            }
        }


        public string GetByTwoIdSql<T>(string returnFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);
            if (string.IsNullOrEmpty(returnFields))
            {
                return table.GetByIdSql;
            }
            else
            {
                return string.Format("SELECT {0} FROM {1} WHERE {2}=@id AND {3}=@secondid", returnFields, table.TableName, table.KeyName, table.SecondKeyName);
            }
        }

        /// <summary>
        /// 一个属性 = 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public string GetByOneFeildSql<T>(string Field, string returnFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);

            return string.Format("SELECT {0} FROM {1} WHERE {2} = @Field ", returnFields, table.TableName, Field);
        }

        /// <summary>
        /// 一个属性 in 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field"></param>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public string GetByOneFeildSqlIn<T>(string Field, string returnFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);

            return string.Format("SELECT {0} FROM {1} WHERE {2} in @Field ", returnFields, table.TableName, Field);
        }


        /// <summary>
        /// 两个属性=参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Fields"></param>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public string GetByTwoFeildsSql<T>(string Field1, string Field2, string returnFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            if (string.IsNullOrEmpty(returnFields))
            {
                returnFields = table.AllFields;
            }

            return string.Format("SELECT {0} FROM {1} WHERE {2} = @Field1 AND {3} IN @Field2", returnFields, table.TableName, Field1, Field2);
        }

        /// <summary>
        /// 三个属性=参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field1"></param>
        /// <param name="Field2"></param>
        /// <param name="Field3"></param>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public string GetByThreeFeildsSql<T>(string Field1, string Field2, string Field3, string returnFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            if (string.IsNullOrEmpty(returnFields))
            {
                returnFields = table.AllFields;
            }

            return string.Format("SELECT {0} FROM {1} WHERE {2} = @Field1 AND {3} IN @Field2 AND {4} IN @Field3", returnFields, table.TableName, Field1, Field2, Field3);
        }

        /// <summary>
        /// 获取通过id查找全部的sql语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public string GetByIdsSql<T>(string returnFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            CommonUtil.CheckTableKey(table);
            if (string.IsNullOrEmpty(returnFields))
            {
                return table.GetByIdsSql;
            }
            else
            {
                return string.Format("SELECT {0} FROM {1} WHERE {2} IN @ids AND DELETE_MARK = '0'", returnFields, table.TableName, table.KeyName);
            }
        }

        public string GetByIdsWithFieldSql<T>(string field, string returnFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            if (string.IsNullOrEmpty(returnFields))
            {
                returnFields = table.AllFields;
            }

            return string.Format("SELECT {0} FROM {1} WHERE {2} IN @ids", returnFields, table.TableName, field);
        }

        public string GetByWhereSql<T>(string where, string returnFields, string orderBy)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            if (string.IsNullOrEmpty(returnFields))
            {
                returnFields = table.AllFields;
            }

            string format = string.Format("SELECT {0} FROM {1} {2} {3}", returnFields, table.TableName, where, orderBy);

            return format;
        }

        public string GetByWhereFirstSql<T>(string where, string returnFields)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            if (string.IsNullOrEmpty(returnFields))
            {
                returnFields = table.AllFields;
            }

            if (!string.IsNullOrEmpty(where))
            {
                where += "AND rownum=1";
            }
            else
            {
                where = "WHERE rownum=1";
            }

            return string.Format("SELECT {0} FROM {1} {2}", returnFields, table.TableName, where);
        }

        public string GetBySkipTakeSql<T>(int skip, int take, string where, string returnFields, string orderBy)
        {
            TableEntity table = SqlServerCache.GetTableEntity<T>();
            StringBuilder sb = new StringBuilder();
            InitPage(sb, table, skip, take, where, returnFields, orderBy);
            return sb.ToString();
        }

        public string GetByPageIndexSql<T>(int pageIndex, int pageSize, string where, string returnFields, string orderBy)
        {
            int skip = 0;
            if (pageIndex > 0)
            {
                skip = (pageIndex - 1) * pageSize;
            }
            return GetBySkipTakeSql<T>(skip, pageSize, where, returnFields, orderBy);
        }
        public string GetByPageIndexSql<T>(string query, int pageIndex, int pageSize)
        {
            StringBuilder sb = new StringBuilder();
            int skip = 0;
            if (pageIndex > 0)
            {
                skip = (pageIndex - 1) * pageSize;
            }
            return sb.AppendFormat("SELECT * FROM(SELECT A.*,ROWNUM RN FROM ({0}) A  WHERE ROWNUM <= {1}) WHERE RN > {2}", query, skip + pageSize, skip).ToString();
        }

    }
}
