using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hhmocon.Mes.DataBase
{
    internal class CommonUtil
    {
        public static string GetFieldsStr(IEnumerable<string> fieldList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in fieldList)
            {
                sb.AppendFormat("{0}", item);

                if (item != fieldList.Last())
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        public static string GetFieldsAtStr(IEnumerable<string> fieldList, string symbol = "@")
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in fieldList)
            {
                sb.AppendFormat("{0}{1}", symbol, item);

                if (item != fieldList.Last())
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        public static string GetFieldsEqStr(IEnumerable<string> fieldList, string symbol = "@")
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in fieldList)
            {
                sb.AppendFormat("{0}={1}{0}", item, symbol);

                if (item != fieldList.Last())
                {
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// param只能是ienumerable且不能是string不能是字典
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IEnumerable GetMultiExec(object param)
        {
            return (param is IEnumerable && !(param is string || param is IEnumerable<KeyValuePair<string, object>>)) ? (IEnumerable)param : null;
        }

        public static bool ObjectIsEmpty(object param)
        {
            bool result = true;
            IEnumerable data = GetMultiExec(param);
            if (data != null)
            {
                foreach (object item in data)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 创建数据库实体
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TableEntity CreateTableEntity(Type t)
        {
            //返回该类自定义属性的数组，不搜索此成员的继承链，
            //取序列中满足条件的第一个元素
            TableAttribute table = t.GetCustomAttributes(false).FirstOrDefault(f => f is TableAttribute) as TableAttribute;
            //如果找不到
            if (table == null)
            {
                //抛出异常，类没有特性[TableAttribute],请先打上标签
                throw new Exception("Class " + t.Name + " is not labeled [TableAttribute], please label it first");
            }
            //如果找到了
            else
            {
                //用一个数据库实体类来取它标签的各项属性
                TableEntity model = new TableEntity
                {
                    TableName = table.TableName,
                    KeyName = table.KeyName,
                    Code = table.Code,
                    SecondKeyName = table.SecondKeyName,
                    IsIdentity = table.IsIdentity,
                    SequenceName = table.SequenceName,
                    AllFieldList = new List<string>(),
                    ExceptKeyFieldList = new List<string>()
                };

                //获取type的所有公共属性
                System.Reflection.PropertyInfo[] allproperties = t.GetProperties();

                //遍历Type的属性，获取【Type的属性】的自定义属性的数组，查找各种标签特性
                foreach (System.Reflection.PropertyInfo item in allproperties)
                {
                    IgoreAttribute igore = item.GetCustomAttributes(false).FirstOrDefault(f => f is IgoreAttribute) as IgoreAttribute;
                    if (igore == null)
                    {
                        string Name = item.Name;
                        ColumnAttribute column = item.GetCustomAttributes(false).FirstOrDefault(f => f is ColumnAttribute) as ColumnAttribute;
                        if (column != null)
                        {
                            Name = column.Name;
                            model.AllFieldList.Add(Name);
                        }
                        else
                        {
                            model.AllFieldList.Add(Name.ToUpper());
                        }

                        if (Name.ToLower().Equals(model.KeyName.ToLower()))
                        {
                            model.KeyType = item.PropertyType;
                        }
                        else
                        {
                            model.ExceptKeyFieldList.Add(Name);
                        }
                    }
                }

                //返回数据库实体
                return model;
            }

        }

        /// <summary>
        /// 拼接sql语句
        /// </summary>
        /// <param name="table"></param>
        public static void InitTableForOracle(TableEntity table)
        {
            string Fields = GetFieldsStr(table.AllFieldList);
            string FieldsAt = GetFieldsAtStr(table.AllFieldList, "@");
            string FieldsEq = GetFieldsEqStr(table.AllFieldList, "@");

            string FieldsExtKey = GetFieldsStr(table.ExceptKeyFieldList);
            string FieldsAtExtKey = GetFieldsAtStr(table.ExceptKeyFieldList, "@");
            string FieldsEqExtKey = GetFieldsEqStr(table.ExceptKeyFieldList, "@");

            table.AllFields = Fields;
            table.AllFieldsAt = FieldsAt;
            table.AllFieldsAtEq = FieldsEq;

            table.AllFieldsExceptKey = FieldsExtKey;
            table.AllFieldsAtExceptKey = FieldsAtExtKey;
            table.AllFieldsAtEqExceptKey = FieldsEqExtKey;

            if (!string.IsNullOrEmpty(table.KeyName))
            {
                table.InsertReturnIdSql = string.Format("INSERT INTO {0}({1})VALUES(```seq```.NEXTVAL,{2})", table.TableName, Fields, FieldsAtExtKey);
                if (!string.IsNullOrEmpty(table.SequenceName))
                {
                    table.InsertSql = string.Format("INSERT INTO {0}({1})VALUES({2}.NEXTVAL, {3})", table.TableName, Fields, table.SequenceName, FieldsAtExtKey);
                }
                if (table.IsIdentity)
                {
                    table.InsertSql = string.Format("INSERT INTO {0}({1})VALUES({2})", table.TableName, FieldsExtKey, FieldsAtExtKey);
                }
                else
                {
                    table.InsertSql = string.Format("INSERT INTO {0}({1})VALUES({2})", table.TableName, Fields, FieldsAt);
                }

                table.DeleteByIdSql = string.Format("DELETE FROM {0} WHERE {1}=@id", table.TableName, table.KeyName);
                table.DeleteByIdsSql = string.Format("DELETE FROM {0} WHERE {1} IN @ids", table.TableName, table.KeyName);

                table.GetByIdSql = string.Format("SELECT {0} FROM {1} WHERE {2}=@id", Fields, table.TableName, table.KeyName);
                table.GetByIdsSql = string.Format("SELECT {0} FROM {1} WHERE {2} IN @ids", Fields, table.TableName, table.KeyName);
                table.GetByCodeSql = string.Format("SELECT {0} FROM {1} WHERE {2} =@code", Fields, table.TableName, table.Code);
                table.UpdateSql = string.Format("UPDATE {0} SET {1} WHERE {2}=@{2}", table.TableName, FieldsEqExtKey, table.KeyName);

            }
            else
            {
                //对于不存在Key的表 则直接插入
                table.InsertSql = string.Format("INSERT INTO {0}({1})VALUES({2})", table.TableName, Fields, FieldsAt);
            }

            table.DeleteAllSql = string.Format("DELETE FROM {0} ", table.TableName);
            table.GetAllSql = string.Format("SELECT {0} FROM {1} ", Fields, table.TableName);
        }

        /// <summary>
        /// 检查表实体类的KeyName属性是否为空，为空则报错
        /// </summary>
        /// <param name="table"></param>
        public static void CheckTableKey(TableEntity table)
        {
            if (string.IsNullOrEmpty(table.KeyName))
            {
                string msg = "table [" + table.TableName + "] has no primary key";
                throw new Exception(msg);
            }

        }

        public static string CreateUpdateSql(TableEntity table, string updateFields, string symbol = "@")
        {
            string updateList = GetFieldsEqStr(updateFields.Split(',').ToList(), symbol);
            return string.Format("UPDATE {0} SET {1} WHERE {2}={3}{2}", table.TableName, updateList, table.KeyName, symbol);
        }

        public static string CreateUpdateTwoKeySql(TableEntity table, string updateFields, string symbol = "@")
        {
            string updateList = GetFieldsEqStr(updateFields.Split(',').ToList(), symbol);
            return string.Format("UPDATE {0} SET {1} WHERE {2}={3}{2} AND {4}={3}{4}", table.TableName, updateList, table.KeyName, symbol, table.SecondKeyName);
        }

        public static string CreateUpdateByWhereSql(TableEntity table, string where, string updateFields, string symbol = "@")
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("UPDATE {0} SET ", table.TableName);
            if (string.IsNullOrEmpty(updateFields))
            {
                if (!string.IsNullOrEmpty(table.KeyName))
                {
                    sb.AppendFormat(table.AllFieldsAtEqExceptKey);
                }
                else
                {
                    sb.AppendFormat(table.AllFieldsAtEq);
                }
            }
            else
            {
                string updateList = GetFieldsEqStr(updateFields.Split(',').ToList(), symbol);
                sb.Append(updateList);
            }
            sb.Append(" ");
            sb.Append(where);

            return sb.ToString();
        }

    }
}
