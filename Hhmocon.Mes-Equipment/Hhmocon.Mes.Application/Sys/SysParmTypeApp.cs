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
using Hhmocon.Mes.Repository.Repository.Sys.SysParmType;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 单位类型App
    /// </summary>
    public class SysParmTypeApp
    {
        private readonly PikachuRepository _pikachuRepository;
        private readonly PikachuApp _pikachuApp;
        private readonly IAuth _auth;
        private readonly SysParmTypeRepository _sysParmTypeRepository;
        public SysParmTypeApp(PikachuRepository pikachuRepository, IAuth auth, PikachuApp pikachuApp, SysParmTypeRepository sysParmTypeRepository)
        {
            _pikachuRepository = pikachuRepository;
            _auth = auth;
            _pikachuApp = pikachuApp;
            _sysParmTypeRepository = sysParmTypeRepository;
        }
        /// <summary>
        /// 新增单位类型
        /// </summary>
        /// <returns></returns>
        public bool Insert(sys_parm_type obj)
        {
            //取ID
            obj.parm_type_id = CommonHelper.GetNextGUID();
            obj.modified_time = Time.Now;
            obj.create_time = DateTime.Now;
            obj.create_by = _auth.GetUserAccount(null);
            obj.create_by_name = _auth.GetUserName(null);
            obj.modified_by = _auth.GetUserAccount(null);
            obj.modified_by_name = _auth.GetUserName(null);
            return (_pikachuRepository.Insert(obj));
        }

        /// <summary>
        /// 参数类型树，参数类型没有上下级，但要做成树的格式
        /// </summary>
        /// <returns></returns>
        public List<TreeEasy> SysParmTree()
        {
            //获取sys_parm_type
            List<sys_parm_type> sys_Parm_Types = _pikachuRepository.GetAll<sys_parm_type>();
            //转化为TreeEasy类型
            List<TreeEasy> easies = new();
            foreach (sys_parm_type temp in sys_Parm_Types)
            {
                TreeEasy treeEasy = new();
                treeEasy.children = null;
                treeEasy.id = temp.parm_type_id;
                treeEasy.label = temp.parm_type_name;
                treeEasy.NodeType = temp.parm_type_default;
                easies.Add(treeEasy);
            }

            return easies;
        }


        public void CheckChartIfExistsData(ref List<string> referenceCharts, string id, string chartName)
        {
            _sysParmTypeRepository.CheckChartIfExistsData(ref referenceCharts, id, chartName);
        }

        public void DeleteParmType(string[] idsl)
        {
            _sysParmTypeRepository.DeleteParmType(idsl);
        }
    }
}
