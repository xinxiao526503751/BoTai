/*
 * @Author: your name
 * @Date: 2021-07-21 21:04:32
 * @LastEditTime: 2022-01-17 10:50:15
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BaseExamItem.js
 */
import request from '@/utils/request'

// 新增挂载在设备下的点检定义
export function CreateItem(data) {
  return request({
    url: '/ExamPlanRecItem/CreateItem',
    method: 'post',
    data
  })
}
export function CreateRecAndItem(data) {
  return request({
    url: '/ExamPlanRecItem/CreateRecAndItem',
    method: 'post',
    data
  })
}
export function GetExamPlanRecItemByRecId(RecId, res) {
  return request({
    url: '/ExamPlanRecItem/GetExamPlanRecItemByRecId',
    method: 'post',
    params: {
      RecId: RecId,
      res: res
    }
  })
}
export function qualifyAll(RecId) {
  return request({
    url: '/ExamPlanRecItem/qualifyAll',
    method: 'post',
    params: {
      RecId: RecId
    }
  })
}
