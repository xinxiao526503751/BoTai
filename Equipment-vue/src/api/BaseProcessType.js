/*
 * @Author: your name
 * @Date: 2021-07-12 11:30:43
 * @LastEditTime: 2021-07-21 20:16:15
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseProcessType.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseProcessType/GetList',
    method: 'post',
    data
  })
}
export function GetAll(data) {
  return request({
    url: '/BaseProcessType/GetAll',
    method: 'post',
    data
  })
}

// 新建工序类型信息
export function Create(data) {
  return request({
    url: '/BaseProcessType/Create',
    method: 'post',
    data
  })
}
// 修改工序类型信息
export function Update(data) {
  return request({
    url: '/BaseProcessType/Update',
    method: 'post',
    data
  })
}
// 删除工序类型信息
export function Delete(data) {
  return request({
    url: '/BaseProcessType/Delete',
    method: 'post',
    data
  })
}
