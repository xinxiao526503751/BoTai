using Hhmocon.Mes.DataBase;
using System.ComponentModel;

namespace Hhmocon.Mes.Repository.Domain
{
    [Table(TableName = "association_parameter", KeyName = "id", IsIdentity = false)]
    public class association_parameter
    {

        [DefaultValue("")]
        public string id { get; set; }

        [DefaultValue("")]
        public string vlue_type { get; set; }

        [DefaultValue("")]
        public string method_type { get; set; }

    }
}
