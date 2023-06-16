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
using Hhmocon.Mes.Application.Request;
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
    /// 仓库位置App
    /// </summary>
    public class WareHouseLocApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly IMapper _mapper;
        private readonly IAuth _auth;
        private readonly IWareHouseLocRepository _wareHouseLocRepository;
        public WareHouseLocApp(PikachuRepository pikachuRepository, IMapper mapper, IAuth auth, IWareHouseLocRepository wareHouseLocRepository)
        {
            _pikachuRepository = pikachuRepository;
            _mapper = mapper;
            _auth = auth;
            _wareHouseLocRepository = wareHouseLocRepository;
        }
        /// <summary>
        /// 新增库位
        /// </summary>
        /// <returns></returns>
        public bool Insert(WareHouseLocRequest obj)
        {
            //对name和code进行查重
            List<base_warehouse_loc> exists =
            _pikachuRepository.GetAll<base_warehouse_loc>().Where(c =>
                c.warehouse_loc_name == obj.warehouse_loc_name
                 || c.warehouse_loc_code == obj.warehouse_loc_code).ToList();
            if (exists.Count > 0)
            {
                throw new Exception("名称或编码重复");
            }


            base_warehouse_loc base_Warehouse_Loc = new();
            //取ID
            obj.warehouse_loc_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);
            //根据仓库name获取仓库id
            obj.warehouse_id = _pikachuRepository.GetAll<base_warehouse>().Where(c => c.warehouse_name == obj.warehouse_name).FirstOrDefault().warehouse_id;

            _mapper.Map(obj, base_Warehouse_Loc);

            return (_pikachuRepository.Insert(base_Warehouse_Loc));
        }

        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="chartName"></param>
        /// <returns></returns>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            _wareHouseLocRepository.CheckChartIfExistsData(ref referenceCharts, id, chartName);
        }

        /// <summary>
        /// 删除库位
        /// </summary>
        public void deleteWarehouseLoc(string[] ids, IDbConnection dbConnection = null)
        {
            _wareHouseLocRepository.deleteWarehouseLoc(ids, dbConnection);
        }

    }
}
