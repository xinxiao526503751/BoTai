using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    public class BaseFilesApp
    {
        private readonly BaseFilesRepository _BaseFilesRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;

        public BaseFilesApp(BaseFilesRepository BaseFilesRepository, PikachuRepository pikachuRepository, IAuth auth)
        {
            _BaseFilesRepository = BaseFilesRepository;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
        }

        public int Insert(List<base_files> data)
        {
            foreach (base_files item in data)
            {
                //取ID
                item.file_id = CommonHelper.GetNextGUID();
                item.modified_time = Time.Now;
                item.create_time = DateTime.Now;
                item.create_by = _auth.GetUserAccount(null);
                item.create_by_name = _auth.GetUserName(null);
                item.create_by = _auth.GetUserAccount(null);
                item.modified_by = _auth.GetUserName(null);
                item.modified_by_name = _pikachuRepository.GetById<base_material>(item.material_id)?.material_name;
                item.process_name = _pikachuRepository.GetById<base_process>(item.process_id)?.process_name;
                if (!_pikachuRepository.Insert(item))
                {
                    return -1;
                }
            }
            return data.Count;
        }

        public List<base_files> GetListByProcessId(PageReq req, string processId, ref long lcount)
        {
            string strKey = req.key;
            int iPage = req.page;
            int iRows = req.rows;
            string strSort = req.sort;
            string strOrder = req.order;
            string whereStr = CommonHelper.GetSqlConditonalStr(strKey);
            string ordStr = string.Empty;

            List<base_files> _Files = _pikachuRepository.GetAll<base_files>()
                .Where(a => a.process_id == processId)?.ToList();
            lcount = _Files != null ? _Files.Count : 0;
            _Files = _Files != null ? _Files.Skip((iPage - 1) * iRows).Take(iRows).ToList() : null;
            return _Files;
        }

    }
}
