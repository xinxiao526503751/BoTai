using Hhmocon.Mes.DataBase;
using System;

namespace Hhmocon.Mes.Repository.Domain
{
    /// <summary>
    /// </summary>
    /// <summary>
    /// 文档记录表
    /// </summary>
    [Table(TableName = "base_files", KeyName = "file_id", IsIdentity = false)]
    public class base_files
    {

        /// <summary>
        /// 物料name
        /// </summary>
        public string material_name { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public string process_id { get; set; }

        /// <summary>
        /// 工序name
        /// </summary>
        public string process_name { get; set; }

        /// <summary>
        /// create_by
        /// </summary>
        public string create_by { get; set; }

        /// <summary>
        /// create_by_name
        /// </summary>
        public string create_by_name { get; set; }

        /// <summary>
        /// modified_by
        /// </summary>
        public string modified_by { get; set; }

        /// <summary>
        /// delete_mark
        /// </summary>
        public int delete_mark { get; set; }

        /// <summary>
        ///  file_time
        /// </summary>
        public DateTime file_time { get; set; }

        /// <summary>
        /// create_time
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// modified_time
        /// </summary>
        public DateTime modified_time { get; set; }

        /// <summary>
        /// file_size
        /// </summary>
        public long file_size { get; set; }

        /// <summary>
        /// file_id
        /// </summary>
        public string file_id { get; set; }

        /// <summary>
        /// file_name
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// file_path
        /// </summary>
        public string file_path { get; set; }

        /// <summary>
        /// file_src_name
        /// </summary>
        public string file_src_name { get; set; }

        /// <summary>
        /// file_type
        /// </summary>
        public string file_type { get; set; }

        /// <summary>
        /// material_id
        /// </summary>
        public string material_id { get; set; }

        /// <summary>
        /// modified_by_name
        /// </summary>
        public string modified_by_name { get; set; }
    }
}
