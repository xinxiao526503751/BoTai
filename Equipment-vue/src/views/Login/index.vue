<template>
    <div class="login_contauner">
      <div class="text">设备管理平台</div>
      <div class="login_box">
        <div class="login">登录</div>
        <el-form
          ref="loginForm"
          :model="loginForm"
          :rules="loginRules"
          label-width="0px"
          class="login_form"
        >
          <!-- 用户名 v-model将用户名绑定到username上，-->
          <el-form-item prop="username">
            <el-input
              type="text"
              autoComplete="on"
              placeholder="用户名"
              v-model="loginForm.username"
              prefix-icon="el-icon-user-solid"
            ></el-input>
          </el-form-item>
          <!-- 密码 -->
          <el-form-item prop="password">
            <el-input
              name="password"
              :type="passwordType"
              @keyup.enter.native="handleLogin"
              autoComplete="on"
              placeholder="密码"
              v-model="loginForm.password"
              prefix-icon="el-icon-lock"
            ></el-input>
          </el-form-item>
          <!-- 按钮区域 ，加居右位置class-->
          <el-form-item class="btns">
            <el-button
              style=" background-color:#409EFF; color:#fff"
              @click="handleLogin"
              size="small"
              >登录</el-button
            >
            <el-button type="info" @click="resetLoginForm" size="small"
              >重置</el-button
            >
          </el-form-item>
        </el-form>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      const validateUsername = (rule, value, callback) => {
        if (value.length <= 0) {
          callback(new Error('用户名不能为空'))
        } else {
          callback()
        }
      }
      const validatePass = (rule, value, callback) => {
        if (value.length <= 0) {
          callback(new Error('密码不能为空'))
        } else if (value.length < 6) {
          callback(new Error('长度在 6 到 15 个字符'))
        } else {
          callback()
        }
      }
      return {
        loginForm: {
          username: '',
          password: ''
        },
        loginRules: {
          username: [
            {
              required: true,
              trigger: 'blur',
              validator: validateUsername
            }
          ],
          password: [
            {
              required: true,
              trigger: 'blur',
              validator: validatePass
            }
          ]
        },
        loading: false,
        passwordType: 'password'
      }
    },
    methods: {
      showPwd() {
        if (this.pwdType === 'password') {
          this.pwdType = ''
        } else {
          this.pwdType = 'password'
        }
      },
      handleLogin() {
        this.$refs.loginForm.validate(valid => {
          if (valid) {
            this.loading = true
            this.$store
              .dispatch('Login', this.loginForm) //把Login分发到actions，在通过actions提交到mutation执行
              .then(() => {
                this.loading = false
                this.$router.push({
                  path: '/equipmentInformation' //登录成功之后重定向到首页，实现页面跳转
                })
              })
              .catch(() => {
                this.loading = false
              })
          } else {
            this.$message.error('登录失败') //登录失败提示错误
            return false
          }
        })
      },
      //点击重置按钮，重置登录表单
      resetLoginForm() {
        //这里的this就指向当前登录组件的实例对象
        //resetFields 清除表单数据
        this.$refs.loginForm.resetFields()
      }
    }
  }
  </script>
  <style lang="scss" scoped>
  .login_contauner {
    background: url('~@/assets/login.jpg') no-repeat center center;
    background-size: cover;
    background-attachment: fixed;
    width: 100%;
    background-color: #2b4b6b;
    height: 100%;
    .text {
      position: absolute;
      margin: 55px 0 0 207px;
      width: 372px;
      height: 52px;
      font-size: 36px;
      font-family: SourceHanSansSC-Medium, SourceHanSansSC;
      font-weight: 500;
      color: #ffffff;
      line-height: 52px;
    }
  }
  .login_box {
    width: 328px;
    height: 283px;
    background-color: #fff;
    border-radius: 3px;
    position: absolute;
    left: 65%;
    top: 50%;
    transform: translate(-50%, -50%);
    .login {
      // width: 56px;
      // height: 31px;
      margin-top: 30px;
      font-size: 24px;
      font-family: MicrosoftYaHei;
      color: #0080ff;
      line-height: 31px;
      text-align: center;
    }
    /* 语法嵌套 */
  }
  
  .login_form {
    // position: absolute;
    // margin-top: 50px;
    bottom: 0;
    width: 100%;
    /* // 边框距离：上下和左右 */
    padding: 30px 20px;
    box-sizing: border-box;
  }
  .avatar_box {
    height: 190px;
    width: 195px;
    /* // 给外边的盒子加一个边框线 */
    border: 1px solid #eee;
    border-radius: 50%;
    /* // 让边框和图片有一个间距 */
    padding: 10px;
    /* // 加阴影 */
    box-shadow: 0 0 10px #ddd;
    /* //margin: auto;居中 */
    position: absolute;
    left: 50%;
    /* //横向位移距离，纵向位移距离 */
    transform: translate(-50%, -50%);
    /* //边框之间的空白加白色 */
    background-color: #fff;
  }
  img {
    width: 85%;
    height: 80%;
    transform: translate(10%, 12%);
    border-radius: 10%;
    background-color: #fff;
  }
  
  .btns {
    /* //变成弹力和模型 */
    display: flex;
    /* //横轴尾部对齐 */
    justify-content: flex-end;
    margin-top: 30px;
  }
  </style>
  