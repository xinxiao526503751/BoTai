namespace Hhmocon.Mes.Repository.Domain
{
    public class planWorkRptResponse : plan_work_rpt
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

    }
}
