using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.Domain.Equipment
{
    /// <summary>
    /// 安装地点
    /// </summary>
    public class InstallSite
    {
        public int Id { get; set; }
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public string PlaceId { get; set; }
        public int DELETE_MARK { get; set; }
        public DateTime CREATED_TIME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? UPDATE_TIME { get; set; }
        public string? UPDATE_BY { get; set; }
    }
}
