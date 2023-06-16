using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{
    /// <summary>
    /// 设备子设备关联表控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Equipment", IgnoreApi = false)]
    public class BaseEquipmentSubController : ControllerBase
    {
        private readonly BaseEquipmentSubApp _app;
        private readonly PikachuApp _pikachuApp;
        private readonly ILogger<BaseEquipmentSubController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="picachuApp"></param>
        /// <param name="logger"></param>
        public BaseEquipmentSubController(BaseEquipmentSubApp app, PikachuApp picachuApp, ILogger<BaseEquipmentSubController> logger)
        {
            _app = app;
            _pikachuApp = picachuApp;
            _logger = logger;
        }


        /// <summary>
        /// 向关联表中添加设备子设备
        /// </summary>
        /// <param name="parent_id">父设备</param>
        /// <param name="ids">子设备，页面支持多选</param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> AddEquipmentSubEquipment(string parent_id, List<string> ids)
        {
            Response<string> result = new Response<string>();
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    throw new Exception("子设备不能为空");
                }

                foreach (string temp in ids)
                {
                    if (temp == parent_id)
                    {
                        throw new Exception("子设备id不能和父设备id相同");
                    }
                }

                bool insert = _app.InsertSubEquipment(parent_id, ids);
                result.Result = "添加父子关联成功";
                if (!insert)
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
        /// 传入设备id，通过子设备关联表获取二级设备
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetSecondEquipmentByEquipmentId(string equipment_id)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                List<base_equipment_sub> subs = _pikachuApp.GetAll<base_equipment_sub>().Where(c => c.equipment_id == equipment_id).ToList();
                pd.Data = subs;
                pd.Total = subs.Count;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            result.Result = pd;
            return result;
        }

        /// <summary>
        /// 通过设备id获取工作单元层级树
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> WorkUnitHierarchyTree(string equipment_id)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                List<base_equipment_sub> subequipments = _pikachuApp.GetAll<base_equipment_sub>().Where(c => c.equipment_id == equipment_id).ToList();
                //手动加根
                base_equipment_sub root = new();
                root.equipment_id = null;
                root.equipment_sub_id = equipment_id;
                if (subequipments == null)
                {
                    subequipments = new();
                }

                subequipments.Add(root);
                //转化为Node
                List<TreeModel> equipment_nodes = _pikachuApp.ListElementToNode(subequipments, false);

                //将节点列表转化为树
                List<TreeModel> treeModels = _app.ListToTreeModel(equipment_nodes, equipment_id);
                //将树转化为TreeEasy树
                List<TreeEasy> easies = _pikachuApp.TreeModelToTreeEasy(treeModels);
                pd.Data = easies;
                pd.Total = easies.Count();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }


            result.Result = pd;
            return result;
        }


        /// <summary>
        /// 添加设备子设备,选择界面的地点设备树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> AddSubEquipment(string equipment_id)
        {
            Response<PageData> result = new Response<PageData>();
            PageData pd = new PageData();
            try
            {
                //节点列表
                List<TreeModel> Nodes = new();
                //获取location
                List<base_location> list_location = _pikachuApp.GetRootAndBranch<base_location>(null);
                //转化为node
                Nodes = _pikachuApp.ListElementToNode(list_location);
                //为Node添加type
                foreach (TreeModel node in Nodes)
                {
                    node.NodeType = "location";
                }


                //获取所有无子的设备
                List<base_equipment> list_equipment = _pikachuApp.GetAll<base_equipment>();
                //移除传进来的equipment_id选中的设备
                base_equipment item = list_equipment.Where(c => c.equipment_id.Equals(equipment_id)).FirstOrDefault();
                list_equipment.Remove(item);
                //转化为Node
                List<TreeModel> equipment_nodes = _pikachuApp.ListElementToNode(list_equipment);
                foreach (TreeModel temp in equipment_nodes)
                {
                    temp.NodeType = "equipment";
                }


                //找到parentid为null的设备节点，将Parentid改为locationid用以衔接
                foreach (TreeModel temp in equipment_nodes)
                {
                    if (string.IsNullOrEmpty(temp.parentId))
                    {
                        temp.parentId = list_equipment.Where(c => c.equipment_name.Equals(temp.label)).FirstOrDefault().location_id;
                    }
                }
                //并添加进节点列表
                Nodes.AddRange(equipment_nodes);

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
        /// 删除设备关联
        /// </summary>
        /// <param name="id">主设备id</param>
        /// <param name="sub_id">下级设备id</param>
        [HttpPost]
        public Response<string> Delete(string id, string sub_id)
        {
            Response<string> result = new Response<string>();
            try
            {
                result.Result = $"id:{id}" + Environment.NewLine + $"sub_id:{sub_id}";
                if (!_app.Delete(id, sub_id))
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
                _logger.LogError(ex.Message);
            }

            return result;
        }

    }
}
