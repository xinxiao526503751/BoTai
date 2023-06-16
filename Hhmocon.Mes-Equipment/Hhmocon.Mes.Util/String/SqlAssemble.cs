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

namespace Hhmocon.Mes.Util.String
{
    /// <summary>
    /// sql语句拼装
    /// </summary>
    public static class SqlAssemble
    {
        /// <summary>
        /// 用于给条件判断字符串装配假查询条件
        /// where  (判断) and delete_mark = '0'
        /// where delete_mark = '0'
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public static string Delete_Mark(string whereStr)
        {
            if (string.IsNullOrEmpty(whereStr))
            {
                whereStr = " DELETE_MARK = '0'";
            }
            else
            {
                whereStr = "(" + whereStr + " )" + " AND DELETE_MARK = '0'";
            }
            whereStr = "WHERE " + whereStr;
            return whereStr;
        }
    }
}
