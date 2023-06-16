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

using AutoMapper;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Application.Request;
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
    /// 出入库记录App
    /// </summary>
    public class WareHouseIoRecApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IMapper _mapper;
        private readonly IAuth _auth;
        public WareHouseIoRecApp(PikachuRepository pikachuRepository, PikachuApp pikachuApp, IMapper mapper, IAuth auth)
        {
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _mapper = mapper;
            _auth = auth;
        }

        /// <summary>
        /// 新增出入库记录
        /// </summary>
        /// <returns></returns>
        public bool Insert(warehouse_io_rec obj)
        {
            //取ID
            obj.warehouse_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);
            base_warehouse temp_base_Warehouse = _pikachuApp.GetById<base_warehouse>(obj.warehouse_id);
            base_warehouse_loc temp_base_Warehouse_loc = _pikachuApp.GetById<base_warehouse_loc>(obj.warehouse_loc_id);
            obj.warehouse_code = temp_base_Warehouse.warehouse_code;
            obj.warehouse_name = temp_base_Warehouse.warehouse_name;
            obj.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
            obj.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;
            return (_pikachuRepository.Insert(obj));
        }

        /// <summary>
        /// 出入库操作对库位的数量进行检验
        /// 入库
        /// 1代表小于最大值超过安全值
        /// 2代表 小于安全值
        /// 
        /// 出库
        /// 1代表出库后数量仍超过安全值
        /// 2代表出库后数量在0和安全值之间
        /// 
        /// 说明：出入库时前后端传的是记录，入库时只对库位做检查，然后根据物料id生成
        /// </summary>
        /// <param name="recs"></param>
        /// <returns></returns>
        public bool CheckIoWareHouseOperation(List<warehouse_io_rec> recs)
        {
            using IDbConnection conn = SqlServerDbHelper.GetConn();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            try
            {
                List<string> Loc_id = new();

                //遍历一遍，找出有多少不同的库位id
                foreach (warehouse_io_rec temp in recs)
                {
                    if (temp.rec_qty == 0)
                    {
                        throw new Exception("记录的数量不能为0");
                    }

                    if (temp.warehouse_loc_id == null || temp.warehouse_loc_id == "")
                    {
                        throw new Exception("请选择库位");
                    }

                    if (Loc_id.Contains(temp.warehouse_loc_id) == false)
                    {
                        Loc_id.Add(temp.warehouse_loc_id);
                    }
                }
                //loc_id:max_num,safe_num,current_num,
                Dictionary<string, int[]> keyValuePairs = new();
                //遍历库位id,用字典列表暂存当前库存
                foreach (string temp in Loc_id)
                {
                    base_warehouse_loc base_Warehouse_Loc = _pikachuApp.GetById<base_warehouse_loc>(temp, dbConnection: conn);
                    int current_num = base_Warehouse_Loc.current_num;
                    int[] num = new int[3];
                    num[0] = base_Warehouse_Loc.max_num;
                    num[1] = base_Warehouse_Loc.safety_num;
                    num[2] = base_Warehouse_Loc.current_num;
                    keyValuePairs.Add(temp, num);
                }

                //true代表没有超过安全值的  false代表有超过安全值的
                bool safe_flag = true;
                //遍历记录，对当前值执行出入库的加减操作
                foreach (warehouse_io_rec temp in recs)
                {
                    if (temp.rec_qty <= 0)
                    {
                        throw new Exception("数量必须为正整数");
                    }

                    if (temp.io_type == 0)//入库
                    {
                        keyValuePairs[temp.warehouse_loc_id][2] += temp.rec_qty;
                        //current_num>max_num
                        if (keyValuePairs[temp.warehouse_loc_id][2] > keyValuePairs[temp.warehouse_loc_id][0])
                        {
                            throw new Exception($"有库位变动后超过最大库存数量{keyValuePairs[temp.warehouse_loc_id][0]}，请检查");
                        }
                        //max_num>current_num>safe_num
                        if (keyValuePairs[temp.warehouse_loc_id][2] > keyValuePairs[temp.warehouse_loc_id][1])
                        {
                            safe_flag = false;
                        }
                    }
                    if (temp.io_type == 1)//出库
                    {
                        keyValuePairs[temp.warehouse_loc_id][2] -= temp.rec_qty;
                        if (keyValuePairs[temp.warehouse_loc_id][2] < 0)
                        {
                            throw new Exception("有库位出库总量超过当前已有库存");
                        }

                        //max_num>current_num>safe_num
                        if (keyValuePairs[temp.warehouse_loc_id][2] > keyValuePairs[temp.warehouse_loc_id][1])
                        {
                            safe_flag = false;
                        }
                    }
                }

                return safe_flag;
            }


            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }




        /// <summary>
        /// 出入库操作
        /// </summary>
        /// <param name="recs"></param>
        /// <returns></returns>
        public bool IoWareHouseOperation(List<warehouse_io_rec> recs)
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
                    foreach (warehouse_io_rec temp in recs)
                    {
                        //写入记录
                        temp.warehouse_io_rec_id = CommonHelper.GetNextGUID();
                        temp.modified_time = Time.Now;
                        temp.create_time = DateTime.Now;
                        temp.create_by = _auth.GetUserAccount();
                        temp.create_by_name = _auth.GetUserName();
                        base_warehouse temp_base_Warehouse = _pikachuApp.GetById<base_warehouse>(temp.warehouse_id);
                        base_warehouse_loc temp_base_Warehouse_loc = _pikachuApp.GetById<base_warehouse_loc>(temp.warehouse_loc_id);
                        temp.warehouse_code = temp_base_Warehouse.warehouse_code;
                        temp.warehouse_name = temp_base_Warehouse.warehouse_name;
                        temp.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
                        temp.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;
                        _pikachuRepository.Insert(temp, transaction, dbConnection: conn);

                        base_warehouse_loc base_Warehouse_Loc = new();
                        //根据记录的库位id
                        base_Warehouse_Loc = _pikachuRepository.GetById<base_warehouse_loc>(temp.warehouse_loc_id, tran: transaction, dbConnection: conn);

                        //********************************后期如果批次号和库位是一对一的话，可能需要再增加一个批次号************************
                        //查找该条记录的库位和物料  在库存表中有没有已经存在的库存记录
                        List<string> Feild2_Value = new();
                        Feild2_Value.Add(base_Warehouse_Loc.warehouse_loc_id);
                        //该物料在该库位下对应的库存
                        base_stock base_Stock = _pikachuRepository.GetByTwoFeildsSql<base_stock>("material_id", temp.material_id, "warehouse_loc_id", Feild2_Value, tran: transaction, dbConnection: conn).FirstOrDefault();
                        if (base_Stock == null)//如果 物料在库位下没有库存数据
                        {
                            if (temp.io_type == 1)//没有库存记录是不能出库的
                            {
                                throw new Exception("请先执行入库操作");
                            }

                            base_stock new_base_stock = new();  //在库存表中新增一条该库位下对应物料的库存记录
                            new_base_stock.stock_id = CommonHelper.GetNextGUID();
                            new_base_stock.material_id = temp.material_id;
                            new_base_stock.material_code = temp.material_code;
                            new_base_stock.material_name = temp.material_name;

                            new_base_stock.warehouse_loc_id = base_Warehouse_Loc.warehouse_loc_id;
                            new_base_stock.warehouse_loc_code = base_Warehouse_Loc.warehouse_loc_code;
                            new_base_stock.warehouse_loc_name = base_Warehouse_Loc.warehouse_loc_name;
                            new_base_stock.warehouse_id = temp.warehouse_id;
                            // new_base_stock.qty = temp.rec_qty;新增库存时数量在后面给

                            new_base_stock.create_time = Time.Now;
                            new_base_stock.modified_time = Time.Now;
                            new_base_stock.create_by = _auth.GetUserAccount(null);
                            new_base_stock.create_by_name = _auth.GetUserName(null);
                            new_base_stock.modified_by = _auth.GetUserAccount(null);
                            new_base_stock.modified_by_name = _auth.GetUserName(null);

                            _pikachuRepository.Insert(new_base_stock, tran: transaction, dbConnection: conn);

                            base_Stock = new_base_stock;
                        }

                        //根据记录类型对库存进行操作，并更新库位
                        if (temp.io_type == 0)//入库
                        {
                            //更改库存
                            base_Stock.qty += temp.rec_qty;
                            //更改库位
                            base_Warehouse_Loc.current_num += temp.rec_qty;
                        }
                        else if (temp.io_type == 1)//出库
                        {
                            //更改库存
                            base_Stock.qty -= temp.rec_qty;
                            if (base_Stock.qty < 0)
                            {
                                throw new Exception("当前库存数量不足，出库失败");
                            }
                            //更改库位
                            base_Warehouse_Loc.current_num -= temp.rec_qty;
                            if (base_Warehouse_Loc.current_num < 0)
                            {
                                throw new Exception("库位数量不足，出库失败");
                            }
                        }
                        //如果库存的数据不为0要更新
                        if (base_Stock.qty > 0)
                        {
                            //更新库存的数量
                            _pikachuRepository.Update(base_Stock, tran: transaction, dbConnection: conn);
                        }
                        else
                        {//为0要删除该库存
                            string[] ids = { base_Stock.stock_id };
                            _pikachuRepository.Delete_Mask<base_stock>(ids, tran: transaction, dbConnection: conn);
                        }

                        //更新库位的数量
                        _pikachuRepository.Update(base_Warehouse_Loc, tran: transaction, dbConnection: conn);


                    }
                    transaction.Commit();
                    return true;
                }

                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception(exception.Message);
                }

            }

        }

        /// <summary>
        /// 根据地点id或者仓库id获取出入库记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IOoperation">0入库 1出库 3我都要</param>
        /// <returns></returns>
        public List<warehouse_io_rec> GetIoWareHouseRec(string id, int IOoperation)
        {
            List<base_warehouse> base_Warehouses = new();
            List<warehouse_io_rec> warehouse_Io_Recs = new();
            List<base_warehouse> All_WareHouse = _pikachuRepository.GetAll<base_warehouse>();
            //获取id下的地点树
            List<base_location> base_Locations = _pikachuApp.GetRootAndBranch<base_location>(id, 1);
            //遍历地点，找地点下的仓库
            foreach (base_location temp_location in base_Locations)
            {
                List<base_warehouse> Warehouses_temp = All_WareHouse.Where(c => c.location_id == temp_location.location_id).ToList();
                if (Warehouses_temp.Count != 0)
                {
                    base_Warehouses.AddRange(Warehouses_temp);//找到的仓库添加进结果
                }
            }

            base_warehouse base_Warehouse = All_WareHouse.Where(c => c.warehouse_id == id).FirstOrDefault();
            if (base_Warehouse != null)
            {
                base_Warehouses.Add(base_Warehouse);
            }

            //遍历仓库，获取仓库的记录
            foreach (base_warehouse temp_warehouse in base_Warehouses)
            {
                List<warehouse_io_rec> temp_rec = _pikachuRepository.GetAll<warehouse_io_rec>().Where(c => c.warehouse_id == id).ToList();
                if (temp_rec.Count != 0)
                {
                    warehouse_Io_Recs.AddRange(temp_rec);//找到的记录添加进结果
                }
            }
            if (IOoperation != 3)
            {
                warehouse_Io_Recs = warehouse_Io_Recs.Where(c => c.io_type == IOoperation).ToList();
            }

            return warehouse_Io_Recs;

        }



        /// <summary>
        ///  调拨操作
        /// </summary>
        /// <param name="MoveRequests"></param>
        /// <param name="stock_id">选中的库存id</param>
        /// <returns></returns>
        public bool Move(List<MoveRequest> MoveRequests, string stock_id)
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
                    //根据记录id获取库存
                    base_stock rec = _pikachuRepository.GetById<base_stock>(stock_id);

                    //对调拨请求进行遍历，完成出库和入库
                    foreach (MoveRequest moveRequest in MoveRequests)
                    {
                        //对数值做检查
                        if (moveRequest.qty == 0)
                        {
                            throw new Exception("数量不能为0");
                        }
                        if (moveRequest.qty < 0)
                        {
                            throw new Exception("数量必须为正数");
                        }

                        //调拨必须是两个不同库位
                        if (moveRequest.old_warehouse_loc_id == moveRequest.new_warehouse_loc_id)
                        {
                            throw new Exception("调拨行为必须发生在两个不同库位之间");
                        }


                        //如果申请调拨的数量大于记录的数量
                        if (moveRequest.qty > rec.qty)
                        {
                            throw new Exception("申请调拨的数量大于记录的数量");
                        }

                        warehouse_io_rec warehouse_Io_Rec = new();
                        //warehouse_Io_Rec.barcode = rec.//条形码
                        warehouse_Io_Rec.create_by = _auth.GetUserAccount();
                        warehouse_Io_Rec.create_by_name = _auth.GetUserName();
                        //warehouse_Io_Rec.iowarehouse_type_id = rec.//出入库类型
                        warehouse_Io_Rec.material_code = rec.material_code;
                        warehouse_Io_Rec.material_id = rec.material_id;
                        //warehouse_Io_Rec.material_lot_no = rec.//物料批次号
                        warehouse_Io_Rec.material_name = rec.material_name;
                        //warehouse_Io_Rec.parm_id = rec.
                        //warehouse_Io_Rec.parm_name = 
                        warehouse_Io_Rec.rec_qty = moveRequest.qty;
                        warehouse_Io_Rec.io_type = 1;//生成旧库位的出库记录
                        warehouse_Io_Rec.op_type = "move";//操作为  “调拨”
                        warehouse_Io_Rec.warehouse_id = moveRequest.old_warehouse_id;
                        warehouse_Io_Rec.warehouse_io_rec_id = CommonHelper.GetNextGUID();
                        warehouse_Io_Rec.warehouse_loc_id = moveRequest.old_warehouse_loc_id;
                        warehouse_Io_Rec.create_time = Time.Now;
                        warehouse_Io_Rec.modified_time = Time.Now;
                        base_warehouse temp_base_Warehouse = _pikachuApp.GetById<base_warehouse>(warehouse_Io_Rec.warehouse_id);
                        base_warehouse_loc temp_base_Warehouse_loc = _pikachuApp.GetById<base_warehouse_loc>(warehouse_Io_Rec.warehouse_loc_id);
                        warehouse_Io_Rec.warehouse_code = temp_base_Warehouse.warehouse_code;
                        warehouse_Io_Rec.warehouse_name = temp_base_Warehouse.warehouse_name;
                        warehouse_Io_Rec.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
                        warehouse_Io_Rec.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;
                        _pikachuRepository.Insert(warehouse_Io_Rec, transaction, dbConnection: conn);//写入旧库位的出库记录
                        //获取旧库位数据
                        base_warehouse_loc base_Warehouse_Loc = _pikachuRepository.GetById<base_warehouse_loc>(moveRequest.old_warehouse_loc_id, tran: transaction, dbConnection: conn);

                        //更新旧库位的值
                        base_Warehouse_Loc.current_num -= moveRequest.qty;
                        if (base_Warehouse_Loc.current_num < 0)
                        {
                            throw new Exception($"{base_Warehouse_Loc.warehouse_loc_name}库位可用数量不足够,调拨操作失败");
                        }
                        else
                        {
                            _pikachuRepository.Update(base_Warehouse_Loc, tran: transaction, dbConnection: conn);
                        }

                        //更新旧库存的值
                        rec.qty -= moveRequest.qty;
                        if (rec.qty == 0)//数量移动后为0删库位
                        {
                            string[] ids = { rec.stock_id };
                            _pikachuRepository.Delete_Mask<base_stock>(ids, tran: transaction, dbConnection: conn);
                        }
                        else if (rec.qty < 0)
                        {
                            throw new Exception("调拨后原库存数量少于0");
                        }
                        else
                        {
                            rec.modified_time = Time.Now;
                            rec.modified_by = _auth.GetUserAccount();
                            rec.modified_by_name = _auth.GetUserName();
                            _pikachuRepository.Update(rec, tran: transaction, dbConnection: conn);
                        }


                        warehouse_Io_Rec.io_type = 0;//生成新库位的入库记录
                        warehouse_Io_Rec.op_type = "move";//操作为  “调拨”
                        warehouse_Io_Rec.warehouse_id = moveRequest.new_warehouse_id;
                        warehouse_Io_Rec.warehouse_io_rec_id = CommonHelper.GetNextGUID();
                        warehouse_Io_Rec.warehouse_loc_id = moveRequest.new_warehouse_loc_id;
                        warehouse_Io_Rec.create_time = Time.Now;
                        warehouse_Io_Rec.modified_time = Time.Now;
                        warehouse_Io_Rec.create_by = _auth.GetUserAccount();
                        warehouse_Io_Rec.create_by_name = _auth.GetUserName();
                        base_warehouse temp_base_Warehouse2 = _pikachuApp.GetById<base_warehouse>(warehouse_Io_Rec.warehouse_id);
                        base_warehouse_loc temp_base_Warehouse_loc2 = _pikachuApp.GetById<base_warehouse_loc>(warehouse_Io_Rec.warehouse_loc_id);
                        warehouse_Io_Rec.warehouse_code = temp_base_Warehouse2.warehouse_code;
                        warehouse_Io_Rec.warehouse_name = temp_base_Warehouse2.warehouse_name;
                        warehouse_Io_Rec.warehouse_loc_code = temp_base_Warehouse_loc2.warehouse_loc_code;
                        warehouse_Io_Rec.warehouse_loc_name = temp_base_Warehouse_loc2.warehouse_loc_name;
                        _pikachuRepository.Insert(warehouse_Io_Rec, transaction, dbConnection: conn);//写入新库位的入库记录
                        //获取目的库位数据
                        base_Warehouse_Loc = _pikachuRepository.GetById<base_warehouse_loc>(moveRequest.new_warehouse_loc_id, tran: transaction, dbConnection: conn);

                        base_Warehouse_Loc = _pikachuRepository.GetById<base_warehouse_loc>(moveRequest.new_warehouse_loc_id, tran: transaction, dbConnection: conn);
                        if (base_Warehouse_Loc == null)
                        {
                            throw new Exception("目标库位不能为空");
                        }

                        //检查目的仓库和目的库位是否匹配
                        if (base_Warehouse_Loc.warehouse_id != moveRequest.new_warehouse_id)
                        {
                            throw new Exception("目标仓库和目标库位不匹配，请检查");
                        }

                        //更新目的库位的值
                        base_Warehouse_Loc.current_num += moveRequest.qty;
                        if (base_Warehouse_Loc.current_num > base_Warehouse_Loc.safety_num)
                        {
                            throw new Exception($"{base_Warehouse_Loc.warehouse_loc_name}库存将会超过最大数量,调拨操作失败");
                        }
                        else
                        {
                            _pikachuRepository.Update(base_Warehouse_Loc, tran: transaction, dbConnection: conn);
                        }
                        //更新新库存的值
                        //检查库位下是否有该物料的库存
                        List<base_stock> base_Stocks = new();
                        base_Stocks = _pikachuRepository.GetByOneFeildsSql<base_stock>("material_id", moveRequest.material_id, tran: transaction, dbConnection: conn);
                        base_Stocks = base_Stocks.Where(c => c.warehouse_loc_id == base_Warehouse_Loc.warehouse_loc_id).ToList();

                        if (base_Stocks.Count == 0)//没有的话要新增
                        {
                            base_stock base_Stock = new();
                            base_Stock = rec;
                            base_Stock.stock_id = CommonHelper.GetNextGUID();
                            base_warehouse_loc base_Warehouse_Loc1 = new();
                            base_Warehouse_Loc1 = _pikachuRepository.GetById<base_warehouse_loc>(moveRequest.new_warehouse_loc_id, tran: transaction, dbConnection: conn);
                            base_Stock.warehouse_loc_id = base_Warehouse_Loc1.warehouse_loc_id;
                            base_Stock.warehouse_loc_name = base_Warehouse_Loc1.warehouse_loc_name;
                            base_Stock.warehouse_loc_code = base_Warehouse_Loc1.warehouse_loc_code;
                            base_Stock.qty = moveRequest.qty;//数量是调拨请求的数量
                            _pikachuRepository.Insert(base_Stock, tran: transaction, dbConnection: conn);
                        }
                        else//有的话要给新库位加数量
                        {
                            base_stock base_Stock = base_Stocks.FirstOrDefault();
                            base_warehouse_loc base_Warehouse_Loc1 = new();
                            base_Warehouse_Loc1 = _pikachuRepository.GetById<base_warehouse_loc>(moveRequest.new_warehouse_loc_id, tran: transaction, dbConnection: conn);
                            base_Stock.warehouse_loc_id = base_Warehouse_Loc1.warehouse_loc_id;
                            base_warehouse base_Warehouse = _pikachuRepository.GetById<base_warehouse>(base_Warehouse_Loc1.warehouse_id);
                            base_Stock.warehouse_id = base_Warehouse.warehouse_id;
                            base_Stock.warehouse_code = base_Warehouse.warehouse_code;
                            base_Stock.warehouse_loc_name = base_Warehouse_Loc1.warehouse_loc_name;
                            base_Stock.warehouse_loc_code = base_Warehouse_Loc1.warehouse_loc_code;
                            base_Stock.qty += moveRequest.qty;
                            _pikachuRepository.Update(base_Stock, tran: transaction, dbConnection: conn);
                        }

                    }

                    transaction.Commit();
                    return true;
                }

                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception(exception.Message);
                }
            }
        }


        /// <summary>
        /// 盘点操作
        /// </summary>
        /// <param name="stock_id">库存id</param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool CheckOperation(string stock_id, int num)
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
                    base_stock _Io_Stock = _pikachuApp.GetById<base_stock>(stock_id, tran: transaction, dbConnection: conn);

                    base_warehouse_loc base_Warehouse_Loc = _pikachuApp.GetById<base_warehouse_loc>(_Io_Stock.warehouse_loc_id, tran: transaction, dbConnection: conn);
                    if (num > base_Warehouse_Loc.max_num)
                    {
                        throw new Exception("数量超过库位最大值");
                    }
                    else if (num < 0)
                    {
                        throw new Exception("数量不能为负数");
                    }

                    base_Warehouse_Loc.current_num = num;
                    //更新库位
                    _pikachuApp.Update(base_Warehouse_Loc, tran: transaction, dbConnection: conn);
                    //更新库存
                    _Io_Stock.qty = num;
                    if (_Io_Stock.qty == 0)//库存数量为0要删除库存
                    {
                        _Io_Stock.delete_mark = 1;
                        conn.Update(_Io_Stock, tran: transaction);
                    }
                    else//库存不为0更新库存数量
                    {
                        _pikachuApp.Update(_Io_Stock, tran: transaction, dbConnection: conn);
                    }


                    warehouse_io_rec warehouse_Io_Rec = new();
                    warehouse_Io_Rec.warehouse_id = _Io_Stock.warehouse_id;
                    warehouse_Io_Rec.warehouse_loc_id = _Io_Stock.warehouse_loc_id;
                    //warehouse_Io_Rec.barcode = 
                    //warehouse_Io_Rec.iowarehouse_type_id = _Io_Stock.
                    //warehouse_Io_Rec.io_type
                    warehouse_Io_Rec.material_code = _Io_Stock.material_code;
                    warehouse_Io_Rec.material_id = _Io_Stock.material_id;
                    //warehouse_Io_Rec.material_lot_no = _Io_Stock.
                    warehouse_Io_Rec.material_name = _Io_Stock.material_name;
                    //warehouse_Io_Rec.parm_id = _Io_Stock.pa
                    //warehouse_Io_Rec.parm_name
                    warehouse_Io_Rec.rec_qty = _Io_Stock.qty;


                    warehouse_Io_Rec.warehouse_io_rec_id = CommonHelper.GetNextGUID();
                    warehouse_Io_Rec.op_type = "move";
                    warehouse_Io_Rec.create_by = _auth.GetUserAccount();
                    warehouse_Io_Rec.create_by_name = _auth.GetUserName();
                    warehouse_Io_Rec.create_time = Time.Now;
                    warehouse_Io_Rec.modified_time = Time.Now;
                    //新增盘点记录
                    _pikachuRepository.Insert(warehouse_Io_Rec, tran: transaction, dbConnection: conn);

                    transaction.Commit();
                    return true;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception(exception.Message);
                }
            }
        }

        /// <summary>
        /// 检查 参数 和 库位的 最大数量 与 安全数量
        /// </summary>
        /// <param name="warehouse_loc_id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int CheckNumberOfWareLoc(string warehouse_loc_id, int num)
        {
            base_warehouse_loc loc = _pikachuApp.GetById<base_warehouse_loc>(warehouse_loc_id);
            if (num < loc.max_num)
            {
                if (num < loc.safety_num)
                {
                    return 200;//正常添加
                }
                else
                {
                    return 100;//给警告，
                }
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="warehouse_loc_id"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public List<int> GetNumOfWareLoc(string warehouse_loc_id)
        {
            base_warehouse_loc loc = _pikachuApp.GetById<base_warehouse_loc>(warehouse_loc_id);
            List<int> data = new();
            data.Add(loc.safety_num);
            data.Add(loc.max_num);
            return data;
        }


    }
}
