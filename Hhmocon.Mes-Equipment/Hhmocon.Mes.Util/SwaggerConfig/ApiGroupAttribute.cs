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

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;

namespace Hhmocon.Mes.Util.SwaggerConfig
{
    /// <summary>
    /// 自定义Api分组特性.在接口文档有一个接口对多个分组 这样的需求时 使用
    /// 暂未实现
    /// 需要在
    /// DocInclusionPredicate中修改判断逻辑
    /// </summary>
    public class ApiGroupAttribute : Attribute, IApiDescriptionGroupNameProvider
    {
        public ApiGroupAttribute(ApiGroupNames[] name)
        {
            GroupName = name.ToString();
        }

        public string GroupName { get; set; }

        //public ApiGroupNames[] GroupName { get; set; }
    }




}
