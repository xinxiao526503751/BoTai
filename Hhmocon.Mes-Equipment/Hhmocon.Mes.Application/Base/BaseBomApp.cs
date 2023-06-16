using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using hmocon.Mes.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    public class BaseBomApp
    {
        private readonly IBaseBomRepository _baseBomRepository;
        private readonly IBaseDetailBomRespository _baseBomDetailRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseBomApp(IBaseBomRepository IbaseBomRepository, IBaseDetailBomRespository IbaseBomDetailRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseBomRepository = IbaseBomRepository;
            _baseBomDetailRepository = IbaseBomDetailRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        public base_bom_plus Insert(base_bom_plus data)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                IDbTransaction transaction = conn.BeginTransaction();

                try
                {
                    //取ID
                    data.base_Bom.bom_id = CommonHelper.GetNextGUID();
                    data.base_Bom.modified_time = Time.Now;
                    data.base_Bom.create_time = DateTime.Now;
                    data.base_Bom.create_by_name = _auth.GetUserName(null);
                    data.base_Bom.create_by = _auth.GetUserAccount(null);
                    data.base_Bom.modified_by = _auth.GetUserAccount(null);
                    data.base_Bom.modified_by_name = _auth.GetUserName(null);
                    base_bom getbycode = _pikachuRepository.GetByCode<base_bom>(data.base_Bom.bom_code);
                    base_bom getbyname = _pikachuRepository.GetAll<base_bom>().Where(a => a.bom_name == data.base_Bom.bom_name).ToList().FirstOrDefault();

                    if (getbycode != null || getbyname != null)//如果能根据name或code找到内容
                    {
                        throw new Exception(string.Format("名称或编码重复，已存在有该名称={0}或编码={1}的数据，请检查并重新填写", data.base_Bom.bom_name, data.base_Bom.bom_code));

                    }
                    if (!_pikachuRepository.Insert(data.base_Bom, tran: transaction, dbConnection: conn))
                    {
                        throw new Exception("数据写入失败");
                    }

                    if (data.base_Bom_Details == null)
                    {
                        transaction.Commit();
                        return data;
                    }

                    foreach (base_bom_detail item in data.base_Bom_Details)
                    {
                        item.bom_detail_id = CommonHelper.GetNextGUID();
                        item.bom_id = data.base_Bom.bom_id;
                        item.modified_time = DateTime.Now;
                        item.create_time = DateTime.Now;
                        item.create_by_name = _auth.GetUserName(null);
                        item.create_by = _auth.GetUserAccount(null);
                        item.modified_by = _auth.GetUserAccount(null);
                        item.modified_by_name = _auth.GetUserName(null);
                        if (!_pikachuRepository.Insert(item, tran: transaction, dbConnection: conn))
                        {
                            throw new Exception("数据写入失败");
                        }

                    }
                    transaction.Commit();
                    return data;
                }


                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 修改plus
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_bom_plus Update(base_bom_plus data)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            using (conn)
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    base_bom _bom = _pikachuRepository.GetById<base_bom>(data.base_Bom.bom_id);
                    //如果能够根据id找到
                    if (_bom == null)
                    {
                        throw new Exception("没有找到bom数据");
                    }

                    data.base_Bom.bom_code = _bom.bom_code;//锁死code
                    data.base_Bom.create_time = _bom.create_time;//锁死创建时间
                    data.base_Bom.modified_time = DateTime.Now;
                    if (data.base_Bom.bom_id == data.base_Bom.parent_bom_id)
                    {
                        throw new Exception("上级bom不能选定自身");
                    }

                    if (!_pikachuRepository.Update(data.base_Bom, tran: transaction, dbConnection: conn))
                    {
                        throw new Exception("数据写入失败");
                    }

                    if (data.base_Bom_Details == null)
                    {
                        transaction.Commit();
                        return data;
                    }
                    List<base_bom_detail> bom_Details = _pikachuRepository.GetAll<base_bom_detail>().Where(a => a.bom_id == data.base_Bom.bom_id).ToList();
                    string[] ids = (from bom_Detail in bom_Details select bom_Detail.bom_detail_id).ToArray();

                    foreach (base_bom_detail item in data.base_Bom_Details)
                    {
                        //if (item.bom_detail_id == null)//走新增
                        //{                    

                        item.bom_detail_id = CommonHelper.GetNextGUID();
                        item.bom_id = data.base_Bom.bom_id;
                        item.modified_time = Time.Now;
                        item.create_time = DateTime.Now;
                        item.create_by_name = _auth.GetUserName(null);
                        item.create_by = _auth.GetUserAccount(null);
                        item.modified_by = _auth.GetUserAccount(null);
                        item.modified_by_name = _auth.GetUserName(null);
                        if (!_pikachuRepository.Insert(item, tran: transaction, dbConnection: conn))
                        {
                            throw new Exception("数据写入失败");
                        }

                        //else//走修改
                        //{
                        //    base_bom_detail detail = _pikachuRepository.GetById<base_bom_detail>(item.bom_detail_id, tran: transaction, dbConnection: conn);
                        //    item.bom_detail_id = CommonHelper.GetNextGUID();
                        //    item.bom_id = detail.bom_id;
                        //    item.bom_detail_id = detail.bom_detail_id;
                        //    item.modified_time = DateTime.Now;
                        //    item.create_time = detail.create_time;//锁死创建时间
                        //    if (!_pikachuRepository.Update(item, tran: transaction, dbConnection: conn))
                        //    {
                        //        throw new Exception("数据写入失败");
                        //    }
                        //}

                    }

                    _pikachuRepository.Delete_Mask<base_bom_detail>(ids, tran: transaction, dbConnection: conn);
                    transaction.Commit();
                    return data;
                }

                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        /// <summary>
        /// 遍历所有根节点获取数据
        /// </summary>
        /// <returns></returns>
        public List<base_bom> GetAllNodes()
        {
            //先获取父节点
            List<base_bom> rootNotes = _pikachuRepository.GetAllByParentId<base_bom>(null);
            List<base_bom> allNotes = rootNotes;
            if (rootNotes.Count != 0)
            {
                ///遍历所有根节点
                for (int i = rootNotes.Count - 1; i >= 0; i--)
                {
                    //搜索该根节点的子节点
                    List<base_bom> childrens = GetChildrens(rootNotes[i].parent_bom_id);
                    //加到返回列表
                    allNotes.AddRange(childrens);
                }
            }
            return allNotes;
        }

        public List<base_bom> GetBomTree(string ParentId)
        {
            List<base_bom> datas = new();
            base_bom _Bom = _pikachuRepository.GetById<base_bom>(ParentId);
            if (_Bom != null)
            {
                datas.Add(_Bom);
                List<base_bom> childrens = GetChildrens(ParentId);
                datas.AddRange(childrens);
            }

            return datas;

        }
        /// <summary>
        /// 递归搜索所有子节点
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public List<base_bom> GetChildrens(string ParentId)
        {
            List<base_bom> data = new();

            List<base_bom> childrens = _pikachuRepository.GetAllByParentId<base_bom>(ParentId);
            data.AddRange(childrens);
            if (childrens.Count != 0)
            {
                for (int i = childrens.Count - 1; i >= 0; i--)
                {
                    //搜索该节点的所有子节点
                    List<base_bom> childrenOfchildrens = GetChildrens(childrens[i].bom_id);
                    data.AddRange(childrenOfchildrens);
                }
            }

            return data;
        }
        public List<base_bom_response> GeBomChildrens(string ParentId)
        {
            List<base_bom_response> childrens = _baseBomRepository.QuertBomListByParentId(ParentId);
            if (childrens.Count != 0)
            {
                for (int i = childrens.Count - 1; i >= 0; i--)
                {
                    //搜索该节点的所有子节点
                    List<base_bom_response> childrenOfchildrens = GeBomChildrens(childrens[i].bom_id);
                    childrens.AddRange(childrenOfchildrens);
                }
            }
            return childrens;
        }
        /// <summary>
        /// 根据物料id获取bom列表
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="req"></param>
        /// <param name="icount"></param>
        /// <returns></returns>
        public List<base_bom_response> GetBomLiistByMaterialId(string materialId, PageReq req, ref long icount)
        {

            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;
            List<base_bom_response> data = _baseBomRepository.QuertBomListResponse(materialId, whereStr);
            icount = data != null ? data.Count : 0;
            //分页
            data = data != null ? data.Skip((iPage - 1) * iRows).Take(iRows).ToList() : null;

            return data;

        }



    }
}
