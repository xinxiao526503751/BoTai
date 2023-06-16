using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 权限表
    /// </summary>
    [Table(TableName = "sys_right", KeyName = "right_id", IsIdentity = false)]
    public class sys_right
    {

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public int right_ishide { get; set; }

        /// <summary>
        /// 排序序号
        /// </summary>
        public int right_sort { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public string right_id { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string right_code { get; set; }

        /// <summary>
        /// 上级权限Id
        /// </summary>
        public string parent_right_id { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string right_name { get; set; }

        /// <summary>
        /// 权限内容
        /// </summary>
        public string right_content { get; set; }

        /// <summary>
        /// 权限类型 0是系统，1是权限，2是按钮
        /// </summary>
        public string right_type { get; set; }

        /// <summary>
        /// 是否允许读写
        /// </summary>
        public string right_readwrite { get; set; }

        /// <summary>
        /// create_by_name
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// modified_by
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// modified_by_name
        /// </summary>
        public string modified_by_name { get; set; }


        /// <summary>
        /// URL
        /// </summary>
        public string right_url { get; set; }

        /// <summary>
        /// 图片目录
        /// </summary>
        public string right_pic { get; set; }

        /// <summary>
        /// 权限别名
        /// </summary>
        public string right_alias { get; set; }

        /// <summary>
        /// 附加参数
        /// </summary>
        public string right_parm { get; set; }

        /// <summary>
        /// create_by
        /// </summary>
        public string create_by { get; set; }
    }


}
