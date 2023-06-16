/*
 * @Author: your name
 * @Date: 2021-07-21 21:04:32
 * @LastEditTime: 2021-07-21 21:04:32
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseExamItem.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseExamitem/GetList',
    method: 'post',
    data
  })
}
// 根据条件得到全部列表数据
export function GetAll(data) {
  return request({
    url: '/BaseExamitem/GetAll',
    method: 'post',
    data
  })
}

// 新建地点类型信息
export function Create(data) {
  return request({
    url: '/BaseExamitem/Create',
    method: 'post',
    data
  })
}
// 更新点检类型信息
export function Update(data) {
  return request({
    url: '/BaseExamitem/Update',
    method: 'post',
    data
  })
}
//删除信息
export function Delete(data) {
  return request({
    url: '/BaseExamitem/Delete',
    method: 'post',
    data
  })
}
// 得到点检列表数据：param+data接口文档中
export function GetLeafs(data, params) {
  return request({
    url: '/BaseExamitem/GetLeafs',
    method: 'post',
    data,
    params: {
      type_id: params
    }
  })
}
export function SearchLeafs(data, params) {
  return request({
    url: '/BaseExamitem/SearchLeafs',
    method: 'post',
    data,
    params: {
      type_id: params
    }
  })
}

export function selectParamter(data, params) {
  return request({
    url: '/BaseExamitem/selectParamter',
    method: 'post',
    data,
    params: {
      id: params
    }
  })
}
