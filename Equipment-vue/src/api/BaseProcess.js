/*
 * @Author: your name
 * @Date: 2021-07-12 11:28:43
 * @LastEditTime: 2021-07-23 11:11:01
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseProcess.js
 */
import request from '@/utils/request'

// 得到工序类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseProcess/GetList',
    method: 'post',
    data
  })
}
// 根据条件得到全部列表数据
export function GetAll(data) {
  return request({
    url: '/BaseProcess/GetAll',
    method: 'post',
    data
  })
}

// 新建工序信息
export function Create(data) {
  return request({
    url: '/BaseProcess/Create',
    method: 'post',
    data
  })
}
// 修改工序信息
export function Update(data) {
  return request({
    url: '/BaseProcess/Update',
    method: 'post',
    data
  })
}
// 删除工序信息
export function Delete(data) {
  return request({
    url: '/BaseProcess/Delete',
    method: 'post',
    data
  })
}
// 得到工序列表数据
export function GetLeafs(data, params) {
  return request({
    url: '/BaseProcess/GetLeafs',
    method: 'post',
    data,
    params: {
      type_id: params
    }
  })
}
export function SearchLeafs(data, params) {
  return request({
    url: '/BaseProcess/SearchLeafs',
    method: 'post',
    data,
    params: {
      type_id: params
    }
  })
}
