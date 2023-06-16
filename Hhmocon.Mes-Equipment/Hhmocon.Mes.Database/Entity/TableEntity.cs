using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.DataBase
{
    /// <summary>
    /// 数据库表实例
    /// </summary>
    internal class TableEntity
    {
        //表名
        public string TableName { get; set; }
        //主键
        public string KeyName { get; set; }
        //第二主键
        public string SecondKeyName { get; set; }

        //Code
        public string Code { get; set; }

        //主键类型
        public Type KeyType { get; set; }
        public bool IsIdentity { get; set; }
        public string SequenceName { get; set; }


        public List<string> AllFieldList { get; set; }
        public List<string> ExceptKeyFieldList { get; set; }

        public string AllFields { get; set; }
        public string AllFieldsAt { get; set; }
        public string AllFieldsAtEq { get; set; }

        public string AllFieldsExceptKey { get; set; }
        public string AllFieldsAtExceptKey { get; set; }
        public string AllFieldsAtEqExceptKey { get; set; }

        public string InsertSql { get; set; }
        public string InsertReturnIdSql { get; set; }
        public string GetByIdSql { get; set; }
        public string GetByIdsSql { get; set; }

        public string GetByCodeSql { get; set; }

        public string GetAllSql { get; set; }
        public string DeleteByIdSql { get; set; }
        public string DeleteByIdsSql { get; set; }
        public string DeleteAllSql { get; set; }
        public string UpdateSql { get; set; }
    }
}
