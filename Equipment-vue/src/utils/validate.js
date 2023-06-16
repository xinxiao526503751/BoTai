/*
 * @Author: your name
 * @Date: 2021-08-18 11:00:04
 * @LastEditTime: 2021-11-22 16:17:03
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\utils\validate.js
 */
// 校验模块
export default {
  // 中文校验
  validateNoChi: (rule, value, callback) => {
    if (value) {
      if (/[\u4E00-\u9FA5]/g.test(value)) {
        callback(new Error('不能为中文!'))
      } else {
        callback()
      }
    }
    callback()
  },
  // 密码校验
  validatePsdReg: (rule, value, callback) => {
    if (!value) {
      return callback(new Error('请输入密码'))
    }
    if (
      !/^(?![\d]+$)(?![a-zA-Z]+$)(?![^\da-zA-Z]+$)([^\u4e00-\u9fa5\s]){6,20}$/.test(
        value
      )
    ) {
      callback(
        new Error(
          '请输入6-20位英文字母、数字或者符号（除空格），且字母、数字和标点符号至少包含两种'
        )
      )
    } else {
      callback()
    }
  },
  // 手机校验
  validateMobile: (rule, value, callback) => {
    if (value === '') {
      callback()
    } else {
      if (value !== '') {
        var reg = /^1[3456789]\d{9}$/
        if (!reg.test(value)) {
          callback(new Error('请输入有效的手机号码'))
        }
      }
      callback()
    }
  },
  // 邮箱校验
  validateEmail: (rule, value, callback) => {
    if (value === '') {
      callback()
    } else {
      if (value !== '') {
        var reg = /^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/
        if (!reg.test(value)) {
          callback(new Error('请输入有效的邮箱'))
        }
      }
      callback()
    }
  },
  // 固话验证
  validatePhone: (rule, value, callback) => {
    if (value === '') {
      callback()
    } else {
      if (value !== '') {
        var reg = /^((0\d{2,3}-\d{7,8})|(1[3584]\d{9}))$/
        if (!reg.test(value)) {
          callback(
            new Error('请输入正确的手机号或者座机号格式为：0000-0000000')
          )
        }
      }
      callback()
    }
  },
  // 数字验证
  validateNum: (rule, value, callback) => {
    const reg = /^[0-9]*$/
    if (!reg.test(value)) {
      callback(new Error('只能为数字'))
    } else {
      callback()
    }
  }
}
