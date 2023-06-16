/*
 * @Author: your name
 * @Date: 2021-09-02 17:12:09
 * @LastEditTime: 2021-09-02 17:15:11
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\FaultRecord.js
 */
import request from '@/utils/request'

// 上报事件,生成事件记录的同时生成事件记录流程
export function ReportFault(data) {
  return request({
    url: '/FaultRecord/ReportFault',
    method: 'post',
    data
  })
}

// 请求已有的异常事件记录,可根据设备名称分类查询
export function FaultRecordRequest(equipment_id) {
  return request({
    url: '/FaultRecord/FaultRecordRequest',
    method: 'post',
    params: {
      equipment_id: equipment_id
    }
  })
}
