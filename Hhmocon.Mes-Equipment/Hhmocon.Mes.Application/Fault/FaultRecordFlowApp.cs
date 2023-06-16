
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.DataBase;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 事件记录流程App层
    /// </summary>
    public class FaultRecordFlowApp
    {
        private readonly IFaultRecordFlowRepository _faultRecordFlowRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        public FaultRecordFlowApp(IFaultRecordFlowRepository faultRecordFlowRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _faultRecordFlowRepository = faultRecordFlowRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新增事件记录流程
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public fault_record_flow InsertFaultFlowRecord(fault_record_flow data)
        {
            //取ID
            data.fault_record_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据异常记录id找到
        /// 已经完成的流程的处理人和处理时间
        /// </summary>
        /// <param name="fault_record_id"></param>
        /// <returns></returns>
        public FaultDealPeopleAndDealTime GetAlreadyExistsFlowByFaultRecordId(string fault_record_id)
        {
            List<fault_record_flow> fault_Record_Flow =
                _pikachuRepository.GetAll<fault_record_flow>().Where(c => c.fault_record_id == fault_record_id && c.flow_status == 1).ToList();
            fault_record_flow Confirm = fault_Record_Flow.Where(c => c.flow_seq == 0).FirstOrDefault();
            fault_record_flow Deal = fault_Record_Flow.Where(c => c.flow_seq == 1).FirstOrDefault();
            fault_record_flow Shut = fault_Record_Flow.Where(c => c.flow_seq == 2).FirstOrDefault();

            FaultDealPeopleAndDealTime data = new();
            data.ConfirmPeoPle = Confirm?.create_by_name;
            data.ConfirmTime = Confirm?.create_time.ToString();

            data.DealPeoPle = Deal?.create_by_name;
            data.DealTime = Deal?.create_time.ToString();

            data.ShutPeoPle = Shut?.create_by_name;
            data.ShutTime = Shut?.create_time.ToString();

            return data;
        }

        /// <summary>
        /// 根据异常记录id寻找未完成的流程并做处理
        /// </summary>
        /// <param name="fault_record_id"></param>
        /// <param name="operationCode"></param>
        /// <param name="flow_info"></param>
        /// <returns></returns>
        public bool DealFaultFlow(string fault_record_id, int operationCode, string flow_info)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            IDbTransaction transaction = conn.BeginTransaction();
            try
            {
                //找到异常记录未完成的流程
                List<fault_record_flow> fault_Record_Flows = _pikachuRepository.GetByOneFeildsSql<fault_record_flow>("fault_record_id", fault_record_id, tran: transaction, dbConnection: conn)
                    .Where(c => c.is_finish == 0).ToList();
                if (fault_Record_Flows.Count == 0)
                {
                    throw new Exception("异常记录没有未完成的流程");
                }

                fault_record_flow fault_Record_Flow = fault_Record_Flows.FirstOrDefault();
                //判断操作与当前未完成的流程是否相符
                switch (operationCode)
                {
                    case 1:
                        if (fault_Record_Flow.flow_seq != 1)
                        {
                            throw new Exception("当前流程已完成确认");
                        }

                        break;
                    case 2:
                        if (fault_Record_Flow.flow_seq!=2)
                        {
                            throw new Exception("请先完成确认或处理已经完成");
                        }

                        break;
                    case 3:
                        if (fault_Record_Flow.flow_seq!=3)
                        {
                            throw new Exception("请先确认或处理流程");
                        }

                        break;
                    default:
                        throw new Exception("操作与流程不对应");
                }

                fault_record fault_Record = _pikachuRepository.GetByOneFeildsSql<fault_record>("fault_record_id", fault_Record_Flow.fault_record_id, tran: transaction, dbConnection: conn).FirstOrDefault();
                if (fault_Record==null)
                {
                    fault_Record_Flow.delete_mark = 1;
                    _pikachuRepository.Update(fault_Record_Flow, tran: transaction, dbConnection: conn);
                    throw new Exception("查询到无效的记录,已删除对应的流程，请重试");
                }
                fault_Record_Flow.is_finish = 1;
                fault_Record_Flow.flow_status = 1;
                fault_Record_Flow.flow_end_time = Time.Now.ToString();
                fault_Record_Flow.modified_time = Time.Now;
                fault_Record_Flow.modified_by_name = _auth.GetUserName();
                fault_Record_Flow.modified_by = _auth.GetUserAccount();
                //fault_Record_Flow.flow_user_id//流程刷卡人员
                fault_Record_Flow.flow_info = flow_info;
              
                _pikachuRepository.Update(fault_Record_Flow, tran: transaction, dbConnection: conn);//更新流程

                fault_record_flow fault_Record_Flow1 = new();

                fault_Record_Flow1.create_time = Time.Now;
                fault_Record_Flow1.create_by = _auth.GetUserAccount();
                fault_Record_Flow1.create_by_name = _auth.GetUserName();
                fault_Record_Flow1.duration = 0;
                fault_Record_Flow1.delete_mark = 0;
                fault_Record_Flow1.fault_id = fault_Record_Flow.fault_id;
                fault_Record_Flow1.fault_record_flow_id = CommonHelper.GetNextGUID();
                fault_Record_Flow1.fault_record_id = fault_Record_Flow.fault_record_id;
                fault_Record_Flow1.flow_end_time = "流程未结束";
                fault_Record_Flow1.flow_info = "";

                fault_Record_Flow1.flow_start_time = Time.Now;
                fault_Record_Flow1.flow_status = 0;
                //fault_Record_Flow1.flow_user_id;//流程刷卡人员
                fault_Record_Flow1.is_finish = 0;
                fault_Record_Flow1.modified_time = Time.Now;
                fault_Record_Flow1.modified_by = _auth.GetUserAccount();
                fault_Record_Flow1.modified_by_name = _auth.GetUserName();
                switch (operationCode)
                {
                    case 1://完成确认流程
                        //生成处理流程
                        fault_Record_Flow1.flow_seq = 2;
                        conn.Insert(fault_Record_Flow1, tran: transaction);
                        break;
                    case 2://完成处理流程
                        //生成关闭流程
                        fault_Record_Flow1.flow_seq = 3;
                        conn.Insert(fault_Record_Flow1, tran: transaction);
                        break;
                    case 3://完成关闭流程，完成异常记录
                        string[] a = new string[1] { fault_record_id };
                        fault_Record.is_finish = 1;
                        _pikachuRepository.Update(fault_Record,tran: transaction, dbConnection: conn);
                        break;
                }
                transaction.Commit();
                return true;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new(exception.Message);
            }
        }


        /// <summary>
        /// 通过流程顺序为1且is_finish为0 的流程顺序 找到事件记录id
        /// </summary>
        /// <returns></returns>
        public List<string> GetRecordsByFlowSeq1(IDbConnection dbConnection, IDbTransaction transaction)
        {
            return _faultRecordFlowRepository.GetRecordsByFlowSeq1(dbConnection, transaction);
        }

        /// <summary>
        /// 通过流程顺序为1且is_finish为0 的流程顺序 找到流程
        /// </summary>
        /// <returns></returns>
        public List<fault_record_flow> GetFlowsByFlowSeq1(IDbConnection dbConnection, IDbTransaction transaction)
        {
            return _faultRecordFlowRepository.GetFlowsByFlowSeq1(dbConnection, transaction);
        }

    }
}
