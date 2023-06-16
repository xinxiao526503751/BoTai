using Dapper;
using Hhmocon.Mes.Util;
using System;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Hhmocon.Mes.Application
{
    public class CommonHelper
    {

        // private static ISysKeyValuesRepository SysKeyValues = new SysKeyValuesRepository();

        //取运算符
        public static string GetConditoalChar(string tempchar, ref bool IsLikeIn)
        {
            string strRet = string.Empty;
            IsLikeIn = false;
            switch (tempchar)
            {
                case "eq":
                    strRet = "=";
                    break;
                case ">":
                    strRet = ">";
                    break;
                case "<":
                    strRet = "<";
                    break;
                case ">eq":
                    strRet = ">=";
                    break;
                case "<eq":
                    strRet = "<=";
                    break;
                case "neq":
                    strRet = "!=";
                    break;

                case "%":
                    strRet = "LIKE";
                    IsLikeIn = true;
                    break;
                case "l%":
                    strRet = "LLIKE";  //单独处理
                    IsLikeIn = true;
                    break;
                case "r%":
                    strRet = "RLIKE";  //单独处理
                    IsLikeIn = true;
                    break;

                case "in":
                    strRet = "IN";
                    IsLikeIn = true;
                    break;
                default:
                    strRet = "=";
                    break;

            }

            return strRet;
        }

        /// <summary>
        /// 根据入参 返回条件字符串
        /// </summary>
        /// <param name="strkey"></param>
        /// <returns></returns>
        public static string GetSqlConditonalStr(string strkey)
        {
            string retStr = string.Empty;
            StringBuilder sbCod = new StringBuilder();
            try
            {
                // 先找出有没有 &
                // 再找出有没有 # 
                string[] conArrAnd = strkey.Split('&');//&分割

                for (int i = 0; i < conArrAnd.Length; i++)//遍历
                {
                    string[] conArrOr = conArrAnd[i].Split('#');//#分割

                    if (conArrOr.Length <= 1)//如果#分割不出东西
                    {
                        // 用 or 区分
                        string[] oneCon = conArrOr[0].Split('|');//用|分割
                        //分割后长度不为2，直接跳出for循环
                        if (oneCon.Length != 2)
                        {
                            continue;
                        }
                        int iseq = oneCon[1].IndexOf('=');
                        int ilen = oneCon[1].Length;


                        sbCod.Append(oneCon[0]).Append(" ");
                        bool isLikeIn = false;
                        //取运算符
                        string tempConChar = GetConditoalChar((oneCon[1].Substring(0, iseq)).ToLower(), ref isLikeIn);
                        string tempConValue = oneCon[1].Substring(iseq + 1, (ilen - (iseq + 1)));


                        if (!isLikeIn)
                        {
                            sbCod.Append(tempConChar).Append(" ");
                            sbCod.Append(tempConValue).Append(" ");
                        }
                        else
                        {
                            if (string.Compare("LIKE", tempConChar) == 0)
                            {
                                sbCod.Append("LIKE").Append(" ");
                                sbCod.Append("'%").Append(Str.EraseSingleQutotationString(tempConValue)).Append("%'").Append(" "); ;
                            }
                            else if (string.Compare("LLIKE", tempConChar) == 0)
                            {
                                sbCod.Append("LIKE").Append(" ");
                                sbCod.Append("'%").Append(Str.EraseSingleQutotationString(tempConValue)).Append("' "); ;
                            }
                            else if (string.Compare("RLIKE", tempConChar) == 0)
                            {
                                sbCod.Append("LIKE").Append(" ");
                                sbCod.Append(" '").Append(Str.EraseSingleQutotationString(tempConValue)).Append("%'").Append(" "); ;
                            }
                            else if (string.Compare("IN", tempConChar) == 0)
                            {
                                sbCod.Append("IN").Append(" ");
                                sbCod.Append("(").Append(tempConValue).Append(")").Append(" ");
                            }
                        }
                    }
                    else
                    {
                        // 出现or 关键字
                        for (int j = 0; j < conArrOr.Length; j++)
                        {
                            // 用 or 区分
                            string[] oneCon = conArrOr[j].Split('|');
                            if (oneCon.Length != 2)
                            {
                                continue;
                            }
                            int iseq = oneCon[1].IndexOf('=');
                            int ilen = oneCon[1].Length;


                            sbCod.Append(oneCon[0]).Append(" ");
                            bool isLikeIn = false;
                            //取运算符
                            //string tempConChar = GetConditoalChar(oneCon[1].Substring(0, iseq - 1), ref isLikeIn);
                            //string tempConValue = oneCon[1].Substring(iseq, ilen - iseq);

                            string tempConChar = GetConditoalChar(oneCon[1].Substring(0, iseq), ref isLikeIn);
                            string tempConValue = oneCon[1].Substring(iseq + 1, (ilen - (iseq + 1)));



                            if (!isLikeIn)
                            {
                                sbCod.Append(tempConChar).Append(" ");
                                sbCod.Append(tempConValue).Append(" ");
                            }
                            else
                            {
                                if (string.Compare("LIKE", tempConChar) == 0)
                                {
                                    sbCod.Append("LIKE").Append(" ");
                                    sbCod.Append("'%").Append(Str.EraseSingleQutotationString(tempConValue)).Append("%'").Append(" "); ;
                                }
                                else if (string.Compare("LLIKE", tempConChar) == 0)
                                {
                                    sbCod.Append("LIKE").Append(" ");
                                    sbCod.Append("'%").Append(Str.EraseSingleQutotationString(tempConValue)).Append("' "); ;
                                }
                                else if (string.Compare("RLIKE", tempConChar) == 0)
                                {
                                    sbCod.Append("LIKE").Append(" ");
                                    sbCod.Append(" '").Append(Str.EraseSingleQutotationString(tempConValue)).Append("%'").Append(" "); ;
                                }
                                else if (string.Compare("IN", tempConChar) == 0)
                                {
                                    sbCod.Append("IN").Append(" ");
                                    sbCod.Append("(").Append(tempConValue).Append(")").Append(" ");
                                }
                            }

                            if (j < conArrOr.Length - 1)
                            {
                                sbCod.Append(" OR ");
                            }
                        }
                    }

                    if (i < conArrAnd.Length - 1)
                    {
                        sbCod.Append(" AND ");
                    }
                }

                retStr = sbCod.ToString();
            }
            catch
            {
                //todo 写日志
            }
            return retStr;
        }

        //使用反射拼接字符串
        public static string GetTableFieldList<T>()
        {
            string str = "";
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var columns = props.Select(p => p.Name).ToList();
            for (int i = 1; i < columns.Count; i++)
            {
                if (i == columns.Count - 1)
                {
                    str += columns[i];
                }
                else
                {
                    str += columns[i] + ",";
                }
            }
            str = "(" + str + ")";
            return str;
        }

        public static string GetTableFieldListWithSign<T>()
        {
            string str = "";
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var columns = props.Select(p => p.Name).ToList();
            for (int i = 1; i < columns.Count; i++)
            {
                if (i == 1)
                {
                    str += "@" + columns[i];
                }
                else
                {
                    str += ",@" + columns[i];
                }
            }
            str = "(" + str + ")";
            return str;
        }

        public static string GetTableFiledListByUpdate<T>()
        {
            string str = "";
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var columns = props.Select(p => p.Name).ToList();
            for (int i = 1; i < columns.Count; i++)
            {
                if (i == columns.Count - 1)
                {
                    str += (columns[i] + " = @" + columns[i]);
                }
                else
                {
                    str += (columns[i] + " = @" + columns[i] + ",");
                }
            }
            return str;
        }

        public static DynamicParameters GetUpdateParameter<T>(T obj)
        {
            var parameters = new DynamicParameters();
            Type type = typeof(T);
            //获取全部公开属性
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                var name = property.Name;
                object value = property.GetValue(obj);
                parameters.Add(name, value);
            }
            return parameters;
        }
        public static string GetNextGUID()
        {
            Guid guid = Guid.NewGuid();
            return BitConverter.ToString(guid.ToByteArray()).Replace("-", "");
        }

        public static string GetEquCode(string str)
        {
            string time = (DateTime.Now).ToString();
            //拿到时间的年月日
            string res2 = time.Split(' ')[0];
            //拿到时间的时分秒
            string res3 = time.Split(' ')[1];
            //继续分割年月日
            string[] res4 = res2.Split('/');
            string[] res5 = res3.Split(':');

            string res6 = res4[0].Substring(2, 2);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (int.Parse(res4[1]) < 10)
            {
                str1 = "0" + int.Parse(res4[1]);
            }
            else
            {
                str1 = res4[1];
            }
            if (int.Parse(res4[2]) < 10)
            {
                str2 = "0" + int.Parse(res4[2]);
            }
            else
            {
                str2 = res4[2];
            }
            if (int.Parse(res5[0]) < 10)
            {
                str3 = "0" + int.Parse(res5[0]);
            }
            else
            {
                str3 = res5[0];
            }
            if (int.Parse(res5[1]) < 10)
            {
                str4 = "0" + int.Parse(res5[1]);
            }
            else
            {
                str4 = res5[1];
            }
            if(int.Parse(res5[2])<10)
            {
                str5 = "0" + int.Parse(res5[2]);
            }
            else
            {
                str5 = res5[2];
            }
            return str + res6 + str1 + str2 + str3 + str4+str5;
        }

        public static string GetCheckItemCode(string str)
        {
            string time = (DateTime.Now).ToString();
            //拿到时间的年月日
            string res2 = time.Split(' ')[0];
            //拿到时间的时分秒
            string res3 = time.Split(' ')[1];
            //继续分割年月日
            string[] res4 = res2.Split('/');
            string[] res5 = res3.Split(':');

            string res6 = res4[0].Substring(2, 2);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (int.Parse(res4[1]) < 10)
            {
                str1 = "0" + int.Parse(res4[1]);
            }
            else
            {
                str1 = res4[1];
            }
            if (int.Parse(res4[2]) < 10)
            {
                str2 = "0" + int.Parse(res4[2]);
            }
            else
            {
                str2 = res4[2];
            }
            if (int.Parse(res5[0]) < 10)
            {
                str3 = "0" + int.Parse(res5[0]);
            }
            else
            {
                str3 = res5[0];
            }
            if (int.Parse(res5[1]) < 10)
            {
                str4 = "0" + int.Parse(res5[1]);
            }
            else
            {
                str4 = res5[1];
            }
            if (int.Parse(res5[2]) < 10)
            {
                str5 = "0" + int.Parse(res5[2]);
            }
            else
            {
                str5 = res5[2];
            }
            return "DJ" + "_" + str + "_" + res6 + str1 + str2 + str3 + str4 + str5;
        }

        public static string GetPlanId()
        {
            string time = (DateTime.Now).ToString();
            //拿到时间的年月日
            string res2 = time.Split(' ')[0];
            //拿到时间的时分秒
            string res3 = time.Split(' ')[1];
            //继续分割年月日
            string[] res4 = res2.Split('/');
            string[] res5 = res3.Split(':');

            string res6 = res4[0].Substring(2, 2);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (int.Parse(res4[1]) < 10)
            {
                str1 = "0" + int.Parse(res4[1]);
            }
            else
            {
                str1 = res4[1];
            }
            if (int.Parse(res4[2]) < 10)
            {
                str2 = "0" + int.Parse(res4[2]);
            }
            else
            {
                str2 = res4[2];
            }
            if (int.Parse(res5[0]) < 10)
            {
                str3 = "0" + int.Parse(res5[0]);
            }
            else
            {
                str3 = res5[0];
            }
            if (int.Parse(res5[1]) < 10)
            {
                str4 = "0" + int.Parse(res5[1]);
            }
            else
            {
                str4 = res5[1];
            }
            if (int.Parse(res5[2]) < 10)
            {
                str5 = "0" + int.Parse(res5[2]);
            }
            else
            {
                str5 = res5[2];
            }
            return "PLAN"+ "_" + res6 + str1 + str2 + str3 + str4 + str5;
        }

        public static string GetUpkeepItemCode(string str)
        {
            string time = (DateTime.Now).ToString();
            //拿到时间的年月日
            string res2 = time.Split(' ')[0];
            //拿到时间的时分秒
            string res3 = time.Split(' ')[1];
            //继续分割年月日
            string[] res4 = res2.Split('/');
            string[] res5 = res3.Split(':');

            string res6 = res4[0].Substring(2, 2);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (int.Parse(res4[1]) < 10)
            {
                str1 = "0" + int.Parse(res4[1]);
            }
            else
            {
                str1 = res4[1];
            }
            if (int.Parse(res4[2]) < 10)
            {
                str2 = "0" + int.Parse(res4[2]);
            }
            else
            {
                str2 = res4[2];
            }
            if (int.Parse(res5[0]) < 10)
            {
                str3 = "0" + int.Parse(res5[0]);
            }
            else
            {
                str3 = res5[0];
            }
            if (int.Parse(res5[1]) < 10)
            {
                str4 = "0" + int.Parse(res5[1]);
            }
            else
            {
                str4 = res5[1];
            }
            if (int.Parse(res5[2]) < 10)
            {
                str5 = "0" + int.Parse(res5[2]);
            }
            else
            {
                str5 = res5[2];
            }
            return "BY" + "_" + str + "_" + res6 + str1 + str2 + str3 + str4 + str5;
        }

        public static string GetMaintainItemCode(string str)
        {
            string time = (DateTime.Now).ToString();
            //拿到时间的年月日
            string res2 = time.Split(' ')[0];
            //拿到时间的时分秒
            string res3 = time.Split(' ')[1];
            //继续分割年月日
            string[] res4 = res2.Split('/');
            string[] res5 = res3.Split(':');

            string res6 = res4[0].Substring(2, 2);
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            if (int.Parse(res4[1]) < 10)
            {
                str1 = "0" + int.Parse(res4[1]);
            }
            else
            {
                str1 = res4[1];
            }
            if (int.Parse(res4[2]) < 10)
            {
                str2 = "0" + int.Parse(res4[2]);
            }
            else
            {
                str2 = res4[2];
            }
            if (int.Parse(res5[0]) < 10)
            {
                str3 = "0" + int.Parse(res5[0]);
            }
            else
            {
                str3 = res5[0];
            }
            if (int.Parse(res5[1]) < 10)
            {
                str4 = "0" + int.Parse(res5[1]);
            }
            else
            {
                str4 = res5[1];
            }
            if (int.Parse(res5[2]) < 10)
            {
                str5 = "0" + int.Parse(res5[2]);
            }
            else
            {
                str5 = res5[2];
            }
            return "WX" + "_" + str + "_" + res6 + str1 + str2 + str3 + str4 + str5;
        }
    }
}
