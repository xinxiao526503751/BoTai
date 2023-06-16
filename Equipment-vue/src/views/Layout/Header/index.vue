<template>
  <div>
    <el-menu class="navbar" mode="horizontal">
      <div class="left-logo">
        <span>
          <img src="@/assets/logo2.png" alt="logo">
        </span>
      </div>
      <div class="word">{{ title }}</div>
      <div class="right-message">
        <span class="time">
          {{ nowTime }}
        </span>
        <span class="line">|</span>
        <span class="el-icon-switch-button" @click="onBack"></span>
      </div>
    </el-menu>
  </div>
</template>
  <script>
import * as Login from '@/api/Login'
import dayjs from 'dayjs'
export default {
  name: 'Header',
  data() {
    return {
      nowTime: '',
      title:'设备管理系统'
    }
  },
  mounted() {
  },
  methods: {
    // 注销
    onBack() {
      this.$store.dispatch('LogOut').then(()=>{
        location.reload()
      })
      window.sessionStorage.clear()
      this.$router.push('/login')
    }
  },
  watch:{
    // '$route.path'(newPath,oldPath){
    //   console.log('----路径发生变化',newPath)
    //   if(newPath==='/'){
    //     this.title='大屏数据显示分析'
    //   }else{
    //     this.title='设备管理系统'
    //   }
    // }
  },
  // 当页面生成的时候就格式化获取的时间渲染到页面上
  created() {
    let _this=this;
    this.timer=setInterval(()=>{
      _this.nowTime=dayjs(new Date()).format('YYYY / MM / DD  HH : mm : ss');
    },1000)
  },
  // 销毁定时器
  beforeDestroy(){
    if(this.timer){
      clearInterval(this.timer);
    }
  }
}
</script>
  <style rel="stylesheet/scss" lang="scss" scoped>
.navbar {
  position: relative;
  height: 54px;
  line-height: 54px;
  border-bottom: 0 !important;
  background: url('~@/assets/headBg.png') no-repeat center center;
  background-size: cover;
  background-color: #112364;
  .left-logo{
    position: absolute;
    left: 20px;
    width: 115px;
    height: 54px;
    span img{
      width: 100%;
      height: 100%;
    }
  }
  .word {
    position: absolute;
    width: 100px;
    left: 47%;
    color: white;
    font-size: 18px;
    letter-spacing: 5px;
    white-space: nowrap;
    line-height: 44px;
    text-align: center;
    text-shadow: 0px 0px 36px rgba(56, 117, 119, 0.84);
  }
  .right-message {
    text-align: right;
    font-size: 16px;
    padding-right: 15px;
    color: #fff;
    .line {
      font-size: 16px;
      margin: 10px;
    }
  }
}
</style>
  