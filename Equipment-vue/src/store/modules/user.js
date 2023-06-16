import {
  login,
  logout,
  getInfo,
  getModules
  // getModulesTree,
  // getOrgs
} from '@/api/Login'
import { getToken, setToken, removeToken } from '@/utils/auth'

const user = {
  state: {
    //用来存储数据
    token: getToken(),
    name: '',
    modules: null
  },

  mutations: {
    //用来更改数据的状态
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_NAME: (state, name) => {
      state.name = name
    },
    SET_MODULES: (state, modules) => {
      state.modules = modules
    }
  },
  //actions中负责处理逻辑，将结果传入mutations(触发commit),再更新state中的对象
  actions: {
    // 登录
    Login({ commit }, userInfo) {
      const username = userInfo.username.trim()
      return new Promise((resolve, reject) => {
        login(username, userInfo.password)
          .then(response => {
            const data = response
            setToken(data.Token) //登录成功后将token存储在cookie之中
            commit('SET_TOKEN', data.Token)
            this.state.token = data.Token
            resolve()
          })
          .catch(error => {
            reject(error)
          })
      })
    },

    // 获取用户信息
    GetInfo({ commit, state }) {
      return new Promise((resolve, reject) => {
        getInfo(state.token)
          .then(response => {
            commit('SET_NAME', response.Result)
            resolve(response)
          })
          .catch(error => {
            reject(error)
          })
      })
    },
    // 获取用户模块
    GetModules({ commit, state }) {
      return new Promise((resolve, reject) => {
        getModules(state.token)
          .then(response => {
            commit('SET_MODULES', response.Result)
            resolve(response)
          })
          .catch(error => {
            reject(error)
          })
      })
    },
    // 获取用户模块
    GetModulesTree({ commit, state }) {
      return new Promise((resolve, reject) => {
        if (state.modules != null) {
          resolve(state.modules)
          return
        }
        getModulesTree(state.token)
          .then(response => {
            commit('SET_MODULES', response.Result)
            resolve(response.Result)
          })
          .catch(error => {
            reject(error)
          })
      })
    },
    // 登出
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        logout(state.token)
          .then(() => {
            commit('SET_TOKEN', '')
            commit('SET_MODULES', [])
            removeToken()
            resolve()
          })
          .catch(error => {
            reject(error)
          })
      })
    },

    // 前端 登出
    FedLogOut({ commit }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '')
        removeToken()
        resolve()
      })
    }
  }
}

export default user
