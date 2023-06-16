using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 工艺路线规则App
    /// </summary>
    public class BaseProcessRouteDetailApp
    {
        private readonly IBaseProcessRouteDetailRepository _baseProcessRouteDetailRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseProcessRouteDetailApp(IBaseProcessRouteDetailRepository repository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseProcessRouteDetailRepository = repository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }
        /// <summary>
        /// 添加工艺路线规则数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Insert(base_process_route_detail data)
        {
            //取ID
            data.process_route_detail_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);

            //根据  物料id 工艺路线id 工序id  查找工艺路线规则中 符合的数据  ，填写工艺顺序
            List<string> process_route_ids = new();
            process_route_ids.Add(data.process_route_id);
            List<string> process_ids = new();
            process_ids.Add(data.process_route_id);

            List<base_process_route_detail> base_Process_Route_Details = _pikachuRepository.GetByTwoFeildsSql<base_process_route_detail>("material_id", data.material_id, "process_route_id", process_route_ids); // _pikachuRepository.GetByThreeFeildsSql<base_process_route_detail>("material_id",data.material_id,"process_route_id", process_route_ids, "process_id", process_ids);

            int seq_max = 1;
            foreach (base_process_route_detail temp in base_Process_Route_Details)
            {
                if (temp.process_seq.CompareTo(seq_max) > 0)
                {
                    seq_max = temp.process_seq;
                }
            }

            if (seq_max == 1)
            {
                data.process_seq = 10;
            }
            else
            {
                data.process_seq = seq_max + 10;
            }

            if (_pikachuRepository.Insert(data))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据工艺路线id获取所有工艺路线工序细节
        /// </summary>
        /// <param name="material_id"></param>
        /// <returns></returns>
        public List<ProcessRouteDetailResponse> GetProcessRouteDetailByProcessRouteId(string process_route_id)
        {
            List<base_process_route_detail> base_Process_Route_Detail = _pikachuRepository.GetByOneFeildsSql<base_process_route_detail>("process_route_id", process_route_id);
            if (base_Process_Route_Detail.Count == 0)
            {
                throw new Exception("找不到对应的工序细节");
            }

            List<ProcessRouteDetailResponse> processRouteDetailResponses = new();
            foreach (base_process_route_detail temp in base_Process_Route_Detail)
            {
                ProcessRouteDetailResponse processRouteDetailResponse = new();
                base_process base_Process = _pikachuRepository.GetById<base_process>(temp.process_id);
                if (base_Process == null)
                {
                    string[] a = new string[1] { temp.process_route_detail_id };
                    _pikachuRepository.Delete_Mask<base_process_route_detail>(a);
                    throw new Exception("找不到对应的工序,已经清除无效的关联数据，请重试");
                }
                processRouteDetailResponse.process_code = base_Process.process_code;
                processRouteDetailResponse.process_name = base_Process.process_name;
                processRouteDetailResponse.process_seq = temp.process_seq;

                processRouteDetailResponse.process_route_detail_id = temp.process_route_detail_id;

                processRouteDetailResponse.process_id = temp.process_id;
                processRouteDetailResponse.process_route_id = temp.process_route_id;
                processRouteDetailResponses.Add(processRouteDetailResponse);
            }
            return processRouteDetailResponses;
        }

        /// <summary>
        /// 根据物料id获取工艺路线细节
        /// </summary>
        /// <param name="material_id"></param>
        /// <returns></returns>
        public List<ProcessRouteDetailResponse> GetProcessRouteDetailByMaterialId(string material_id)
        {
            List<base_process_route_detail> base_Process_Route_Detail = _pikachuRepository.GetByOneFeildsSql<base_process_route_detail>("material_id", material_id);
            List<ProcessRouteDetailResponse> processRouteDetailResponses = new();
            foreach (base_process_route_detail temp in base_Process_Route_Detail)
            {
                ProcessRouteDetailResponse processRouteDetailResponse = new();
                base_process base_Process = _pikachuRepository.GetById<base_process>(temp.process_id);
                if (base_Process == null)
                {
                    string[] a = new string[1] { temp.process_route_detail_id};
                    _pikachuRepository.Delete_Mask<base_process_route_detail>(a);
                    throw new Exception("找不到工艺路线细节对应的工序,已将无效的关联数据清除,请重试");
                }
                processRouteDetailResponse.process_code = base_Process.process_code;
                processRouteDetailResponse.process_name = base_Process.process_name;
                processRouteDetailResponse.process_seq = temp.process_seq;

                processRouteDetailResponse.process_id = temp.process_id;
                processRouteDetailResponse.process_route_id = temp.process_route_id;
                processRouteDetailResponses.Add(processRouteDetailResponse);
            }
            return processRouteDetailResponses;
        }

        /// <summary>
        /// 删除工艺路线规则，同时更新所有规则的工序顺序
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Delete(string[] ids)
        {
            using (IDbConnection conn = SqlServerDbHelper.GetConn())
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                IDbTransaction tran = conn.BeginTransaction();
                try
                {
                    List<base_process_route_detail> base_Process_Route_Details = new();
                    base_Process_Route_Details = _pikachuRepository.GetAll<base_process_route_detail>(tran: tran, dbConnection: conn);

                    //删除
                    foreach (string id in ids)
                    {
                        List<string> _id = new();
                        _id.Add(id);
                        base_process_route_detail base_Process_Route_Detail = base_Process_Route_Details.Where(c => c.process_route_detail_id == id).FirstOrDefault();//_pikachuRepository.GetById<base_process_route_detail>(id, tran: tran, dbConnection: conn);
                        base_Process_Route_Detail.delete_mark = 1;
                        conn.Update(base_Process_Route_Detail, tran: tran);
                        base_Process_Route_Details.Remove(base_Process_Route_Detail);
                        //排序
                        foreach (base_process_route_detail temp in base_Process_Route_Details)
                        {
                            if (temp.process_seq > base_Process_Route_Detail.process_seq)//如果遍历到的位置大于被删除的位置
                            {
                                temp.process_seq -= 10;
                            }
                            _pikachuRepository.Update(temp, tran: tran, dbConnection: conn);
                        }
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    tran.Rollback();
                    throw new Exception(exception.Message);
                }
            }
        }


    }
}
