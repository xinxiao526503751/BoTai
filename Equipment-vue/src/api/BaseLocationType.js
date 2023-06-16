/*
 * @Author: your name
 * @Date: 2021-07-12 10:01:44
 * @LastEditTime: 2021-07-14 10:20:57
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseLocationType.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseLocationType/GetList',
    method: 'post',
    data
  })
}
// 根据条件得到全部列表数据
export function GetAll(data) {
  return request({
    url: '/BaseLocationType/GetAll',
    method: 'post',
    data
  })
}
// 新建地点类型信息
export function Create(data) {
  return request({
    url: '/BaseLocationType/Create',
    method: 'post',
    data
  })
}
// 更新地点类型信息
export function Update(data) {
  return request({
    url: '/BaseLocationType/Update',
    method: 'post',
    data
  })
}
export function Delete(data) {
  return request({
    url: '/BaseLocationType/Delete',
    method: 'post',
    data
  })
}
