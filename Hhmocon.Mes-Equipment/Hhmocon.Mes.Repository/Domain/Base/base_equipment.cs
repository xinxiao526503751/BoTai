using Hhmocon.Mes.DataBase;
using System;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 设备表
    /// </summary>
    [Table(TableName = "base_equipment", KeyName = "equipment_id", Code = "equipment_code", IsIdentity = false)]
    public class base_equipment
    {
        /// <summary>
        /// 验收时间
        /// </summary>
        public DateTime reception_time { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string equipment_code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string equipment_name { get; set; }

        /// <summary>
        /// 地点编号
        /// </summary>
        public string location_id { get; set; }

        /// <summary>
        /// 车间地点编号
        /// </summary>
        public string company_shop_id { get; set; }

        /// <summary>
        /// 子工厂
        /// </summary>
        public string sub_company { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public string wh_id { get; set; }

        /// <summary>
        /// 设备功能类型编号
        /// </summary>
        public string equipment_class_id { get; set; }

        /// <summary>
        /// 设备类型编号
        /// </summary>
        public string equipment_type_id { get; set; }

        /// <summary>
        /// 设备简称
        /// </summary>
        public string equipment_short_name { get; set; }

        /// <summary>
        /// 设备长名称
        /// </summary>
        public string equipment_long_name { get; set; }

        /// <summary>
        /// 设备资产编号
        /// </summary>
        public string asset_id { get; set; }

        /// <summary>
        /// 功率
        /// </summary>
        [DefaultValue(0)]
        public double power { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime manufacture_date { get; set; }

        /// <summary>
        /// 安装日期
        /// </summary>
        public DateTime install_date { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }

        /// <summary>
        /// 硬件类型
        /// </summary>
        public string hardware_type { get; set; }

        /// <summary>
        /// 硬件版本
        /// </summary>
        public string hardware_version { get; set; }

        /// <summary>
        /// 设备序列号
        /// </summary>
        public string device_series_id { get; set; }

        /// <summary>
        /// 设备生产厂家编号
        /// </summary>
        public string device_company_id { get; set; }

        /// <summary>
        /// 图片目录
        /// </summary>
        public string pic_path { get; set; }

        /// <summary>
        /// 超时此时间没有加工数产生，设备
        /// </summary>
        public int check_stop_time { get; set; }

        /// <summary>
        /// 实际工作总时长
        /// </summary>
        public int mt_work_time { get; set; }

        /// <summary>
        /// 制造商
        /// </summary>
        public string made_by { get; set; }

        /// <summary>
        /// 设备原价值
        /// </summary>
        [DefaultValue(0)]
        public double org_value { get; set; }

        /// <summary>
        /// 剩余价值
        /// </summary>
        [DefaultValue(0)]
        public double salvage_value { get; set; }

        /// <summary>
        /// 折旧值
        /// </summary>
        [DefaultValue(0)]
        public double depred_value { get; set; }

        /// <summary>
        /// 使用年限
        /// </summary>
        [DefaultValue(0)]
        public double dur_year { get; set; }

        /// <summary>
        /// 折旧方法
        /// </summary>
        public string depr_type { get; set; }

        /// <summary>
        /// 0代表采集设备,1代表未安装采集,2代表闲置，3代表报废
        /// </summary>
        public int use_type { get; set; }

        /// <summary>
        /// 性能率
        /// </summary>
        public int p_rate { get; set; }

        /// <summary>
        /// 停机弹出报警时间
        /// </summary>
        public int alarm_time { get; set; }

        /// <summary>
        /// 经纬度-经度
        /// </summary>
        public string lng { get; set; }

        /// <summary>
        /// 经纬度-纬度
        /// </summary>
        public string lat { get; set; }

        /// <summary>
        /// 节拍标准值
        /// </summary>
        [DefaultValue(0)]
        public double beat { get; set; }

        /// <summary>
        /// 设备规格
        /// </summary>
        public string eqpt_spec { get; set; }

        /// <summary>
        /// 最后的维护时间
        /// </summary>
        public DateTime tested_time { get; set; }

        /// <summary>
        /// 资产等级
        /// </summary>
        public string asset_lvl { get; set; }

        /// <summary>
        /// 资产名称
        /// </summary>
        public string asset_name { get; set; }

        /// <summary>
        /// 资产编号
        /// </summary>
        public string asset_code { get; set; }

        /// <summary>
        /// 资产归属工厂
        /// </summary>
        public string use_sub_company { get; set; }

        /// <summary>
        /// 物理状态
        /// </summary>
        public int physical_state { get; set; }

        /// <summary>
        /// 是否在看板显示
        /// </summary>
        public int is_show_board { get; set; }

        /// <summary>
        /// 最大上模数量
        /// </summary>
        public int max_mould_num { get; set; }

        /// <summary>
        /// 最大操作工人数
        /// </summary>
        public int max_person_num { get; set; }

        /// <summary>
        /// 是否真实采集设备
        /// </summary>
        public int is_real { get; set; }

        /// <summary>
        /// 是否需要生成子工单
        /// </summary>
        public int is_need_sub_wo_code { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public string process_id { get; set; }

        /// <summary>
        /// 用于标识设备的可编辑的唯一性，
        /// </summary>
        public string dev_mn { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string model_id { get; set; }

        /// <summary>
        /// 当前物料编号
        /// </summary>
        public string cur_material_id { get; set; }

        /// <summary>
        /// 预留1
        /// </summary>
        public string res1 { get; set; }

        /// <summary>
        /// 预留2
        /// </summary>
        public string res2 { get; set; }

        /// <summary>
        /// 预留3
        /// </summary>
        public string res3 { get; set; }

        /// <summary>
        /// 预留4
        /// </summary>
        public string res4 { get; set; }

        /// <summary>
        /// 预留5
        /// </summary>
        public string res5 { get; set; }

        /// <summary>
        /// 预留6
        /// </summary>
        public string res6 { get; set; }

        /// <summary>
        /// 预留7
        /// </summary>
        public string res7 { get; set; }

        /// <summary>
        /// 预留8
        /// </summary>
        public string res8 { get; set; }

        /// <summary>
        /// 预留9
        /// </summary>
        public string res9 { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// 修改人时间
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string modified_by_name { get; set; }

        /// <summary>
        /// 地点名称
        /// </summary>
        public string location_name { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string process_name { get; set; }

        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string equipment_type_name { get; set; }
    }
}
