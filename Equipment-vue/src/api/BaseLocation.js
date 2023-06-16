/*
 * @Author: your name
 * @Date: 2021-07-12 09:13:47
 * @LastEditTime: 2021-07-21 20:08:00
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseLocation.js
 */
import request from '@/utils/request'

// 得到地点分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseLocation/GetList',
    method: 'post',
    data
  })
}
// 根据条件得到全部列表数据
export function GetAll(data) {
  return request({
    url: '/BaseLocation/GetAll',
    method: 'post',
    data
  })
}

// 新建地点信息
export function Create(data) {
  return request({
    url: '/BaseLocation/Create',
    method: 'post',
    data
  })
}
// 修改地点信息
export function Update(data) {
  return request({
    url: '/BaseLocation/Update',
    method: 'post',
    data
  })
}
// 删除地点信息
export function Delete(data) {
  return request({
    url: '/BaseLocation/Delete',
    method: 'post',
    data
  })
}
// 得到地点列表数据：param+data接口文档中
export function GetLeafs(data, params) {
  return request({
    url: '/BaseLocation/GetLeafs',
    method: 'post',
    data,
    params: {
      root_id: params
    }
  })
}
//搜索
export function SearchLeafs(data, params) {
  return request({
    url: '/BaseLocation/SearchLeafs',
    method: 'post',
    data,
    params: {
      root_id: params
    }
  })
}
