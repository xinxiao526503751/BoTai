/*
 * @Author: your name
 * @Date: 2021-06-17 14:15:01
 * @LastEditTime: 2022-01-10 15:00:05
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\utils\request.js
 */
import axios from 'axios'
import { Message, MessageBox } from 'element-ui'
import store from '../store'
import { getToken } from '@/utils/auth'

// 创建axios实例(axios二次封装)
const service = axios.create({
  baseURL: process.env.BASE_API, // api的base_url
  timeout: 50000 // 请求超时时间
})

// request拦截器
service.interceptors.request.use(
  config => {
    // 打开可能会报错 x-token不被允许
    // config.headers['x-token'] = 'x-xxxxxxxxxxxxxxxxxxx';
    if (store.getters.token) {
      config.headers.Authorization = getToken() // 让每个请求携带自定义token 请根据实际情况自行修改
    }
    return config
  },
  error => {
    // 请求错误做些事
    return Promise.reject(error)
  }
)

// response拦截器
service.interceptors.response.use(
  response => {
    /**
     * code为非200是抛错 可结合自己业务进行修改
     */
    const res = response.data
    console.log(res)
    // console.log(res.Code)
    if (res.Code !== 200) {
      // 50008:非法的token; 50012:其他客户端登录了;  50014:Token 过期了;
      if (res.Code === 50008 || res.Code === 50012 || res.Code === 50014) {
        MessageBox.confirm(
          '登录已超时，可以【取消】继续留在该页面，或者【重新登录】',
          '超时提醒',
          {
            confirmButtonText: '重新登录',
            cancelButtonText: '取消',
            type: 'warning'
          }
        ).then(() => {
          // store.dispatch('FedLogOut').then(() => {
          location.reload() // 为了重新实例化vue-router对象 避免bug
          // })
        })
      } else {
        Message({
          message: res.Message,
          type: 'error',
          duration: 2 * 1000
        })
      }
      return Promise.reject('error')
    } else {
      return response.data
    }
  },
  error => {
    console.log('err' + error) // for debug
    Message({
      message: '请刷新本页面，异常详情：' + error.message,
      type: 'error',
      duration: 5 * 1000
    })
    return Promise.reject(error)
  }
)

export default service
