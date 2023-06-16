/*
 * @Author: your name
 * @Date: 2021-07-21 21:04:32
 * @LastEditTime: 2021-07-21 21:04:32
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseExamItem.js
 */
import request from '@/utils/request'

// 得到设备点检项分页列表数据
export function GetList(data) {
  return request({
    url: '/ExamPlanRec/GetList',
    method: 'post',
    data
  })
}
// 新增挂载在设备下的点检定义
export function Create(data) {
  return request({
    url: '/ExamPlanRec/Create',
    method: 'post',
    data
  })
}
// 根据EquipmentId获取该设备下的点检计划记录
export function GetExamPlanRecByEquipmentId(data, params) {
  return request({
    url: '/ExamPlanRec/GetExamPlanRecByEquipmentId',
    method: 'post',
    data,
    params: {
      equipment_id: params
    }
  })
}
