using Hhmocon.Mes.DataBase;

namespace Hhmocon.Mes.Repository.Domain
{

    [Table(TableName = "plan_work", KeyName = "plan_work_id", IsIdentity = false)]
    public class planWorkResponse : plan_work
    {

        /// <summary>
        /// 工序名称
        /// </summary>
        public string process_name { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string material_name { get; set; }

        /// <summary>
        /// 设备name
        /// </summary>
        public string equipment_name { get; set; }

        /// <summary>
        /// 设备code
        /// </summary>
        public string equipment_code { get; set; }

        /// <summary>
        /// 地点id
        /// </summary>
        public string location_id { get; set; }


    }
}
