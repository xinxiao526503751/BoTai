/*
 * @Author: your name
 * @Date: 2021-06-11 15:09:17
 * @LastEditTime: 2022-08-06 22:09:42
 * @LastEditors: chenyun625 chenyun625@outlook.com
 * @Description: In User Settings Edit
 * @FilePath: \mes\config\dev.env.js
 */
'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')

// 判断为生产环境还是开发环境
module.exports = merge(prodEnv, {
  NODE_ENV: '"development"',
  // BASE_API: '"http://192.168.146.140:8001/cutter"'
  BASE_API: '"http://localhost:8081/api"'
})
