using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Lituo;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.AuthStrategies;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.LoginRelated;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.AndroidApi

{
    /// <summary>
    /// 手持机接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "Android", IgnoreApi = false)]
    [ApiController]

    public class AndroidApiController : ControllerBase
    {
        private readonly IAuth _authUtil;

        private readonly ILogger<AndroidApiController> _logger;

        private readonly AuthStrategyContext _authStrategyContext;

        private readonly LituoProductionTaskMainApp _lituoProductionTaskMainApp;

        private readonly LituoAndroidApiApp _lituoAndroidApiApp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authUtil"></param>
        /// <param name="logger"></param>
        /// <param name="lituoProductionTaskMainApp"></param>
        /// <param name="lituoAndroidApiApp"></param>
        public AndroidApiController(IAuth authUtil, ILogger<AndroidApiController> logger, LituoProductionTaskMainApp lituoProductionTaskMainApp,
            LituoAndroidApiApp lituoAndroidApiApp)//依赖注入
        {
            _authUtil = authUtil;
            _logger = logger;
            _authStrategyContext = _authUtil.GetCurrentUser();
            _lituoProductionTaskMainApp = lituoProductionTaskMainApp;
            _lituoAndroidApiApp = lituoAndroidApiApp;
        }


        /// <summary>
        /// Android登录接口
        /// </summary>
        /// <param name="request">登录参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public AndroidLoginResult Login([FromBody] PassportLoginRequest request)//登录api  从页面post进PassportLoginRequest
        {
            _logger.LogInformation("Android Api Login enter");//往日志里写入记录
            AndroidLoginResult result = new AndroidLoginResult();
            try
            {
                result = _authUtil.AndroidLogin(request.AppKey, request.Account, request.Password);    //检验appkey   和账号密码
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Android注销登录 注销之需要清除android的缓存即可
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<bool> Logout()
        {
            Response<bool> resp = new Response<bool>();
            try
            {
                resp.Result = true;
            }
            catch (Exception e)
            {
                resp.Result = false;
                resp.Message = e.Message;
            }
            return resp;
        }

        /// <summary>
        /// 获取前端的物料名称
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<MaterialInfo>> GetMaterialInfo()
        {
            List<MaterialInfo> materialInfos = new List<MaterialInfo>();
            Response < List<MaterialInfo>> result = new Response<List<MaterialInfo>>();
            try
            {
                List<base_material> base_Materials = _lituoAndroidApiApp.GetMaterialList();
                foreach(var item in base_Materials)
                {
                    MaterialInfo materialInfo = new MaterialInfo();
                    materialInfo.MaterialCode = item.material_code;
                    materialInfo.MaterialName = item.material_name;
                    materialInfo.MaterialType = item.material_type_code;
                    materialInfo.MaterialId = item.material_id;
                    materialInfos.Add(materialInfo);
                }

                result.Result = materialInfos;


            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }


        [HttpPost]
        public Response<string> RawEnterOuterStock([FromBody] EnterOutParm request)
        {
            Response<string> response = new Response<string>();
            try
            {
                _lituoAndroidApiApp.StockOperate(request.username, request.usercode, request.kwcode, request.pcnum, request.eoflag,
                    request.qty, request.materialid);
                response.Result = "操作成功";

            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = ex.Message;
            }
            return response;
        }



        /// <summary>
        ///  工序报工
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="processid"></param>
        /// <param name="processUserid"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> ReportOrderProcess([FromBody] ReportParm request)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (string.IsNullOrEmpty(request.orderid))
                {
                    throw new Exception("请输入订单单号！");
                }
                if (string.IsNullOrEmpty(request.processid))
                {
                    throw new Exception("请验证该用户是否有报工权限！！！");
                }

                result.Result = _lituoProductionTaskMainApp.reportPrcess(request.orderid, request.processid, request.processUserid);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;

        }



        /// <summary>
        ///  工序报工
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="processid"></param>
        /// <param name="processUserid"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> OrderShip([FromBody] ReportParm request)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (string.IsNullOrEmpty(request.orderid))
                {
                    throw new Exception("请输入订单单号！");
                }
                

                result.Result = _lituoProductionTaskMainApp.OrderShip(request.orderid, request.processUserid);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;

        }


        /// <summary>
        ///  货架入库
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="processid"></param>
        /// <param name="processUserid"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> OrderShelfEnterStock([FromBody] ReportParm request)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (string.IsNullOrEmpty(request.orderid))
                {
                    throw new Exception("请输入货架号！");
                }


                result.Result = _lituoProductionTaskMainApp.OrderShelfEnterStock(request.orderid,request.processid, request.processUserid);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;

        }


        /// <summary>
        ///  订单上架
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="processid"></param>
        /// <param name="processUserid"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> OrderLoadingShelf([FromBody] ReportParm request)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (string.IsNullOrEmpty(request.orderid))
                {
                    throw new Exception("请输入订单单号！");
                }


                result.Result = _lituoProductionTaskMainApp.OrderLoadingShelf(request.orderid, request.processid, request.processUserid);

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }
            return result;

        }


    }


    /// <summary>
    /// 返回给andorid端的 物料信息  浸胶产线的原料名称  理化板出入库的 物品名称
    /// </summary>
    public class MaterialInfo
    {
        public string MaterialType { get; set; }
        //物料名称
        public string MaterialName { get; set; }
        //物料编码
        public string MaterialCode { get; set; }

        public string MaterialId { get; set; }

        public MaterialInfo()
        {
            MaterialType = string.Empty;
            MaterialName = string.Empty;
            MaterialCode = string.Empty;
            MaterialId = string.Empty;
        }

    }



    /// <summary>
    /// 报工请求参数
    /// </summary>
    public class ReportParm
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderid { get; set; }
        /// <summary>
        /// 工序id
        /// </summary>
        public string processid { get; set; }
        /// <summary>
        /// 报工人id
        /// </summary>
        public string processUserid { get; set; }
    }



    //string materialid, string kwcode, string pccode, string qty, string usercode

    /// <summary>
    /// 出入库请求参数
    /// </summary>
    public class EnterOutParm
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string materialid { get; set; }
        /// <summary>
        /// 工序id
        /// </summary>
        public string kwcode { get; set; }
        /// <summary>
        /// 批次id
        /// </summary>
        public string pcnum { get; set; }

        public string qty { get; set; }

        /// <summary>
        /// 出入库标志
        /// </summary>
        public string eoflag { get; set; }
        //用户id
        public string username{ get; set; }

        /// <summary>
        ///用户code
        /// </summary>
        public string usercode { get; set; }
    }


}
