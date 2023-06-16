using hmocon.Mes.Repository.Domain;
using System;
using System.Collections.Generic;

namespace Hhmocon.Mes.Repository.Repository
{
    public interface IDippingDevRepository
    {
        /// <summary>
        /// 查找符合条件的dipping_dev_data
        /// </summary>
        /// <param name="EquipmentName"></param>
        /// <param name="VariableCode"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<dipping_dev_data> GetDippingDevs(string EquipmentName, string VariableCode, DateTime StartTime, DateTime EndTime);
        /// <summary>
        /// 查找符合条件的dipping_platen_data
        /// </summary>
        /// <param name="EquipmentName"></param>
        /// <param name="VariableCode"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<dipping_platen_data> GetPlatenDevs(string EquipmentName, string VariableCode, DateTime StartTime, DateTime EndTime);

        public List<dipping_dev_data> GetDippingDataLast(string dippingDevCode);
    }
}
