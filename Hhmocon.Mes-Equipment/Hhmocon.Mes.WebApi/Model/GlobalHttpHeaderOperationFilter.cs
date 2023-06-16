
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;


namespace Hhmocon.Mes.WebApi.Model
{
    /// <summary>
    /// swagger请求头
    /// </summary>
    public class GlobalHttpHeaderOperationFilter : IOperationFilter
    {

        public GlobalHttpHeaderOperationFilter()
        {

        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            IList<object> actionAttrs = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            bool isAnony = actionAttrs.Any(a => a.GetType() == typeof(AllowAnonymousAttribute));

            //不是匿名，则添加默认的X-Token
            if (!isAnony)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = SysDefine.TOKEN_NAME,
                    In = ParameterLocation.Header,
                    Description = "当前登录用户登录token",
                    Required = false
                });
            }
        }
    }
}
