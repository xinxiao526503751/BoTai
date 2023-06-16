using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Exam;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Exam
{
    /// <summary>
    /// 设备点检项接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Exam", IgnoreApi = false)]
    public class ExamEquipmentItemController : ControllerBase
    {
        private readonly ExamEquipmentItemApp _app;
        private readonly PikachuApp _picachuApp;

        /// <summary>
        /// 设备点检项控制器
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        public ExamEquipmentItemController(ExamEquipmentItemApp app, PikachuApp picachuApp)
        {
            _app = app;
            _picachuApp = picachuApp;
        }

        /// <summary>
        /// 新增挂载在设备下的点检定义
        /// </summary>
        /// <param name="eqp_Exe"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(eqp_exe eqp_Exe)
        {
            Response<string> result = new Response<string>();

            try
            {
                string data = _app.InsertExamEquipmentItem(eqp_Exe);
                if (data == eqp_Exe.equipment_id)
                {
                    result.Result = data;
                }
                else
                {
                    //更新失败
                    result.Code = 100;
                    result.Message = "请检查点检项目是否重复添加，点检信息是否存在";
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
        /// 获取所有关联数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetList(PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _picachuApp.GetList<exam_equipment_item>(req, ref lcount);
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


        /// <summary>
        /// 删除挂载在设备下的点检定义
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        public Response<string[]> Delete(string[] ids)

        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;
                if (!_picachuApp.DeleteMask<exam_equipment_item>(ids))
                {
                    result.Code = 100;
                    result.Message = "操作失败";
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
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(exam_equipment_item obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                exam_equipment_item _Exam_Equipment_Item = _picachuApp.GetById<exam_equipment_item>(obj.exam_equipment_item_id);
                if (_Exam_Equipment_Item != null)
                {
                    obj.create_time = _Exam_Equipment_Item.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;
                    obj.examitem_code = _Exam_Equipment_Item.examitem_code;
                    obj.equipment_id = _Exam_Equipment_Item.equipment_id;
                    obj.equipment_id = _Exam_Equipment_Item.equipment_id;
                    obj.delete_mark = _Exam_Equipment_Item.delete_mark;
                    obj.examitem_name = _Exam_Equipment_Item.examitem_name;
                    obj.method_type = _Exam_Equipment_Item.method_type;
                }
                else
                {
                    //找不到顾客要返回错误信息
                    result.Result = obj.exam_equipment_item_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.exam_equipment_item_id;

                if (!_picachuApp.Update(obj))
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
        ///  根据设备ID获取挂载下面点检项目/维修/保养简单数据
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetExamitemByEquipmentIdSampleData(string equipment_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            List<exam_equipment_item> exam_Equipment_Items = new();
            try
            {
                //根据equipment_id查找对应的关系表
                exam_Equipment_Items = _app.GetByEquipmentId(equipment_id).Where(c => c.method_type == req.key).ToList();
                pd.Total = exam_Equipment_Items.Count;
                if (req != null)
                {
                    int iPage = req.page;
                    int iRows = req.rows;
                    //分页
                    exam_Equipment_Items = exam_Equipment_Items.OrderBy(c => c.examitem_code).Skip((iPage - 1) * iRows).Take(iRows).ToList();
                }
                pd.Data = exam_Equipment_Items;

                result.Result = pd;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 根据设备ID获取挂载下面点检项目/维修/保养详情数据
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <param name="req">区分点检/维修/保养 通过给req = "1"这样的格式</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetExamitemByEquipmentId(string equipment_id, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            List<base_examitem> examitems = new();
            try
            {
                //根据equipment_id查找对应的关系表
                List<exam_equipment_item> exam_Equipment_Items = _app.GetByEquipmentId(equipment_id).Where(c => c.method_type == req.key).ToList();
                //根据关系表获取对应的设备列表
                foreach (exam_equipment_item temp in exam_Equipment_Items)
                {
                    base_examitem _base_Examitems = _picachuApp.GetById<base_examitem>(temp.examitem_id);
                    if (_base_Examitems != null)
                    {
                        examitems.Add(_base_Examitems);
                    }

                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            if (req != null)
            {
                int iPage = req.page;
                int iRows = req.rows;
                //分页
                examitems = examitems.Skip((iPage - 1) * iRows).Take(iRows).ToList();
            }
            pd.Data = examitems;
            pd.Total = examitems.Count;
            result.Result = pd;
            return result;
        }



        /// <summary>
        /// 根据exam_equipment_item_id获取特定的一条关联参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<exam_equipment_item> GetDetail(string id)
        {
            Response<exam_equipment_item> result = new Response<exam_equipment_item>();
            try
            {
                result.Result = _picachuApp.GetById<exam_equipment_item>(id);
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


