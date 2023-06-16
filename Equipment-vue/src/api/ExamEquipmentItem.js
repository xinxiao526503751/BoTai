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
    url: '/ExamEquipmentItem/GetList',
    method: 'post',
    data
  })
}
// 新增挂载在设备下的点检定义
export function Create(data) {
  return request({
    url: '/ExamEquipmentItem/Create',
    method: 'post',
    data
  })
}
// 更新
export function Update(data) {
  return request({
    url: '/ExamEquipmentItem/Update',
    method: 'post',
    data
  })
}
//删除挂载在设备下的点检定义
export function Delete(data) {
  return request({
    url: '/ExamEquipmentItem/Delete',
    method: 'post',
    data
  })
}
// 得到点检列表数据：param+data接口文档中
export function GetExamitemByEquipmentIdSampleData(data, params) {
  return request({
    url: '/ExamEquipmentItem/GetExamitemByEquipmentIdSampleData',
    method: 'post',
    data,
    params: {
      equipment_id: params
    }
  })
}

export function GetExamitemByEquipmentId(data, params) {
  return request({
    url: '/ExamEquipmentItem/GetExamitemByEquipmentId',
    method: 'post',
    data,
    params: {
      equipment_id: params
    }
  })
}

export function GetDetail(data) {
  return request({
    url: '/ExamEquipmentItem/GetDetail',
    method: 'post',
    data
  })
}
