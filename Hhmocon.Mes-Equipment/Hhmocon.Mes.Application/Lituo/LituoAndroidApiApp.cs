using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.DataBase.SqlServer;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Repository;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 用于实现 Android 手持机相关接口函数
/// </summary>
namespace Hhmocon.Mes.Application.Lituo
{
    public class LituoAndroidApiApp
    {
        private LituoProductionTaskMainRepository _lituoProductionTaskMainRepository;
        private PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IBaseMaterialRepository _baseMaterialRepository;
        public LituoAndroidApiApp(LituoProductionTaskMainRepository lituoProductionTaskMainRepository, PikachuRepository pikachuRepository,
            BaseMaterialRepository baseMaterialRepository, PikachuApp pikachuApp)
        {
            _lituoProductionTaskMainRepository = lituoProductionTaskMainRepository;
            _pikachuRepository = pikachuRepository;
            _baseMaterialRepository = baseMaterialRepository;
            _pikachuApp = pikachuApp;
        }


        /// <summary>
        /// 取得Android 手持机需要的 物料列表
        /// </summary>
        /// <returns></returns>

        public List<base_material> GetMaterialList()
        {
            List<base_material> base_Materials = new List<base_material>();

            try
            {
                base_Materials.AddRange(_baseMaterialRepository.GetbyMaterialTypeCodes("JJWuLiao-001"));
                base_Materials.AddRange(_baseMaterialRepository.GetbyMaterialTypeCodes("LHBCP-001"));
                base_Materials.AddRange(_baseMaterialRepository.GetbyMaterialTypeCodes("****"));
            }
            catch
            {

            }
            return base_Materials;
        }


        public bool StockOperate(string username, string usercode, string kwcode, string pccode, string ioflag, string qty, string materialid)
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

                    base_warehouse_loc temp_base_Warehouse_loc = _pikachuApp.GetByCode<base_warehouse_loc>(kwcode);

                    if(temp_base_Warehouse_loc==null)
                    {
                        throw new Exception("当前库位号不存在，请联系系统管理员！");
                    }

                    base_warehouse temp_base_Warehouse = _pikachuApp.GetById<base_warehouse>(temp_base_Warehouse_loc.warehouse_id);
                    if (temp_base_Warehouse == null)
                    {
                        throw new Exception("当前库位号对应仓库不存在，请联系系统管理员！");
                    }

                    base_material base_Material = _pikachuApp.GetById<base_material>(materialid);
                    if (base_Material == null)
                    {
                        throw new Exception("当前物料信息不存在，请联系系统管理员！");
                    }


                    warehouse_io_rec temp = new warehouse_io_rec();
                    //foreach (warehouse_io_rec temp in recs)
                    {
                        //写入记录
                        temp.warehouse_io_rec_id = CommonHelper.GetNextGUID();
                        temp.modified_time = Time.Now;
                        temp.create_time = DateTime.Now;
                        temp.create_by = username;
                        temp.create_by_name = usercode;
          

                        temp.material_id = materialid;
                        temp.material_name = base_Material.material_name;
                        temp.material_code = base_Material.material_code;
                        temp.material_lot_no = pccode;
                        temp.io_type = int.Parse(ioflag);
                        if (ioflag == "0")
                        {
                            temp.op_type = "in";
                        }
                        else if (ioflag == "1")
                        {
                            temp.op_type = "out";
                        }

                        temp.warehouse_id = temp_base_Warehouse.warehouse_id;
                        temp.warehouse_loc_id = temp_base_Warehouse_loc.warehouse_loc_id;

                        temp.warehouse_code = temp_base_Warehouse.warehouse_code;
                        temp.warehouse_name = temp_base_Warehouse.warehouse_name;
                        temp.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
                        temp.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;

                        int itemp = 0;
                        int.TryParse(qty, out itemp);
                        temp.rec_qty = itemp;



                        _pikachuRepository.Insert(temp, transaction, dbConnection: conn);

                       // base_warehouse_loc base_Warehouse_Loc = new();
                        //根据记录的库位id
                       // base_Warehouse_Loc = _pikachuRepository.GetById<base_warehouse_loc>(temp.warehouse_loc_id, tran: transaction, dbConnection: conn);

                        //********************************后期如果批次号和库位是一对一的话，可能需要再增加一个批次号************************
                        //查找该条记录的库位和物料  在库存表中有没有已经存在的库存记录
                        List<string> Feild2_Value = new();
                        Feild2_Value.Add(temp_base_Warehouse_loc.warehouse_loc_id);
                        //该物料在该库位下对应的库存
                        base_stock base_Stock = _pikachuRepository.GetByTwoFeildsSql<base_stock>("material_id", temp.material_id, "warehouse_loc_id", Feild2_Value, tran: transaction, dbConnection: conn).FirstOrDefault();
                        if (base_Stock == null)//如果 物料在库位下没有库存数据
                        {
                            if (temp.io_type == 1)//没有库存记录是不能出库的
                            {
                                throw new Exception("当前库位下无库存，无法进行出库操作！");
                            }

                            base_stock new_base_stock = new();  //在库存表中新增一条该库位下对应物料的库存记录
                            new_base_stock.stock_id = CommonHelper.GetNextGUID();
                            new_base_stock.material_id = temp.material_id;
                            new_base_stock.material_code = temp.material_code;
                            new_base_stock.material_name = temp.material_name;

                            new_base_stock.warehouse_loc_id = temp_base_Warehouse_loc.warehouse_loc_id;
                            new_base_stock.warehouse_loc_code = temp_base_Warehouse_loc.warehouse_loc_code;
                            new_base_stock.warehouse_loc_name = temp_base_Warehouse_loc.warehouse_loc_name;
                            new_base_stock.warehouse_id = temp.warehouse_id;
                            // new_base_stock.qty = temp.rec_qty;新增库存时数量在后面给

                            new_base_stock.create_time = Time.Now;
                            new_base_stock.modified_time = Time.Now;
                            new_base_stock.create_by =usercode;
                            new_base_stock.create_by_name = username;
                            new_base_stock.modified_by = usercode;
                            new_base_stock.modified_by_name = username;

                            _pikachuRepository.Insert(new_base_stock, tran: transaction, dbConnection: conn);

                            base_Stock = new_base_stock;
                        }

                        //根据记录类型对库存进行操作，并更新库位
                        if (temp.io_type == 0)//入库
                        {
                            //更改库存
                            base_Stock.qty += temp.rec_qty;
                            //更改库位
                            temp_base_Warehouse_loc.current_num += temp.rec_qty;
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
                            temp_base_Warehouse_loc.current_num -= temp.rec_qty;

                            if (temp_base_Warehouse_loc.current_num < 0)
                            {
                                throw new Exception("库位中库存数量不足，出库失败");
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
                        _pikachuRepository.Update(temp_base_Warehouse_loc, tran: transaction, dbConnection: conn);


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


    }
}
