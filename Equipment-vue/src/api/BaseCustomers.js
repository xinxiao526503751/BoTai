/*
 * @Author: your name
 * @Date: 2021-06-24 10:20:40
 * @LastEditTime: 2021-06-24 13:51:29
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseCustomer.js
 */
import request from '@/utils/request'

// 得到用户列表数据
export function GetList(data) {
  return request({
    url: '/BaseCustomers/GetList',
    method: 'post',
    data
  })
}

// 根据条件得到全部用户列表数据
export function GetAll(data) {
  return request({
    url: '/BaseCustomers/GetAll',
    method: 'post',
    data
  })
}

// 更新用户信息
export function Update(data) {
  return request({
    url: '/BaseCustomers/Update',
    method: 'post',
    data
  })
}

// 根据用户ID 删除用户数据 可批量删除
export function Delete(data) {
  return request({
    url: '/BaseCustomers/Delete',
    method: 'post',
    data
  })
}

// 新建用户信息
export function Create(data) {
  return request({
    url: '/BaseCustomers/Create',
    method: 'post',
    data
  })
}

// 根据ID得到用户明细信息
export function GetDetail(data) {
  return request({
    url: '/BaseCustomers/GetDetail',
    method: 'post',
    data
  })
}
