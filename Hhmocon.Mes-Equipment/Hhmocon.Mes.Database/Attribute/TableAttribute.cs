using System;

namespace Hhmocon.Mes.DataBase
{
    /// <summary>
    /// TableName：Table name，KeyName：Key，IsIdentity：Identiy, SequenceName : Sequence
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string TableName { get; set; }
        /// <summary>
        /// 主键的名称
        /// </summary>
        public string KeyName { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// 第二个Key的名字  
        /// </summary>
        public string SecondKeyName { get; set; }
        public bool IsIdentity { get; set; }
        public string SequenceName { get; set; }
    }
}
