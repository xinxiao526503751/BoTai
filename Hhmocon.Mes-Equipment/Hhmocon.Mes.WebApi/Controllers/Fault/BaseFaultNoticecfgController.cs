using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.WebApi.Controllers.Fault
{
    /// <summary>
    /// 事件通知配置控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Fault", IgnoreApi = false)]
    public class BaseFaultNoticecfgController : ControllerBase
    {
        private readonly BaseFaultNoticecfgApp _app;
        private readonly BaseFaultClassApp _baseFaultClassApp;
        private readonly PikachuApp _pikachuApp;
        private readonly BaseFaultApp _baseFaultApp;
        private readonly SysDeptApp _sysDeptApp;
        private readonly SysUserApp _sysUserApp;

        public BaseFaultNoticecfgController(BaseFaultNoticecfgApp app, BaseFaultClassApp baseFaultClassApp, PikachuApp picachuApp, BaseFaultApp baseFaultApp, SysDeptApp sysDeptApp, SysUserApp sysUserApp)
        {
            _app = app;
            _pikachuApp = picachuApp;
            _baseFaultClassApp = baseFaultClassApp;
            _baseFaultApp = baseFaultApp;
            _sysDeptApp = sysDeptApp;
            _sysUserApp = sysUserApp;
        }

        /// <summary>
        /// 新建事件通知配置.该控制器对应事件通知配置页面右上角的保存按钮，
        /// 由于通知人员可以多选，需对user进行解析
        /// 由于通知类型包含微信、短信、email三种，需解析
        ///
        /// </summary>
        /// <param name="faultNoticecfgRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(FaultNoticecfgRequest faultNoticecfgRequest)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_fault_noticecfg t = new();
                t.fault_id = faultNoticecfgRequest.fault_id;
                t.notice_type = string.Join('、', faultNoticecfgRequest.notice_type);
                t.notice_level = faultNoticecfgRequest.notice_level;
                bool effect = _app.InsertBaseFaultNoticeCfg(t, faultNoticecfgRequest.user_id);
                if (effect)
                {
                    result.Result = "写入成功";
                }
                else
                {
                    //更新失败
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
        /// 删除事件通知配置
        /// </summary>
        /// <param name="ids">删除的对象ids</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (!_pikachuApp.DeleteMask<base_fault_noticecfg>(ids))
                {
                    //更新失败
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
        /// 修改事件通知配置
        /// </summary>
        /// <param name="obj">修改后的对象</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_fault_noticecfg obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_fault_noticecfg _Base_Fault = _pikachuApp.GetById<base_fault_noticecfg>(obj.fault_id);
                if (_Base_Fault != null)
                {
                    obj.create_time = _Base_Fault.create_time;//锁死创建时间
                    obj.modified_time = Time.Now;//更新修改时间
                }
                else
                {
                    result.Result = obj.fault_id;
                    result.Code = 100;
                    result.Message = "更新失败！没有此id信息";
                    return result;
                }
                result.Result = obj.fault_id;

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
        /// 得到事件通知配置
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
                pd.Data = _pikachuApp.GetList<base_fault_noticecfg>(req, ref lcount);
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
        /// 得到事件通知配置详细信息
        /// </summary>
        /// <param name="id">对象id</param>
        /// <returns></returns>
        [HttpPost]
        public Response<base_fault_noticecfg> GetDetail(string id)
        {
            Response<base_fault_noticecfg> result = new Response<base_fault_noticecfg>();
            try
            {
                result.Result = _pikachuApp.GetById<base_fault_noticecfg>(id);
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 左边事件类型+事件名称树
        /// </summary>
        /// <returns>List[TreeModel]</returns>
        [HttpPost]
        public Response<PageData> GetFailClassAndFailTree()
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                //节点列表
                List<TreeModel> Nodes = new();
                //获取所有base_fault_class
                List<base_fault_class> list_fault_class = _pikachuApp.GetAll<base_fault_class>();
                //转化为node
                Nodes = _baseFaultClassApp.ListElementToNode(list_fault_class);
                //获取所有的事件(事件没有父子关系)
                List<base_fault> list_fault = _pikachuApp.GetAll<base_fault>();
                //转化为Node
                List<TreeModel> fault_nodes = _baseFaultApp.ListElementToNodeLinkWithClass(list_fault);
                //并添加进节点列表
                Nodes.AddRange(fault_nodes);
                //将节点列表转化为树
                List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(Nodes, null);
                //将树转化为TreeEasy树
                List<TreeEasy> easies = _pikachuApp.TreeModelToTreeEasy(treeModels);

                pd.Data = easies;
                pd.Total = easies.Count;
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
        /// 获取部门+用户树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetDeptAndUserTree()
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                //节点列表
                List<TreeModel> Nodes = new();
                //获取所有部门(部门有父子关系)
                List<sys_dept> list_sys_dept = _pikachuApp.GetAll<sys_dept>();
                //转化为node
                Nodes = _sysDeptApp.ListElementToNode(list_sys_dept);
                //获取所有的用户(用户没有父子关系)
                List<sys_user> list_user = _pikachuApp.GetAll<sys_user>();
                //转化为Node
                List<TreeModel> fault_nodes = _sysUserApp.ListElementToNodeLinkWithDept(list_user);
                //并添加进节点列表
                Nodes.AddRange(fault_nodes);
                //将节点列表转化为树
                List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(Nodes, null);
                //将树转化为TreeEasy树
                List<TreeEasy> easies = _pikachuApp.TreeModelToTreeEasy(treeModels);

                pd.Data = easies;
                pd.Total = easies.Count;
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
        /// 通过事件id和通知等级notice_level获取已保存的配置人员
        /// </summary>
        /// <param name="fault_id">事件id</param>
        /// <param name="notice_level">通知等级</param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<SelectedNotifierResponse>> GetByNoticeLevelAndFaultId(string fault_id, string notice_level)
        {
            Response<List<SelectedNotifierResponse>> result = new Response<List<SelectedNotifierResponse>>();
            try
            {
                List<SelectedNotifierResponse> data = new();
                List<base_fault_noticecfg> base_Fault_Noticecfgs = _app.GetByFaultIdAndNoticeLevel(fault_id, notice_level);
                foreach (base_fault_noticecfg temp in base_Fault_Noticecfgs)
                {
                    SelectedNotifierResponse selectedNotifierResponse = new();
                    selectedNotifierResponse.user_id = temp.user_id;
                    selectedNotifierResponse.user_cn_name = _pikachuApp.GetById<sys_user>(temp.user_id).user_name;
                    selectedNotifierResponse.notice_type = temp.notice_type;
                    data.Add(selectedNotifierResponse);
                }

                result.Result = data;
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
