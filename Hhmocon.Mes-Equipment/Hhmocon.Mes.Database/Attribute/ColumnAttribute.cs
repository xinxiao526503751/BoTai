using System;

namespace Hhmocon.Mes.DataBase
{
    /// <summary>
    /// Table column map
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
