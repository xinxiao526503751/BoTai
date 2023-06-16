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

using Hhmocon.Mes.Application.Base;
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
    /// 仓库App
    /// </summary>
    public class WareHouseApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IAuth _auth;
        private readonly IWareHouseRepository _wareHouseRepository;
        public WareHouseApp(PikachuRepository pikachuRepository, PikachuApp pikachuApp, IAuth auth, WareHouseRepository wareHouseRepository)
        {
            _pikachuRepository = pikachuRepository;
            _pikachuApp = pikachuApp;
            _auth = auth;
            _wareHouseRepository = wareHouseRepository;
        }

        /// <summary>
        /// 新增仓库
        /// </summary>
        /// <returns></returns>
        public bool Insert(base_warehouse obj)
        {
            //对name和code进行查重
            List<base_warehouse> exists =
            _pikachuRepository.GetAll<base_warehouse>().Where(c =>
                c.warehouse_name == obj.warehouse_name
                 || c.warehouse_code == obj.warehouse_code).ToList();
            if (exists.Count > 0)
            {
                throw new Exception("name或code重复");
            }

            //取ID
            obj.warehouse_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);
            return (_pikachuRepository.Insert(obj));
        }


        /// <summary>
        /// 地点仓库树
        /// </summary>
        /// <returns></returns>
        public List<TreeEasy> LocationWareHouseTree()
        {
            //节点列表
            List<TreeModel> Nodes = new();
            //获取location
            List<base_location> list_location = _pikachuApp.GetRootAndBranch<base_location>(null);
            //转化为node
            Nodes = _pikachuApp.ListElementToNode(list_location);
            foreach (TreeModel locationNode in Nodes)
            {
                locationNode.NodeType = "location";
            }

            //获取所有仓库，仓库没有父子级关系
            List<base_warehouse> list_warehouse = _pikachuApp.GetAll<base_warehouse>();
            //转化为Node
            List<TreeModel> warehouse_nodes = _pikachuApp.ListElementToNode(list_warehouse);
            foreach (TreeModel warehouse_node in warehouse_nodes)
            {
                warehouse_node.NodeType = "warehouse";
            }

            //找到parentid为null的仓库节点，将Parentid改为locationid用以衔接
            foreach (TreeModel temp in warehouse_nodes)
            {
                if (string.IsNullOrEmpty(temp.parentId))
                {
                    temp.parentId = list_warehouse.Where(c => c.warehouse_name.Equals(temp.label)).FirstOrDefault().location_id;
                }
            }
            //并添加进节点列表
            Nodes.AddRange(warehouse_nodes);

            //获取所有的库位，库位没有父子级关系
            List<base_warehouse_loc> base_Warehouse_Locs = _pikachuApp.GetAll<base_warehouse_loc>();
            //转化为Node
            List<TreeModel> loc_nodes = _pikachuApp.ListElementToNode(base_Warehouse_Locs);
            foreach (TreeModel loc_node in loc_nodes)
            {
                loc_node.NodeType = "warehouse_loc";
            }
            //找到parentid为null的库位节点，将Parentid改为warehouse_id用以衔接
            foreach (TreeModel temp in loc_nodes)
            {
                if (string.IsNullOrEmpty(temp.parentId))
                {
                    temp.parentId = base_Warehouse_Locs.Where(c => c.warehouse_loc_name.Equals(temp.label)).FirstOrDefault().warehouse_id;
                }
            }
            //并添加进节点列表
            Nodes.AddRange(loc_nodes);

            //将节点列表转化为树
            List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(Nodes, null);

            //将树转化为TreeEasy树
            List<TreeEasy> easies = _pikachuApp.TreeModelToTreeEasy(treeModels);
            return easies;
        }

        /// <summary>
        /// 地点树,null获取所有地点树，id获取id为根的地点树
        /// </summary>
        /// <returns></returns>
        public List<TreeEasy> LocationTree(string location_id)
        {
            //节点列表
            List<TreeModel> Nodes = new();
            //获取location
            List<base_location> list_location = _pikachuApp.GetRootAndBranch<base_location>(location_id);
            //转化为node
            Nodes = _pikachuApp.ListElementToNode(list_location);

            //将节点列表转化为树
            List<TreeModel> treeModels = _pikachuApp.ListToTreeModel(Nodes, null);

            //将树转化为TreeEasy树
            List<TreeEasy> easies = _pikachuApp.TreeModelToTreeEasy(treeModels);
            return easies;
        }

        /// <summary>
        /// 根据地点id获取挂载的仓库
        /// </summary>
        /// <param name="location_id"></param>
        /// <returns></returns>
        public List<base_warehouse> GetWareHouseByLocationId(string location_id)
        {
            List<base_warehouse> base_Warehouses = _pikachuApp.GetAll<base_warehouse>();
            if (string.IsNullOrEmpty(location_id))
            {
                return base_Warehouses;
            }

            base_Warehouses.Where(c => c.location_id == location_id).ToList();
            return base_Warehouses;
        }


        /// <summary>
        /// 仓库定义页面的搜索框，地点-仓库定义
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<base_warehouse> SearchBar(PageReq req, string location_id)
        {
            List<base_warehouse> base_Warehouses = _pikachuApp.GetAll<base_warehouse>(req);
            //如果没传地点id，直接进行条件查询
            if (string.IsNullOrEmpty(location_id))
            {
                return base_Warehouses;
            }
            //如果传了地点id，根据地点id筛选
            base_Warehouses = base_Warehouses.Where(c => c.location_id == location_id).ToList();
            return base_Warehouses;
        }

        /// <summary>
        /// 检查表中是否存在数据
        /// </summary>
        /// <param name="chartName"></param>
        /// <returns></returns>
        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            _wareHouseRepository.CheckChartIfExistsData(ref referenceCharts, id, chartName);
        }

        /// <summary>
        /// 删除仓库
        /// </summary>
        public void deleteWareHouse(string[] ids, IDbConnection dbConnection = null)
        {
            _wareHouseRepository.DeleteWareHouse(ids, dbConnection);
        }


    }
}
