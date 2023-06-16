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

using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    /// <summary>
    /// 事件通知配置App层
    /// </summary>
    public class BaseFaultNoticecfgApp
    {
        private readonly IBaseFaultNoticecfgRepository _baseFaultNoticecfgRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseFaultNoticecfgApp(IBaseFaultNoticecfgRepository baseFaultNoticecfgRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _baseFaultNoticecfgRepository = baseFaultNoticecfgRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        /// <summary>
        /// 新增事件通知配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_fault_noticecfg InsertBaseFaultNoticeCfg(base_fault_noticecfg data)
        {
            //取ID
            data.fault_noticecfg_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);


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
        /// 根据通知等级获取已经保存的通知配置
        /// </summary>
        /// <param name="notice_level">通知等级</param>
        /// <returns></returns>
        public List<base_fault_noticecfg> GetByFaultIdAndNoticeLevel(string fault_id, string notice_level)
        {
            return _baseFaultNoticecfgRepository.GetByFaultIdAndNoticeLevel(fault_id, notice_level);
        }

        /// <summary>
        /// 插入事件配置
        /// </summary>
        /// <param name="t">事件</param>
        /// <param name="user_ids">用户Id</param>
        /// <returns></returns>
        public bool InsertBaseFaultNoticeCfg(base_fault_noticecfg t, List<string> user_ids)
        {
            //判断事件id是否存在
            base_fault base_Fault = _pikachuRepository.GetById<base_fault>(t.fault_id);
            if (base_Fault == null)
            {
                throw new Exception("发现不存在的事件，请不要勾选事件类型");
            }
            //判断通知类型是否存在
            if (t.notice_type != "webchat")
            {
                throw new Exception("未选择通知方式");
            }

            //根据事件id对通知配置进行查重，如果已经存在要擦除
            base_fault_noticecfg exist_noticecfg = _pikachuRepository.GetAll<base_fault_noticecfg>().Where(c => c.fault_id == t.fault_id).FirstOrDefault();
            if (exist_noticecfg != null)
            {
                string[] exist_ids = { exist_noticecfg.fault_noticecfg_id };
                _pikachuRepository.Delete_Mask<base_fault_noticecfg>(exist_ids);
            }

            List<string> ids = new();
            ids.AddRange(user_ids);
            //对ids进行解析，不要部门，只保留人员
            foreach (string temp in user_ids)
            {
                if (_pikachuRepository.GetById<sys_user>(temp) == null)
                {
                    ids.Remove(temp);
                }
            }
            if (ids.Count == 0)
            {
                throw new("没有选中通知人员");
            }
            return _baseFaultNoticecfgRepository.InsertBaseFaultNoticeCfg(t, ids);
        }
    }
}
