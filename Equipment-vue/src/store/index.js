/*
 * @Author: your name
 * @Date: 2021-09-16 09:29:09
 * @LastEditTime: 2021-11-26 15:24:49
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \Mes_Vue\src\store\index.js
 */
import Vue from 'vue'
import Vuex from 'vuex'
import app from './modules/app'
// import serverConf from './modules/serverConf'
import user from './modules/user'
// import tagsView from './modules/tagsView'
import permission from './modules/permission'
// import dataPrivilegerules from './modules/dataPrivilegerules'
import storage from './modules/storage'
import getters from './getters'
import tagsView from './modules/tagsView'

Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
    app,
    user,
    storage,
    tagsView,
    permission
  },
  getters
})

export default store
