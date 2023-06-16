
namespace Hhmocon.Mes.DataBase
{
    /// <summary>
    /// sql语句生成器
    /// </summary>
    internal interface ISqlBuilder
    {

        #region method (Insert Update Delete)


        string GetInsertSql<T>();

        string GetInsertReturnIdSql<T>(string sequence = null);

        string GetUpdateSql<T>(string updateFields);


        string GetUpdateSqlTwoKey<T>(string updateFields);

        string GetUpdateByWhereSql<T>(string where, string updateFields);

        string GetExistsKeySql<T>();

        string GetExistsTwoKeySql<T>();

        string GetDeleteByIdSql<T>();

        string GetDeleteByIdsSql<T>();

        string GetDeleteByWhereSql<T>(string where);

        string GetDeleteAllSql<T>();

        /// <summary>
        /// 通过一个属性删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field"></param>
        /// <returns></returns>
        string GetDeleteByOneField<T>(string Field);

        /// <summary>
        /// 通过两个属性删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field1"></param>
        /// <param name="Field2"></param>
        /// <returns></returns>
        string GetDeleteByTwoField<T>(string Field1, string Field2);
        #endregion


        #region method (Query)



        string GetTotalSql<T>(string where);

        string GetTotalQuerySql<T>(string query);

        string GetAllSql<T>(string returnFields, string orderBy);
        string GetAllSqlWithout<T>(string returnFields, string orderBy);

        string GetByIdSql<T>(string returnFields);
        string GetByCodeSql<T>(string returnFields);
        /// <summary>
        /// 根据双ID 返回数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        string GetByTwoIdSql<T>(string returnFields);

        string GetByIdsSql<T>(string returnFields);

        /// <summary>
        /// 一个属性= 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field"></param>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        string GetByOneFeildSql<T>(string Field, string returnFields);

        /// <summary>
        /// 一个属性 in 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field"></param>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        public string GetByOneFeildSqlIn<T>(string Field, string returnFields);

        /// <summary>
        /// 两个属性= 参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Fields1"></param>
        /// <param name="Fields2"></param>
        /// <param name="returnFields"></param>
        /// <returns></returns>
        string GetByTwoFeildsSql<T>(string Field1, string Field2, string returnFields);

        string GetByThreeFeildsSql<T>(string Field1, string Field2, string Field3, string returnFields);

        string GetByIdsWithFieldSql<T>(string field, string returnFields);

        string GetByWhereSql<T>(string where, string returnFields, string orderBy);

        string GetByWhereFirstSql<T>(string where, string returnFields);

        string GetBySkipTakeSql<T>(int skip, int take, string where, string returnFields, string orderBy);

        string GetByPageIndexSql<T>(int pageIndex, int pageSize, string where, string returnFields, string orderBy);

        string GetByPageIndexSql<T>(string query, int pageIndex, int pageSize);

        #endregion


    }
}
