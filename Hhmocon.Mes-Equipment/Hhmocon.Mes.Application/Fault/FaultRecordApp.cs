/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Hhmocon.Mes.Application.Request;
using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 事件记录App层
    /// </summary>
    public class FaultRecordApp
    {
        private readonly IFaultRecordRepository _faultRecordRepository;
        private readonly IBaseFaultRepository _baseFaultRepository;
        private readonly IFaultRecordFlowRepository _faultRecordFlowRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IBaseEquipmentRepository _equipmentRepository;
        private readonly IAuth _auth;
        private readonly ILogger _logger;

        public FaultRecordApp(IFaultRecordRepository faultRecordRepository, PikachuRepository pikachuRepository,
            IBaseFaultRepository baseFaultRepository, IBaseEquipmentRepository equipmentRepository,
            IFaultRecordFlowRepository faultRecordFlowRepository, IAuth auth, ILogger<FaultRecordApp> logger)
        {
            _faultRecordRepository = faultRecordRepository;
            _pikachuRepository = pikachuRepository;
            _baseFaultRepository = baseFaultRepository;
            _equipmentRepository = equipmentRepository;
            _faultRecordFlowRepository = faultRecordFlowRepository;
            _auth = auth;
            _logger = logger;
        }


        /// <summary>
        /// 上报事件 ,生成事件记录的同时生成事件记录流程.
        /// 事件的设备名称可以不选
        /// </summary>
        /// <param name="data">上报事件只传入事件类型和事件名称,以及设备名称</param>
        /// <returns></returns>
        public bool InsertFaultRecord(List<FaultReportRequest> faultReportRequests)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            IDbTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable);
            //Console.WriteLine("ReportFault事务内层进入");
            try
            {
                foreach (FaultReportRequest faultReportRequest in faultReportRequests)
                {
                    //【生成事件记录】
                    fault_record fault_Record = new();
                    //取ID
                    fault_Record.fault_record_id = CommonHelper.GetNextGUID();
                    fault_Record.modified_time = Time.Now;
                    fault_Record.modified_by_name = _auth.GetUserName();
                    fault_Record.modified_by = _auth.GetUserAccount();
                    fault_Record.create_time = DateTime.Now;
                    fault_Record.create_by_name = _auth.GetUserName();
                    fault_Record.create_by = _auth.GetUserAccount();
                    //根据事件名称获取事件id
                    fault_Record.fault_id = _pikachuRepository.GetByOneFeildsSql<base_fault>("fault_name", faultReportRequest.fault_name, dbConnection: conn, tran: transaction).FirstOrDefault()?.fault_id;
                    fault_Record.fault_duration = 0;
                    fault_Record.notice_status = 0;
                    base_equipment base_Equipment = _equipmentRepository.GetByName(faultReportRequest.equipment_name);
                    if (base_Equipment == null)
                    {
                        throw new Exception("请选中设备");
                    }
                    //根据设备名称获取设备id
                    fault_Record.equipment_id = base_Equipment.equipment_id;

                    //【生成事件流程，刚上报的事件，事件流程的流程顺序为"确认"，流程状态为0处理中】
                    fault_record_flow fault_Record_Flow = new();
                    fault_Record_Flow.fault_record_flow_id = CommonHelper.GetNextGUID();
                    fault_Record_Flow.fault_record_id = fault_Record.fault_record_id;
                    fault_Record_Flow.fault_id = fault_Record.fault_id;
                    fault_Record_Flow.flow_start_time = Time.Now;
                    fault_Record_Flow.flow_user_id = fault_Record.create_by_name;//流程刷卡人员
                    fault_Record_Flow.modified_time = Time.Now;
                    fault_Record_Flow.flow_seq = 1;//初次上报的事件流程都为 1确认
                    fault_Record_Flow.flow_end_time = "流程未结束";
                    fault_Record_Flow.create_time = Time.Now;
                    fault_Record_Flow.create_by = _auth.GetUserAccount();
                    fault_Record_Flow.create_by_name = _auth.GetUserName();
                    fault_Record_Flow.modified_by = _auth.GetUserAccount();
                    fault_Record_Flow.modified_by_name = _auth.GetUserName();
                    fault_Record_Flow.duration = 0;
                    fault_Record_Flow.flow_status = 0;
                    fault_Record_Flow.notice_flag = 0;
                    //
                    //Console.WriteLine("上报开始写入关键数据");
                    //插入事件记录
                    _faultRecordRepository.Insert(fault_Record, conn, transaction);
                    //插入事件流程
                    _faultRecordFlowRepository.Insert(fault_Record_Flow, conn, transaction);
                    Console.WriteLine("上报关键数据写入完成");
                }
                transaction.Commit();
                //Console.WriteLine("ReportFault事务内层出");
                return true;
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw new(exception.Message);
            }
        }

        /// <summary>
        /// 获取所有的事件记录
        /// </summary>
        /// <returns></returns>
        public List<fault_record> GetAllFaultRecord(IDbTransaction tran = null, IDbConnection dbConnection = null)
        {
            return _pikachuRepository.GetAll<fault_record>(tran: tran, dbConnection: dbConnection);
        }

        /// <summary>
        /// 根据设备id获取事件记录
        /// </summary>
        /// <param name="equipment_id">设备id</param>
        /// <returns></returns>
        public List<fault_record> GetFaultRecordByEquipmentId(string equipment_id,IDbConnection dbConnection= null, IDbTransaction transaction = null)
        {
            return _faultRecordRepository.GetFaultRecordByEquipmentId(equipment_id,dbConnection:dbConnection,transaction:transaction);
        }

        /// <summary>
        /// 根据设备id查询事件记录
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public List<FaultRecordResponse> GetAll(string equipment_id)
        {
            List<FaultRecordResponse> faultRecordRequests = new();
            if (equipment_id == null)
            {
                //根据设备Id获取所有事件记录
                List<fault_record> fault_records = GetFaultRecordByEquipmentId(equipment_id);
                //遍历事件记录，页面请求类获取数据
                foreach (fault_record temp in fault_records)
                {
                    FaultRecordResponse faultRecordRequest = new();
                    base_equipment equipment = _pikachuRepository.GetById<base_equipment>(temp.equipment_id);
                    //事件的设备名
                    faultRecordRequest.equipment_name = equipment.equipment_name;
                    //事件的异常类型
                    base_fault base_Fault = _pikachuRepository.GetById<base_fault>(temp.fault_id);
                    string fault_class_id = base_Fault.fault_class_id;
                    base_fault_class base_Fault_Class = _pikachuRepository.GetById<base_fault_class>(fault_class_id);
                    faultRecordRequest.fault_class = base_Fault_Class.fault_class_name;

                    //所有异常事件流程
                    List<fault_record_flow> fault_Record_Flows = _pikachuRepository.GetAll<fault_record_flow>();

                    //查询事件的当前流程
                    fault_record_flow fault_Record_Flow = fault_Record_Flows.Where(c => ((c.fault_record_id == temp.fault_record_id) && c.flow_status == 0 && c.is_finish == 0)).FirstOrDefault();

                    //事件的异常名
                    faultRecordRequest.fault_name = base_Fault.fault_name;
                    //事件的流程状态
                    faultRecordRequest.flow_status = fault_Record_Flow.flow_status;// "处理中";

                    //根据事件的创建名获取上报人
                    faultRecordRequest.report_people = base_Fault.create_by_name;

                    faultRecordRequests.Add(faultRecordRequest);
                }
            }

            return faultRecordRequests;

        }

        /// <summary>
        /// 递归 记录
        /// 传入 事件类型 找到 下面的 所有事件记录
        /// </summary>
        public void Recurrence(List<base_fault_class> fault_Classes, DateTime StartTime, DateTime EndTime, ref List<GetProductsNumberByTimeDonotChartResponse> GResponse, base_fault_class FaultClass, ref List<string> linksTargets)
        {
            //遍历类型
            foreach (base_fault_class base_Fault_Class in fault_Classes)
            {
                //找到类型对应的记录
                List<fault_record> fault_Records = _faultRecordRepository.GetByTimeScopeAndFaultClass(StartTime, EndTime, base_Fault_Class);

                //数量统计
                GResponse.ForEach(delegate (GetProductsNumberByTimeDonotChartResponse c)
                {
                    //必须是最外层的类型名
                    if (c.name == FaultClass.fault_class_name)
                    {
                        c.value += fault_Records.Count;

                    }
                });

                //根据parentid字段找到所有孩子
                List<base_fault_class> son_Classes = _faultRecordRepository.GetAllByParent(base_Fault_Class);

                //制作LinkTarget
                foreach (base_fault_class temp in son_Classes)
                {
                    linksTargets.Add(temp.fault_class_name);
                }

                //孩子数统计
                GResponse.ForEach(delegate (GetProductsNumberByTimeDonotChartResponse c)
                {
                    //必须是最外层的类型名
                    if (c.name == base_Fault_Class.fault_class_name)
                    {
                        c.value += son_Classes.Count;

                    }
                });
                Recurrence(son_Classes, StartTime, EndTime, ref GResponse, FaultClass, ref linksTargets);
            }
        }


        /// <summary>
        /// 递归 如果有父结点，就把关系添加进LinkTarget[]和SourceTarget[],并填写Value
        /// </summary>
        public void RecurrenceFromSon(base_fault_class fault_Classes, base_fault_class fault_Classes_former, ref FaultRecordStaticsResponse faultRecordStaticsResponse, ref Dictionary<string, int> faultBaseAndClass, int gap_value)
        {
            //如果最底层的类型有父类型
            if (!string.IsNullOrEmpty(fault_Classes_former.fault_class_parentname))
            {
                //获取父类型
                base_fault_class base_Fault_Class = _pikachuRepository.GetById<base_fault_class>(fault_Classes.fault_class_parentid);

                //就在Link数组对应位置为它们创建关系
                faultRecordStaticsResponse.LinksTarget.Add(fault_Classes_former.fault_class_name);//子类名
                faultRecordStaticsResponse.LinksSource.Add(fault_Classes.fault_class_name);//父类名
                //补充类统计 中 对应父类型的数量
                faultBaseAndClass[$"{fault_Classes.fault_class_name}"] += gap_value;
                faultRecordStaticsResponse.value.Add(gap_value);//父-子关系数 = 子数


                RecurrenceFromSon(base_Fault_Class, fault_Classes, ref faultRecordStaticsResponse, ref faultBaseAndClass, gap_value);
            }
        }

        /// <summary>
        /// 递归 如果有父结点，就把关系添加进LinkTarget[]和SourceTarget[],并填写Value
        /// </summary>
        public void RecurrenceFromSon(base_fault_class fault_Classes, base_fault fault_former, ref FaultRecordStaticsResponse faultRecordStaticsResponse, ref Dictionary<string, int> faultBaseAndClass)
        {

            base_fault_class base_Fault_Class = _pikachuRepository.GetById<base_fault_class>(fault_former.fault_class_id);

            //如果最底层的类型或者base有父类型   
            if (!string.IsNullOrEmpty(base_Fault_Class.fault_class_parentid))
            {
                base_fault_class base_Fault_ClassParent = _pikachuRepository.GetById<base_fault_class>(base_Fault_Class.fault_class_parentid);

                //获取类型
                //就在Link数组对应位置为它们创建关系
                faultRecordStaticsResponse.LinksTarget.Add(fault_former.fault_name);//子类名
                faultRecordStaticsResponse.LinksSource.Add(base_Fault_Class.fault_class_name);//父类名
                int gap_value = faultBaseAndClass[$"{fault_Classes.fault_class_name}"];
                //补充类统计 中 对应类型的数量
                faultBaseAndClass[$"{fault_Classes.fault_class_name}"] += faultBaseAndClass[$"{fault_former.fault_name}"];
                gap_value = faultBaseAndClass[$"{fault_Classes.fault_class_name}"] - gap_value;

                //将父类型的枝条数加进response
                faultRecordStaticsResponse.value.Add(faultBaseAndClass[$"{fault_former.fault_name}"]);//父-子关系数 = 子数

                if (!faultRecordStaticsResponse.DataName.Where(c => c == base_Fault_ClassParent.fault_class_name).Any())
                {
                    faultRecordStaticsResponse.DataName.Add(base_Fault_ClassParent.fault_class_name);
                }
                if (!faultRecordStaticsResponse.DataName.Where(c => c == fault_Classes.fault_class_name).Any())
                {
                    faultRecordStaticsResponse.DataName.Add(fault_Classes.fault_class_name);
                }

                RecurrenceFromSon(base_Fault_ClassParent, fault_Classes, ref faultRecordStaticsResponse, ref faultBaseAndClass, gap_value);
            }
            //base有类型
            else if (base_Fault_Class != null)
            {
                //获取类型
                //就在Link数组对应位置为它们创建关系
                faultRecordStaticsResponse.LinksTarget.Add(fault_former.fault_name);//子类名
                faultRecordStaticsResponse.LinksSource.Add(base_Fault_Class.fault_class_name);//父类名
                int gap_value = faultBaseAndClass[$"{fault_Classes.fault_class_name}"];
                //补充类统计 中 对应类型的数量
                faultBaseAndClass[$"{fault_Classes.fault_class_name}"] += faultBaseAndClass[$"{fault_former.fault_name}"];
                gap_value = faultBaseAndClass[$"{fault_Classes.fault_class_name}"] - gap_value;

                //将父类型的枝条数加进response
                faultRecordStaticsResponse.value.Add(faultBaseAndClass[$"{fault_former.fault_name}"]);//父-子关系数 = 子数

                if (!faultRecordStaticsResponse.DataName.Where(c => c == fault_Classes.fault_class_name).Any())
                {
                    faultRecordStaticsResponse.DataName.Add(fault_Classes.fault_class_name);
                }
            }

        }

        /// <summary>
        /// 异常事件统计页面的飘带图
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public FaultRecordStaticsResponse GetProductsNumberByTimeRibbonChartApp(DateTime StartTime, DateTime EndTime)
        {
            try
            {
                //创建Response对象
                FaultRecordStaticsResponse RibbonChartResponses = new();
                RibbonChartResponses.value = new();
                RibbonChartResponses.LinksTarget = new();
                RibbonChartResponses.LinksSource = new();
                RibbonChartResponses.DataName = new();

                //如果查询的开始时间大于结束时间要报错
                if (DateTime.Compare(StartTime, EndTime) > 0)
                {
                    throw new Exception("对不合格事件查询的开始时间不能迟于结束时间");
                }
                //找到所有事件类型
                List<base_fault_class> base_Fault_Classes = _pikachuRepository.GetAll<base_fault_class>();

                //遍历所有基础事件
                List<base_fault> base_Faults = _pikachuRepository.GetAll<base_fault>();
                //用来统计 异常事件和异常类型下的异常记录 的数量 
                Dictionary<string, int> faultBaseAndClass = new();
                foreach (base_fault base_Fault in base_Faults)
                {
                    faultBaseAndClass.Add($"{base_Fault.fault_name}", 0);//初值给0
                    RibbonChartResponses.DataName.Add(base_Fault.fault_name);
                }
                //用来统计 异常事件下的异常记录 的数量 
                Dictionary<string, int> faultBase = new();
                faultBase = new Dictionary<string, int>(faultBaseAndClass);

                //遍历所有事件类型
                foreach (base_fault_class base_Fault_class in base_Fault_Classes)
                {
                    faultBaseAndClass.Add($"{base_Fault_class.fault_class_name}", 0);//初值给0
                }


                List<fault_record> fault_Records = _pikachuRepository.GetAll<fault_record>();
                //遍历所有异常记录 统计 异常事件下的记录数量
                foreach (fault_record fault_Record in fault_Records)
                {
                    base_fault base_Fault = _pikachuRepository.GetById<base_fault>(fault_Record.fault_id);
                    faultBaseAndClass[$"{base_Fault.fault_name}"] += 1;
                    faultBase[$"{base_Fault.fault_name}"] += 1;
                }

                //从最底层的异常事件开始找起，如果parentiid!=null
                //就向sourceLink[]和targetLink[]添加一组关系，往value添加数量
                foreach (KeyValuePair<string, int> Base in faultBase)
                {
                    //根据事件名称找到异常事件
                    base_fault base_Fault = _pikachuRepository.GetByOneFeildsSql<base_fault>("fault_name", Base.Key).FirstOrDefault();
                    if (base_Fault == null)
                    {
                        throw new Exception($"未能找到{Base.Key}对应数据");
                    }


                    base_fault_class base_Fault_Class = _pikachuRepository.GetById<base_fault_class>(base_Fault.fault_class_id);
                    //递归 填补脉络上的所有关系
                    RecurrenceFromSon(base_Fault_Class, base_Fault, ref RibbonChartResponses, ref faultBaseAndClass);
                }

                //将link数组中重复的内容合并，value相加
                if (RibbonChartResponses.LinksSource.Count != RibbonChartResponses.LinksTarget.Count)
                {
                    throw new Exception("LINK坐标未能对齐，统计失败");
                }
                int icount = RibbonChartResponses.LinksSource.Count;
                List<string> LinkSourceCopy = new(RibbonChartResponses.LinksSource.ToArray());
                List<string> LinkTargetCopy = new(RibbonChartResponses.LinksTarget.ToArray());
                List<int> valueCopy = new(RibbonChartResponses.value.ToArray());
                //清空原数据
                RibbonChartResponses.LinksSource = new();
                RibbonChartResponses.LinksTarget = new();
                RibbonChartResponses.value = new();
                //初始化 对于只有一条关系的情况直接加进去
                if (LinkSourceCopy.Count == 1 && LinkTargetCopy.Count == 1 && valueCopy.Count == 1)
                {
                    RibbonChartResponses.LinksSource.Add(LinkSourceCopy[0]);
                    RibbonChartResponses.LinksTarget.Add(LinkTargetCopy[0]);
                    RibbonChartResponses.value.Add(valueCopy[0]);
                }
                else if (LinkSourceCopy.Count != 0 && LinkTargetCopy.Count != 0 && valueCopy.Count != 0)
                {
                    for (int i = 0; i < valueCopy.Count; i++)
                    {
                        for (int j = i + 1; j < valueCopy.Count; j++)
                        {
                            //如果一样，就把数量加起来
                            if (LinkSourceCopy[i] == LinkSourceCopy[j]
                                &&
                                LinkTargetCopy[i] == LinkTargetCopy[j])
                            {
                                valueCopy[i] += valueCopy[j];
                                LinkSourceCopy.RemoveAt(j);
                                LinkTargetCopy.RemoveAt(j);
                                valueCopy.RemoveAt(j);
                                i = 0;
                                j = 0;
                            }
                        }
                    }
                    RibbonChartResponses.LinksSource.AddRange(LinkSourceCopy);
                    RibbonChartResponses.LinksTarget.AddRange(LinkTargetCopy);
                    RibbonChartResponses.value.AddRange(valueCopy);


                }


                List<int> Value_Copy = new(RibbonChartResponses.value);
                int Count = 0;
                Count = Value_Copy.Count;
                //找到数量为0的关系，清掉
                for (int i = 0; i < Count; i++)
                {
                    if (RibbonChartResponses.value[i] == 0)
                    {
                        RibbonChartResponses.LinksSource.RemoveAt(i);
                        int index = RibbonChartResponses.DataName.IndexOf(RibbonChartResponses.LinksTarget[i]);
                        RibbonChartResponses.DataName.RemoveAt(index);
                        RibbonChartResponses.LinksTarget.RemoveAt(i);

                        RibbonChartResponses.value.RemoveAt(i);

                        Count = RibbonChartResponses.value.Count;
                        i--;
                    }
                }

                return RibbonChartResponses;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw new Exception(exception.InnerException?.Message ?? exception.Message);
            }
        }

        public List<FaultRecordResponse> FaultRecordRequest(string equipment_id)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            IDbTransaction transaction = conn.BeginTransaction();
            //Console.WriteLine("FaultRecordRequest事件内层进入");
            try
            {
                List<fault_record> fault_Records = new();
                List<FaultRecordResponse> faultRecordResponses = new();
                if (equipment_id == null)
                {
                    //Console.WriteLine("请求开始读取关键数据");
                    fault_Records = GetAllFaultRecord(dbConnection:conn, tran: transaction);
                    //Console.WriteLine("请求关键数据读取完成");
                }
                else
                {
                    //Console.WriteLine("请求开始读取关键数据");
                    fault_Records = GetFaultRecordByEquipmentId(equipment_id,  dbConnection: conn, transaction: transaction);
                    //Console.WriteLine("请求关键数据读取完成");
                }
                if (fault_Records.Count != 0)
                {
                    foreach (fault_record temp in fault_Records)
                    {
                        FaultRecordResponse a = new();
                        base_equipment base_Equipment = _pikachuRepository.GetById<base_equipment>(temp.equipment_id,dbConnection:conn,tran:transaction);
                        base_fault base_Fault = _pikachuRepository.GetById<base_fault>(temp.fault_id,dbConnection:conn, tran: transaction);
                        if (base_Fault == null)
                        {
                            throw new Exception($"未能根据基础事件id{temp.fault_id}找到对应基础事件");
                        }
                        base_fault_class base_Fault_Class = _pikachuRepository.GetById<base_fault_class>(base_Fault.fault_class_id,dbConnection:conn, tran: transaction);
                        //根据事件记录id找到所有相关流程，最多找到三条，确认的状态1，处理的状态1，关闭的状态1
                        List<fault_record_flow> fault_Record_Flows = _pikachuRepository.GetByOneFeildsSql<fault_record_flow>("fault_record_id", temp.fault_record_id,dbConnection:conn, tran: transaction);
                        //要返回给前端的流程   先找未完成的流程,最多只有一条记录，最少0条记录(关闭的状态0时)
                        fault_record_flow fault_Record_Flow = fault_Record_Flows.Where(c => c.is_finish == 0).FirstOrDefault(); ;//要返回给前端的流程
                        if (fault_Record_Flow == null && temp.is_finish == 0)
                        {
                            throw new Exception($"设备{base_Equipment.equipment_name}的异常事件{base_Fault.fault_name}未能正确生成流程");
                        }
                        if (fault_Record_Flow == null)//找不到未完成的流程
                        {
                            fault_Record_Flow = fault_Record_Flows.Where(c => c.flow_seq == 3 && c.is_finish == 1).FirstOrDefault();//代表关闭已经状态1
                        }

                        a.fault_record_id = fault_Record_Flow.fault_record_id;//返回给前端的事件记录id
                        a.fault_name = base_Fault.fault_name;
                        a.equipment_name = base_Equipment.equipment_name;
                        a.flow_seq = fault_Record_Flow.flow_seq;//当前流程
                        a.fault_class = base_Fault_Class.fault_class_name;
                        a.flow_status = fault_Record_Flow.flow_status;//流程状态
                        DateTime now = Time.Now;
                        //间隔事件的年份以2022年为基准
                        TimeSpan timeSpan = Time.Now - temp.create_time;
                        if (temp.notice_status == 0)//如果事件未被处理
                        {

                            //一天是86400秒，30天封顶
                            a.fault_duration = timeSpan.TotalSeconds.ToInt() <= 8640000 ? timeSpan.TotalSeconds.ToInt() : 8640000;//异常持续时间

                        }
                        else//如果事件已经被处理
                        {
                            a.fault_duration = temp.fault_duration;
                        }

                        if (fault_Record_Flow.flow_status == 0)
                        {
                            a.flow_duration = timeSpan.TotalSeconds.ToInt() <= 8640000 ? timeSpan.TotalSeconds.ToInt() : 8640000;//流程持续时间

                        }
                        else
                        {
                            a.flow_duration = fault_Record_Flow.duration;
                        }
                        a.report_people = temp.create_by_name;//上报人
                        a.create_time = temp.create_time;//异常记录创建时间
                        //流程id
                        faultRecordResponses.Add(a);
                    }
                }
                //Console.WriteLine("FaultRecordRequest事件内层出");
                return faultRecordResponses;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                _logger.LogError($"{e.InnerException?.Message??e.Message}");
                throw new(e.Message);
            }

        }
    }
}
