/*
 * @Author: your name
 * @Date: 2021-07-28 16:46:32
 * @LastEditTime: 2022-01-17 10:48:48
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\ExamPlanMethodItem.js
 */
import request from '@/utils/request'

// 新增设备-点检项关联
export function Create(data, equipment_id, exam_plan_method_id) {
  return request({
    url: '/ExamPlanMethodItem/Create',
    method: 'post',
    data,
    params: {
      equipment_id: equipment_id,
      exam_plan_method_id: exam_plan_method_id
    }
  })
}

// 传入点检计划和设备id 在计划-项目表的数据中 获取设备-点检项目关联
export function GetItemByPlanIdAndEquipmentId(
  exam_plan_method_id,
  equipment_id
) {
  return request({
    url: '/ExamPlanMethodItem/GetItemByPlanIdAndEquipmentId',
    method: 'post',
    params: {
      exam_plan_method_id: exam_plan_method_id,
      equipment_id: equipment_id
    }
  })
}

export function GetItemByPlanId(exam_plan_method_id) {
  return request({
    url: '/ExamPlanMethodItem/GetItemByPlanId',
    method: 'post',
    params: {
      exam_plan_method_id: exam_plan_method_id
    }
  })
}

// 传入点检计划-点检项目关联信息id 删除对应的关联
export function Delete(data) {
  return request({
    url: '/ExamPlanMethodItem/Delete',
    method: 'post',
    data
  })
}
