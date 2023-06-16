/*
 * @Author: your name
 * @Date: 2021-07-28 16:31:48
 * @LastEditTime: 2021-07-28 16:36:11
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\ExamPlanMethod.js
 */
import request from '@/utils/request'

// 添加点检计划
export function Create(data) {
  return request({
    url: '/ExamPlanMethod/Create',
    method: 'post',
    data
  })
}

// 删除点检计划
export function Delete(data) {
  return request({
    url: '/ExamPlanMethod/Delete',
    method: 'post',
    data
  })
}

// 更改点检计划
export function Update(data) {
  return request({
    url: '/ExamPlanMethod/Update',
    method: 'post',
    data
  })
}

// 点检计划分页
export function GetList(data) {
  return request({
    url: '/ExamPlanMethod/GetList',
    method: 'post',
    data
  })
}

// 根据ID得到点检计划明细信息
export function GetDetail(data) {
  return request({
    url: '/ExamPlanMethod/GetDetail',
    method: 'post',
    data
  })
}

// 根据EquipmentId获取该设备下的点检计划
export function GetExamPlanMethodByEquipmentId(data, params) {
  return request({
    url: '/ExamPlanMethod/GetExamPlanMethodByEquipmentId',
    method: 'post',
    data,
    params: {
      equipment_id: params
    }
  })
}
