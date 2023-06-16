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

using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.Response
{
    /// <summary>
    /// 表头
    /// </summary>
    public class right_header
    {
        /// <summary>
        /// 不合格原因name
        /// </summary>
        public string label;
        /// <summary>
        /// 不合格原因code
        /// </summary>
        public string key;
    }

    /// <summary>
    /// 表数据
    /// </summary>
    public class header_data
    {
        /// <summary>
        /// 年-月
        /// </summary>
        public string yearAndMouth;
        /// <summary>
        /// 当月所有不合格品总数
        /// </summary>
        public string AllTotal;

    }

    public class UnqulifiedStaticsByTime
    {
        /// <summary>
        /// 表数据
        /// </summary>
        public List<Dictionary<string, string>> headerDatas;

        /// <summary>
        /// 表头
        /// </summary>
        public List<right_header> right_Headers;
    }


}
