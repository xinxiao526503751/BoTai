using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using hmocon.Mes.Repository.Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.WebApi.Controllers.Base
{

    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiController]
    public class BaseBomController : ControllerBase
    {
        private readonly BaseBomApp _app;
        private readonly BaseBomDetailApp _appDet;
        private readonly PikachuApp _pikachuApp;

        public BaseBomController(BaseBomApp app, BaseBomDetailApp appDet, PikachuApp picachuApp)
        {
            _app = app;
            _appDet = appDet;
            _pikachuApp = picachuApp;

        }

        /// <summary>
        /// 新建bom
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Create(base_bom_plus obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                base_bom_plus data = _app.Insert(obj);
                if (data != null)
                {
                    result.Result = data.base_Bom.bom_id;
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
        /// 删除bom
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string[]> Delete(string[] ids)
        {
            Response<string[]> result = new Response<string[]>();
            try
            {
                result.Result = ids;

                if (!_pikachuApp.DeleteMask<base_bom>(ids))
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
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<string> Update(base_bom_plus obj)
        {
            Response<string> result = new Response<string>();
            try
            {
                result.Result = obj.base_Bom.bom_id;

                if (_app.Update(obj) == null)
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
        /// 获取物料类型和定义bom的物料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Response<List<TreeModel>> ObtainMaterialPlusNodes()
        {
            Response<List<TreeModel>> result = new Response<List<TreeModel>>();
            try
            {
                List<base_material_type> materialType = _pikachuApp.GetAll<base_material_type>();
                List<base_bom> boms = _pikachuApp.GetAll<base_bom>();
                string[] materialids = (from bom in boms select bom.material_id).ToArray();
                string[] array2;
                //materialids.GroupBy(p => p).Select(p => p.Key).ToArray();//去重
                array2 = materialids.Distinct().ToArray();//去重
                List<base_material> material = _pikachuApp.GetAllByIds<base_material>(array2);
                List<TreeModel> materialTypeNodes = _pikachuApp.ListElementToNode(materialType);
                List<TreeModel> materialNodes = _pikachuApp.ListElementToNode(material);
                materialTypeNodes.AddRange(materialNodes);

                List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(materialTypeNodes);
                result.Result = treeModels;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取以当前bom下的子bom数据
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<List<base_bom>> GetBomTreeByParentId(string ParentId)
        {
            Response<List<base_bom>> result = new Response<List<base_bom>>();
            try
            {
                List<base_bom> boms = _app.GetBomTree(ParentId);
                result.Result = boms;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 根据物料id获取bom列表
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetBomListByMaterialId(string materialId, PageReq req)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                if (materialId == null)
                {
                    pd.Data = null;
                }
                else if (_app.GetBomLiistByMaterialId(materialId, req, ref lcount) != null)
                {
                    pd.Data =
                           _app.GetBomLiistByMaterialId(materialId, req, ref lcount);
                }
                else
                {
                    pd.Data = null;
                }

                pd.Total = lcount;
                result.Result = pd;
                // List<base_bom_response> BomResponses = _app.GetRootAndBranch(materialId);
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
