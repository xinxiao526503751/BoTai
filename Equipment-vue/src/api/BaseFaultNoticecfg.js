/*
 * @Author: your name
 * @Date: 2021-08-27 09:24:25
 * @LastEditTime: 2021-09-01 16:48:45
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseFaultNoticecfg.js
 */
import request from '@/utils/request'

// 新建事件通知配置.该控制器对应事件通知配置页面右上角的保存按钮，由于通知人员可以多选，需对user进行解析
export function Create(data) {
  return request({
    url: '/BaseFaultNoticecfg/Create',
    method: 'post',
    data
  })
}

// 得到事件通知配置
export function GetList(data) {
  return request({
    url: '/BaseFaultNoticecfg/GetList',
    method: 'post',
    data
  })
}

// 左边事件类型+事件名称树
export function GetFailClassAndFailTree(data) {
  return request({
    url: '/BaseFaultNoticecfg/GetFailClassAndFailTree',
    method: 'post',
    data
  })
}

// 获取部门+用户树
export function GetDeptAndUserTree(data) {
  return request({
    url: '/BaseFaultNoticecfg/GetDeptAndUserTree',
    method: 'post',
    data
  })
}

// 通过事件id和通知等级notice_level获取已保存的配置人员
export function GetByNoticeLevel(fault_id, notice_level) {
  return request({
    url: '/BaseFaultNoticecfg/GetByNoticeLevelAndFaultId',
    method: 'post',
    params: {
      fault_id: fault_id,
      notice_level: notice_level
    }
  })
}
