using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application
{
    public class SysSubRightApp
    {
        private readonly ISysSubRightRepository _sysSubRightRepository;
        private readonly PikachuRepository _pikachuRepository;
        public SysSubRightApp(ISysSubRightRepository sysRightRepository, PikachuRepository pikachuRepository)
        {
            _sysSubRightRepository = sysRightRepository;
            _pikachuRepository = pikachuRepository;
        }

        public bool Insert(sys_sub_right data)
        {
            //查重
            List<sys_sub_right> getByName = _sysSubRightRepository.GetByName(data.sub_right_name);
            if (getByName.Count != 0)
            {
                return false;
            }
            //取ID
            data.right_id = CommonHelper.GetNextGUID();
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            if (_pikachuRepository.Insert(data))
            {
                return true;
            }
            return false;
        }

        public List<sys_sub_right> GetBySysId(string PartentId, PageReq req, ref long icount)
        {

            List<sys_sub_right> data = _sysSubRightRepository.GetByPartentId(PartentId);
            icount = data.Count;
            //分页
            if (req != null)
            {
                string strKey = req.key;
                int iPage = req.page;
                int iRows = req.rows;
                string strSort = req.sort;
                string strOrder = req.order;
                string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
                string ordStr = string.Empty;
                data = data.Skip((iPage - 1) * iRows).Take(iRows).ToList();
            }
            return data;
        }
    }

}
