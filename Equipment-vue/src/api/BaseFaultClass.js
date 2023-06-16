/*
 * @Author: your name
 * @Date: 2021-08-27 09:23:38
 * @LastEditTime: 2021-08-27 10:28:07
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseFaultClass.js
 */
import request from '@/utils/request'

// 得到地点类型分页列表数据
export function GetList(data) {
  return request({
    url: '/BaseFaultClass/GetList',
    method: 'post',
    data
  })
}
export function GetAll(data) {
  return request({
    url: '/BaseFaultClass/GetAll',
    method: 'post',
    data
  })
}

// 新建事件类型信息
export function Create(data) {
  return request({
    url: '/BaseFaultClass/Create',
    method: 'post',
    data
  })
}
// 修改事件类型信息
export function Update(data) {
  return request({
    url: '/BaseFaultClass/Update',
    method: 'post',
    data
  })
}
// 删除事件类型信息
export function Delete(data) {
  return request({
    url: '/BaseFaultClass/Delete',
    method: 'post',
    data
  })
}

// 得到事件类型详细
export function GetDetail(data) {
  return request({
    url: '/BaseFaultClass/GetDetail',
    method: 'post',
    data
  })
}
