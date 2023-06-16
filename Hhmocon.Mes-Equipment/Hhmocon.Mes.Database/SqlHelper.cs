/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Hhmocon.Mes.Util.AutofacManager;
using Hhmocon.Mes.Util.String;

namespace Hhmocon.Mes.Database
{
    public class SqlHelper : IDependency
    {
        /// <summary>
        /// 找到所有_type_parent_name=参数
        /// </summary>
        /// <returns></returns>
        public string GetByTypeParentName<T>(string _type_parent_name)
        {
            string sql;
            string ClassName = typeof(T).Name;
            string property = "";
            switch (ClassName)
            {
                case "base_material_type":
                    property = "material_type_parentname";
                    break;
                case "base_equipment_type":
                    property = "equipment_type_parentname";
                    break;
                case "base_process_type":
                    property = "process_type_parentname";
                    break;
                case "base_location":
                    property = "location_parentname";
                    break;
                case "base_examitem_type":
                    property = "examitem_type_parentname";
                    break;
            }

            sql = SqlAssemble.Delete_Mark(string.Format("{0} = '{1}'", property, _type_parent_name));

            //类型父名为空
            if (string.IsNullOrEmpty(_type_parent_name))
            {
                sql = SqlAssemble.Delete_Mark(string.Format("{0} is null", property));
            }
            return sql;
        }

        /// <summary>
        /// 找到所有_type_parent_name=参数.动态解析父名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetByTypeParentName<T>(T obj)
        {
            dynamic d = obj;
            string _type_parent_name = null;

            string sql;
            string ClassName = typeof(T).Name;
            string property = "";
            switch (ClassName)
            {
                case "base_material_type":
                    property = "material_type_parentname";
                    _type_parent_name = d.material_type_name;//动态解析
                    break;
                case "base_equipment_type":
                    property = "equipment_type_parentname";
                    _type_parent_name = d.equipment_type_name;
                    break;
                case "base_process_type":
                    property = "process_type_parentname";
                    _type_parent_name = d.process_type_name;
                    break;
                case "base_location":
                    property = "location_parentname";
                    _type_parent_name = d.location_name;
                    break;
                case "base_examitem_type":
                    property = "examitem_type_parentname";
                    _type_parent_name = d.equipment_type_name;
                    break;
            }

            sql = SqlAssemble.Delete_Mark(string.Format("{0} = '{1}'", property, _type_parent_name));

            //类型父名为空
            if (string.IsNullOrEmpty(_type_parent_name))
            {
                sql = SqlAssemble.Delete_Mark(string.Format("{0} is null", property));
            }
            return sql;
        }


        /// <summary>
        /// 找到parentid=参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_type_parent_id"></param>
        /// <returns></returns>
        public string GetByParentId<T>(string parent_id)
        {
            string sql;
            string ClassName = typeof(T).Name;
            string property = "";

            switch (ClassName)
            {
                case "base_material_type":
                    property = "material_type_parentid";
                    break;

                case "base_process_type":
                    property = "process_type_parentid";
                    break;

                case "base_location":
                    property = "location_parentid";
                    break;

                case "base_location_type":
                    property = "location_type_parentid";
                    break;

                case "base_equipment_type":
                    property = "equipment_type_parentid";
                    break;

                case "base_equipment_class":
                    property = "equipment_class_parentid";
                    break;

                case "base_examitem_type":
                    property = "examitem_type_parentid";
                    break;

                case "sys_right":
                    property = "parent_right_id";
                    break;

                case "sys_dept":
                    property = "parent_dept_id";
                    break;

                case "base_bom":
                    property = "parent_bom_id";
                    break;
            }

            sql = SqlAssemble.Delete_Mark(string.Format("{0} = '{1}'", property, parent_id));

            //类型父名为空
            if (string.IsNullOrEmpty(parent_id))
            {
                sql = SqlAssemble.Delete_Mark(string.Format("{0} is null", property));
            }
            return sql;
        }


        /// <summary>
        /// 找到所有_parentid=参数.动态解析父id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetByParent<T>(T obj)
        {
            dynamic d = obj;
            string _type_parent_id = null;

            string sql;
            string ClassName = typeof(T).Name;
            string property = "";
            switch (ClassName)
            {
                case "base_material_type":
                    property = "material_type_parentid";
                    _type_parent_id = d.material_type_id;//动态解析
                    break;

                case "base_equipment":
                    property = "equipment_parentid";
                    _type_parent_id = d.equipment_id;
                    break;

                case "base_equipment_type":
                    property = "equipment_type_parentid";
                    _type_parent_id = d.equipment_type_id;
                    break;

                case "base_equipment_class":
                    property = "equipment_class_parentid";
                    _type_parent_id = d.equipment_class_id;
                    break;

                case "base_process_type":
                    property = "process_type_parentid";
                    _type_parent_id = d.process_type_id;
                    break;

                case "base_location":
                    property = "location_parentid";
                    _type_parent_id = d.location_id;
                    break;

                case "base_location_type":
                    property = "location_type_parentid";
                    _type_parent_id = d.location_type_id;
                    break;

                case "base_examitem_type":
                    property = "examitem_type_parentid";
                    _type_parent_id = d.examitem_type_id;
                    break;

                case "sys_dept":
                    property = "parent_dept_id";
                    _type_parent_id = d.dept_id;
                    break;
            }

            sql = SqlAssemble.Delete_Mark(string.Format("{0} = '{1}'", property, _type_parent_id));

            //类型父名为空
            if (string.IsNullOrEmpty(_type_parent_id))
            {
                sql = SqlAssemble.Delete_Mark(string.Format("{0} is null", property));
            }
            return sql;
        }

        /// <summary>
        /// 根据_type_id找到所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_type_parent_name"></param>
        /// <returns></returns>
        public string GetByTypeId<T>(string _type_id)
        {
            string sql;
            string ClassName = typeof(T).Name;
            string property = "";
            switch (ClassName)
            {
                case "base_location":
                    property = "location_id";
                    break;

                case "base_material":
                    property = "material_type_id";
                    break;

                case "base_equipment":
                    property = "equipment_type_id";
                    break;

                case "base_process":
                    property = "process_type_id";
                    break;

                case "base_examitem":
                    property = "examitem_type_id";
                    break;

            }

            sql = string.Format("{0} = '{1}'", property, _type_id);
            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }

        /// <summary>
        /// 根据_type_id找到所有,动态解析type_id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_type_parent_name"></param>
        /// <returns></returns>
        public string GetByType<T>(T obj)
        {
            dynamic d = obj;
            string _type_id = null;

            string sql;
            string ClassName = typeof(T).Name;
            string property = "";
            switch (ClassName)
            {
                case "base_material":
                    property = "material_type_id";
                    _type_id = d.material_type_id;//动态解析
                    break;
                case "base_material_type":
                    property = "material_type_id";
                    _type_id = d.material_type_id;
                    break;

                case "base_equipment":
                    property = "equipment_type_id";
                    _type_id = d.equipment_type_id;
                    break;
                case "base_equipment_type":
                    property = "equipment_type_id";
                    _type_id = d.equipment_type_id;
                    break;

                case "base_process":
                    property = "process_type_id";
                    _type_id = d.process_type_id;
                    break;
                case "base_process_type":
                    property = "process_type_id";
                    _type_id = d.process_type_id;
                    break;


                case "base_examitem":
                    property = "examitem_type_id";
                    _type_id = d.examitem_type_id;
                    break;
                case "base_examitem_type":
                    property = "examitem_type_id";
                    _type_id = d.examitem_type_id;
                    break;
            }

            sql = string.Format("{0} = '{1}'", property, _type_id);
            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }



        /// <summary>
        /// 根据_name查找所有
        /// 警告：该方自sys模块开始不再被建议使用。停止更新。2021/8/12
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_type_id"></param>
        /// <returns></returns>
        public string GetByName<T>(string _name)
        {
            string sql;
            string ClassName = typeof(T).Name;
            string property = "";
            switch (ClassName)
            {
                case "base_material":
                    property = "material_name";
                    break;

                case "base_material_type":
                    property = "material_type_name";
                    break;

                case "base_process":
                    property = "process_name";
                    break;

                case "base_process_type":
                    property = "process_type_name";
                    break;

                case "base_equipment":
                    property = "equipment_name";
                    break;

                case "base_equipment_type":
                    property = "equipment_type_name";
                    break;

                case "base_equipment_class":
                    property = "equipment_class_name";
                    break;

                case "base_location":
                    property = "location_name";
                    break;

                case "base_location_type":
                    property = "location_type_name";
                    break;

                case "base_examitem_type":
                    property = "examitem_type_name";
                    break;

                case "base_examitem":
                    property = "examitem_type_name";
                    break;

                case "base_customer":
                    property = "customer_name";
                    break;

                case "base_supplier":
                    property = "supplier_name";
                    break;

                case "exam_plan_method":
                    property = "exam_plan_method_name";
                    break;

                case "base_fault":
                    property = "fault_name";
                    break;
                case "base_fault_class":
                    property = "fault_class_name";
                    break;
                case "sys_user":
                    property = "user_name";
                    break;
                case "base_process_route":
                    property = "process_route_name";
                    break;
            }

            if (string.IsNullOrEmpty(_name))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, _name);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }


        /// <summary>
        /// 根据location_id查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location_id"></param>
        /// <returns></returns>
        public string GetByLocationId<T>(string location_id)
        {
            string sql;
            string property = "location_id";

            if (string.IsNullOrEmpty(location_id))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, location_id);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }


        /// <summary>
        /// 根据点检计划id查询
        /// </summary>
        /// <returns></returns>
        public string GetByExamPlanMethodId(string exam_plan_method_id)
        {
            string sql;
            string property = "exam_plan_method_id";

            if (string.IsNullOrEmpty(exam_plan_method_id))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, exam_plan_method_id);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }

        /// <summary>
        /// 通过点检项id查询
        /// </summary>
        /// <param name="exam_id"></param>
        /// <returns></returns>
        public string GetByExamItemId(string examitem_id)
        {
            string sql;
            string property = "exam_id";

            if (string.IsNullOrEmpty(examitem_id))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, examitem_id);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }

        // GetExamPlanRecItemByRecId
        public string GetExamPlanRecItemByRecId<T>(string examPlanRecId)
        {
            string sql;
            string property = "exam_plan_rec_id";

            if (string.IsNullOrEmpty(examPlanRecId))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, examPlanRecId);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }

        /// <summary>
        /// 通过设备id查询
        /// </summary>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public string GetByEquipmentId<T>(string equipment_id)
        {
            string sql;
            string property = "equipment_id";

            if (string.IsNullOrEmpty(equipment_id))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, equipment_id);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }

        /// <summary>
        /// 通过equipemntId联合查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public string GetByEquipmentIdUnion<T>(string equipment_id)
        {
            string sql;
            string property = "equipment_id";

            if (string.IsNullOrEmpty(equipment_id))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, equipment_id);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }
        /// <summary>
        /// 通过点检计划id和设备id查询
        /// </summary>
        /// <param name="exam_plan_method_id"></param>
        /// <param name="equipment_id"></param>
        /// <returns></returns>
        public string GetItemByPlanIdAndEquipmentId(string exam_plan_method_id, string equipment_id)
        {
            string sql;
            string property = "equipment_id";

            if (string.IsNullOrEmpty(equipment_id))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, equipment_id);
            }

            sql += " And ";
            property = "exam_plan_method_id";
            if (string.IsNullOrEmpty(exam_plan_method_id))
            {
                sql += string.Format("{0} is null", property);
            }
            else
            {
                sql += string.Format("{0} = '{1}'", property, equipment_id);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }

        public string GetByRecId<T>(string exam_plan_rec_id)
        {
            string sql;
            string property = "exam_plan_rec_id";

            if (string.IsNullOrEmpty(exam_plan_rec_id))
            {
                sql = string.Format("{0} is null", property);
            }
            else
            {
                sql = string.Format("{0} = '{1}'", property, exam_plan_rec_id);
            }

            sql = SqlAssemble.Delete_Mark(sql);
            return sql;
        }

    }
}
