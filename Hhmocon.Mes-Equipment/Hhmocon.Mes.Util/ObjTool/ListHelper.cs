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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hhmocon.Mes.Util
{
    public static class ListHelper
    {

        /// <summary>
        /// list克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Clone<T>(this List<T> list) where T : new()
        {
            string str = JsonConvert.SerializeObject(list);
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

    }
}
