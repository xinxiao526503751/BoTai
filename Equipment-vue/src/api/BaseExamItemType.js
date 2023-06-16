/*
 * @Author: your name
 * @Date: 2021-07-21 21:07:03
 * @LastEditTime: 2022-01-17 10:44:40
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseExamItemType.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseExamitemType/GetList',
    method: 'post',
    data
  })
}
// 根据条件得到全部列表数据
export function GetAll(params) {
  return request({
    url: '/BaseExamitemType/GetAll',
    method: 'post',
    params: {
      method_type: params
    }
  })
}

// 新建地点类型信息
export function Create(data) {
  return request({
    url: '/BaseExamitemType/Create',
    method: 'post',
    data
  })
}
// 更新点检类型信息
export function Update(data) {
  return request({
    url: '/BaseExamitemType/Update',
    method: 'post',
    data
  })
}
//删除点检类型信息
export function Delete(data) {
  return request({
    url: '/BaseExamitemType/Delete',
    method: 'post',
    data
  })
}
