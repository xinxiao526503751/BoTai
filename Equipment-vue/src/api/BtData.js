/*
 * @Author: your name
 * @Date: 2021-07-15 09:26:51
 * @LastEditTime: 2021-09-08 21:10:25
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\BtData.js
 */
import request from '@/utils/request'

// 得到总装线数据
export function GetPlcMacDataList(sitname,bgdate,eddate,pagenum,pagerows) {
  return request({
    url: '/BtData/GetPlcMacDataList',
    method: 'post',
    params: {
      sitname:sitname,
      bgdate:bgdate,
      eddate:eddate,
      pagenum:pagenum,
      pagerows:pagerows,
    }
  })
}
// 得到原始总装线数据
export function GetPlcOrcMacDataList(sitname,bgdate,eddate,pagenum,pagerows) {
  return request({
    url: '/BtData/GetPlcOrcMacDataList',
    method: 'post',
    params: {
      sitname:sitname,
      bgdate:bgdate,
      eddate:eddate,
      pagenum:pagenum,
      pagerows:pagerows,
    }
  })
}

// 得到用户列表数据
export function GetCutMacDataList(devname, bgdate, eddate, pagenum, pagerows) {
  return request({
    url: '/BtData/GetCutMacDataList',
    method: 'post',
    params: {
      devname: devname,
      bgdate: bgdate,
      eddate: eddate,
      pagenum: pagenum,
      pagerows: pagerows
    }
  })
}