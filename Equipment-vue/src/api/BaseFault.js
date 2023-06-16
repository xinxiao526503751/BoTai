/*
 * @Author: your name
 * @Date: 2021-08-27 09:24:02
 * @LastEditTime: 2021-09-09 09:26:44
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseFault.js
 */
import request from '@/utils/request'

// 得到工序类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseFault/GetList',
    method: 'post',
    data
  })
}

// 根据条件得到全部列表数据
export function GetAll(data) {
  return request({
    url: '/BaseFault/GetAll',
    method: 'post',
    data
  })
}

// 新建工序信息
export function Create(data) {
  return request({
    url: '/BaseFault/Create',
    method: 'post',
    data
  })
}

// 修改工序信息
export function Update(data) {
  return request({
    url: '/BaseFault/Update',
    method: 'post',
    data
  })
}

// 删除工序信息
export function Delete(data) {
  return request({
    url: '/BaseFault/Delete',
    method: 'post',
    data
  })
}

// 得到工序列表数据
export function GetLeafs(data, params) {
  return request({
    url: '/BaseFault/GetFaultByFaultClassId',
    method: 'post',
    data,
    params: {
      fault_class_id: params
    }
  })
}

// 得到事件类型详细
export function GetDetail(data) {
  return request({
    url: '/BaseFault/GetDetail',
    method: 'post',
    data
  })
}

export function SearchBar(data) {
  return request({
    url: '/BaseFault/SearchBar',
    method: 'post',
    data
  })
}
