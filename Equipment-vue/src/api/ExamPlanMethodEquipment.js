/*
 * @Author: your name
 * @Date: 2021-07-28 16:36:58
 * @LastEditTime: 2022-01-17 10:47:54
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\ExamPlanMethodEquipment.js
 */
import request from '@/utils/request'

// 新增点检计划-设备关联
export function Create(data, exam_plan_method_id, method_type) {
  return request({
    url: '/ExamPlanMethodEquipment/Create',
    method: 'post',
    data,
    params: {
      exam_plan_method_id: exam_plan_method_id,
      method_type: method_type
    }
  })
}

// 传入点检计划id获取关联的设备相关信息
export function GetEquipmentByPlanId(id) {
  return request({
    url: '/ExamPlanMethodEquipment/GetExamPlanMethodEquipmentByPlanId',
    method: 'post',
    params: {
      id: id
    }
  })
}

// 传入设备ids和点检计划id 在点检计划-设备表中删除对应的关联
export function Delete(data) {
  return request({
    url: '/ExamPlanMethodEquipment/Delete',
    method: 'post',
    data
  })
}
