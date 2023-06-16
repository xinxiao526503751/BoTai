using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.WareHouse
{
    /// <summary>
    /// 出入库类型控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "WareHouse", IgnoreApi = false)]
    public class IOWareHouseTypeController : ControllerBase
    {
        private readonly PikachuApp _pikachuApp;
        private readonly IoWareHouseTypeApp _iOWareHouseTypeApp;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="pikachuApp"></param>
        /// <param name="iOWareHouseTypeApp"></param>
        public IOWareHouseTypeController(PikachuApp pikachuApp, IoWareHouseTypeApp iOWareHouseTypeApp)
        {
            _pikachuApp = pikachuApp;
            _iOWareHouseTypeApp = iOWareHouseTypeApp;
        }

        /// <summary>
        /// 新建出入库类型信息
        /// </summary>
        /// <param name="obj">部门对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> CreateWareHouseType(base_iowarehouse_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (_iOWareHouseTypeApp.CreateWareHouseType(obj))
                {
                    result.Result = obj.iowarehouse_type_id;
                }
                else
                {
                    result.Code = 100;
                    result.Message = "数据写入失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }



        /// <summary>
        /// 删除出入库类型信息
        /// </summary>
        /// <param name="ids">出入库类型id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;



                if (!_pikachuApp.DeleteMask<base_iowarehouse_type>(ids))
                {
                    result.Code = 100;
                    result.Message = "操作失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 更新出入库类型信息
        /// </summary>
        /// <param name="obj">出入库类型对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_iowarehouse_type obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_iowarehouse_type base_Iowarehouse_Type = _pikachuApp.GetById<base_iowarehouse_type>(obj.iowarehouse_type_id);
                //如果能够根据id找到出入库类型信息
                if (base_Iowarehouse_Type != null)
                {
                    obj.iowarehouse_type_code = base_Iowarehouse_Type.iowarehouse_type_code;//锁死code
                    obj.create_time = base_Iowarehouse_Type.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//给定修改时间
                }
                else
                { //找不到要返回错误信息
                    result.Result = obj.iowarehouse_type_id;
                    result.Code = 100;
                    result.Message = "没有此id信息";
                    return result;
                }

                base_iowarehouse_type temp = new();
                //如果修改的是入库类型，要在出库类型中找重名的类型
                if (obj.io_type == 0)
                {
                    temp = _pikachuApp.GetAll<base_iowarehouse_type>().Where(c => c.iowarehouse_type_code == obj.iowarehouse_type_name && obj.io_type == 1).FirstOrDefault();
                }
                else if (obj.io_type == 1)
                {
                    temp = _pikachuApp.GetAll<base_iowarehouse_type>().Where(c => c.iowarehouse_type_code == obj.iowarehouse_type_name && obj.io_type == 0).FirstOrDefault();
                }

                if (temp != null)
                {
                    throw new Exception("已经有同名的类型存在");
                }

                result.Result = obj.iowarehouse_type_id;

                if (!_pikachuApp.Update(obj))
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "更新失败！";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }



        /// <summary>
        /// 得到出入库列表数据
        /// </summary>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _pikachuApp.GetList<base_iowarehouse_type>(req, ref lcount);
                pd.Total = lcount;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }
    }
}

