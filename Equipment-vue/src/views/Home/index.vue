<template>
  <div class="home">
    <div class="header">
      <Header></Header>
      <!-- <el-button
        class="btn"
        @click="onClick"
        size="medium"
        >进入主页</el-button
      > -->
    </div>
    <div class="body">
      <div class="left">
        <div class="top">
          <div class="list-box">
            <el-row class="header-title" :gutter="20">
              <el-col :span="7">计划ID</el-col>
              <el-col :span="5">设备名称</el-col>
              <el-col :span="7">点检项名称</el-col>
              <el-col :span="5">点检结果</el-col>
            </el-row>
            <el-scrollbar style="height:380px" class="scroller">
              <vue-seamless-scroll :data="checkDataList" class="seamless-warp">
                <el-row v-for="(item, index) in checkDataList" :key="index">
                  <el-col :span="7">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.PLAN_ID)"
                      placement="top"
                    >
                      <span>{{ item.PLAN_ID }}</span>
                    </el-tooltip>
                  </el-col>
                  <el-col :span="5">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.EQU_NAME)"
                      placement="top"
                    >
                      <span>{{ item.EQU_NAME }}</span>
                    </el-tooltip>
                  </el-col>
                  <el-col :span="7">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.CHECK_ITEM_NAME)"
                      placement="top"
                    >
                      <span>{{ item.CHECK_ITEM_NAME }}</span>
                    </el-tooltip>
                  </el-col>
                  <el-col :span="5">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.CHECK_RESULT)"
                      placement="top"
                    >
                      <span>{{ item.CHECK_RESULT }}</span>
                    </el-tooltip>
                  </el-col>
                </el-row>
              </vue-seamless-scroll>
            </el-scrollbar>
          </div>
          <div class="foot-line"></div>
        </div>
        <div class="center">
          <div class="message">
            点检结果：
            <span
              >已完成 &nbsp; {{ this.qualifiedNum }} / {{ this.total }}</span
            >
            <span>&nbsp; 合格：{{ this.qualifiedNum }}</span>
            <span>&nbsp; 不合格：{{ this.unqualifiedNum }}</span>
          </div>
          <div class="foot-line"></div>
        </div>
        <div class="buttom">
          <div class="leftChart"></div>
          <div class="foot-line"></div>
        </div>
      </div>
      <div class="center">
        <div class="top">
          <div class="message">
            保养结果：
            <span
              >已完成 &nbsp; {{ this.qualifiedNum1 }} / {{ this.total1 }}</span
            >
            <span>&nbsp; 完成：{{ this.qualifiedNum1 }}</span>
            <span>&nbsp; 未完成：{{ this.unqualifiedNum1 }}</span>
          </div>
          <div class="foot-line"></div>
        </div>
        <div class="center">
          <div class="centerChart"></div>
          <div class="foot-line"></div>
        </div>
        <div class="buttom">
          <div class="list-box">
            <el-row class="header-title" :gutter="20">
              <el-col :span="7">计划ID</el-col>
              <el-col :span="5">设备名称</el-col>
              <el-col :span="7">保养项名称</el-col>
              <el-col :span="5">保养结果</el-col>
            </el-row>
            <el-scrollbar style="height: 380px" class="scroller">
              <vue-seamless-scroll :data="upkeepDataList" class="seamless-warp">
                <el-row v-for="(item, index) in upkeepDataList" :key="index">
                  <el-col :span="7">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.PLAN_ID)"
                      placement="top"
                    >
                      <span>{{ item.PLAN_ID }}</span>
                    </el-tooltip>
                  </el-col>
                  <el-col :span="5">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.EQU_NAME)"
                      placement="top"
                    >
                      <span>{{ item.EQU_NAME }}</span>
                    </el-tooltip>
                  </el-col>
                  <el-col :span="7">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.UPKEEP_ITEM_NAME)"
                      placement="top"
                    >
                      <span>{{ item.UPKEEP_ITEM_NAME }}</span>
                    </el-tooltip>
                  </el-col>
                  <el-col :span="5">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.UPKEEP_RESULT)"
                      placement="top"
                    >
                      <span>{{ item.UPKEEP_RESULT }}</span>
                    </el-tooltip>
                  </el-col>
                </el-row>
              </vue-seamless-scroll>
            </el-scrollbar>
          </div>
          <div class="foot-line"></div>
        </div>
      </div>
      <div class="right">
        <div class="top">
          <div class="list-box">
            <el-row class="header-title" :gutter="20">
              <el-col :span="9">设备名称</el-col>
              <el-col :span="9">维修项名称</el-col>
              <el-col :span="6">维修结果</el-col>
            </el-row>
            <el-scrollbar style="height: 380px" class="scroller">
              <vue-seamless-scroll
                :data="maintainDataList"
                class="seamless-warp"
              >
                <el-row v-for="(item, index) in maintainDataList" :key="index">
                  <el-col :span="9">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.EQU_NAME)"
                      placement="top"
                    >
                      <span>{{ item.EQU_NAME }}</span>
                    </el-tooltip>
                  </el-col>
                  <el-col :span="9">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.MAINTAIN_ITEM_NAME)"
                      placement="top"
                    >
                      <span>{{ item.MAINTAIN_ITEM_NAME }}</span>
                    </el-tooltip>
                  </el-col>
                  <el-col :span="6">
                    <el-tooltip
                      class="item"
                      effect="dark"
                      :content="String(item.MAINTAIN_RESULT)"
                      placement="top"
                    >
                      <span>{{ item.MAINTAIN_RESULT }}</span>
                    </el-tooltip>
                  </el-col>
                </el-row>
              </vue-seamless-scroll>
            </el-scrollbar>
          </div>
          <div class="foot-line"></div>
        </div>
        <div class="center">
          <div class="message">
            维修结果：
            <span
              >已完成 &nbsp; {{ this.qualifiedNum2 }} / {{ this.total2 }}</span
            >
            <span>&nbsp; 完成：{{ this.qualifiedNum2 }}</span>
            <span>&nbsp; 未完成：{{ this.unqualifiedNum2 }}</span>
          </div>
          <div class="foot-line"></div>
        </div>
        <div class="buttom">
          <div class="rightChart"></div>
          <div class="foot-line"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Header from '@/views/Layout/Header'
import * as echarts from 'echarts'
import * as EquCheckItem from '@/api/EquCheckItem'
import * as EquUpkeepItem from '@/api/EquUpkeepItem'
import * as EquMiantainItem from '@/api/EquMaintainItem'
import vueSeamlessScroll from 'vue-seamless-scroll'
export default {
  name: 'Home',
  components: {
    Header,
    vueSeamlessScroll
  },
  data() {
    return {
      checkDataList: [],
      upkeepDataList: [],
      maintainDataList: [],
      intervalId: null,
      intervalId1: null,
      intervalId2: null,
      total: 0,
      total1: 0,
      total2: 0,
      qualifiedNum: 0,
      unqualifiedNum: 0,
      qualifiedNum1: 0,
      unqualifiedNum1: 0,
      qualifiedNum2: 0,
      unqualifiedNum2: 0,
      windowWidth: 0,
      windowHeight: 0
    }
  },
  created() {
    this.getDataAndStartRolling()
    this.getUpkeepDataStartRolling()
    this.getMaintainDaraStartRolling()
  },
  mounted() {
    this.getEchart()
    this.getEchart2()
    this.getEchart3()
    this.$nextTick(() => {
      window.addEventListener('resize', this.handleResize, { passive: true })
    })
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.handleResize)
  },
  computed: {
    //滚动表格的配置项
    optionHover() {
      return {
        step: 0.4, // 数值越大速度滚动越快
        limitMoveNum: 5, // 开始无缝滚动的数据量 this.dataList.length
        hoverStop: true, // 是否开启鼠标悬停stop
        direction: 1, // 0向下 1向上 2向左 3向右
        openWatch: false, // 开启数据实时监控刷新dom
        singleHeight: 0, // 单步运动停止的高度(默认值0是无缝不停止的滚动) direction => 0/1
        singleWidth: 0, // 单步运动停止的宽度(默认值0是无缝不停止的滚动) direction => 2/3
        waitTime: 1000 // 单步运动停止的时间(默认值1000ms)
      }
    }
  },
  methods: {
    // onClick() {
    //   this.$router.push({
    //     path: 'equipmentInformation'
    //   })
    // },
    handleResize() {
      this.windowWidth = window.innerWidth
      this, (this.windowHeight = window.innerHeight)
      location.reload()
    },
    // 获取点检数据
    getAllCheckItemList() {
      this.qualifiedNum = 0
      this.unqualifiedNum = 0
      EquCheckItem.GetAll().then((response) => {
        if (response.Code === 200) {
          this.checkDataList = response.Data
          this.total = response.Total
          response.Data.forEach((element) => {
            if (element.CHECK_RESULT === '合格') {
              this.qualifiedNum++
            } else {
              this.unqualifiedNum++
            }
          })
        }
      })
    },
    // 获取保养数据
    getAllUpkeepItemList() {
      this.qualifiedNum1 = 0
      this.unqualifiedNum1 = 0
      EquUpkeepItem.GetAll().then((response) => {
        if (response.Code === 200) {
          this.upkeepDataList = response.Data
          this.total1 = response.Total
          response.Data.forEach((element) => {
            if (element.UPKEEP_RESULT === '完成') {
              this.qualifiedNum1++
            } else {
              this.unqualifiedNum1++
            }
          })
        }
      })
    },
    // 获取维修数据
    getAllMaintainItemList() {
      this.qualifiedNum2 = 0
      this.unqualifiedNum2 = 0
      EquMiantainItem.GetAll().then((response) => {
        if (response.Code === 200) {
          this.maintainDataList = response.Data
          this.total2 = response.Total
          response.Data.forEach((element) => {
            if (element.MAINTAIN_RESULT === '完成') {
              this.qualifiedNum2++
            } else {
              this.unqualifiedNum2++
            }
          })
        }
      })
    },
    getDataAndStartRolling() {
      this.getAllCheckItemList() // 首次加载数据
      this.intervalId = setInterval(() => {
        this.getAllCheckItemList() // 定时更新数据
      }, 10000) // 每 10 秒获取一次数据
    },
    getUpkeepDataStartRolling() {
      this.getAllUpkeepItemList() // 首次加载数据
      this.intervalId1 = setInterval(() => {
        this.getAllUpkeepItemList() // 定时更新数据
      }, 10000) // 每 10 秒获取一次数据
    },
    getMaintainDaraStartRolling() {
      this.getAllMaintainItemList() // 首次加载数据
      this.intervalId2 = setInterval(() => {
        this.getAllMaintainItemList() // 定时更新数据
      }, 10000) // 每 10 秒获取一次数据
    },
    getEchart() {
      var chartDom = document.querySelector('.leftChart')
      var myChart = echarts.init(chartDom)
      var option
      ;(option = {
        title: {
          text: '设备点检合格率',
          left: 'center',
          textStyle:{
            color:'#fff'
          }
        },
        color:['#9fe080','#ff7070'],
        tooltip: {
          trigger: 'item',
          formatter: '{b}<br/>{a}: {c} ({d}%)<br/>'
        },
        legend: {
          orient: 'vertical',
          left: 'left',
          data:['合格','不合格'],
          textStyle:{
            color:'#fff'
          }
        },
        series: [
          {
            name: 'Percent Of Pass',
            type: 'pie',
            center: ['50%', '50%'],
            radius: '60%',
            data: [
              { value: this.qualifiedNum, name: '合格'},
              { value: this.unqualifiedNum, name: '不合格'}
            ],
            label:{
              textStyle:{
                color:'#fff'
              }
            },
            emphasis: {
              itemStyle: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              }
            }
          }
        ]
      }),
        option && myChart.setOption(option)
      // 监听qualifiedNum和unqialifiedNum的变化，更新echart
      this.$watch(
        () => [this.qualifiedNum, this.unqualifiedNum],
        () => {
          myChart.setOption({
            series: [
              {
                data: [
                  { value: this.qualifiedNum, name: '合格' },
                  { value: this.unqualifiedNum, name: '不合格' }
                ]
              }
            ]
          })
        }
      )
    },
    getEchart2() {
      var chartDom = document.querySelector('.centerChart')
      var myChart2 = echarts.init(chartDom)
      var option

      option = {
        title: {
          text: '设备保养完成率',
          left: 'center',
          textStyle:{
            color:'#fff'
          }
        },
        legend: {
          top: 'bottom',
          data:['完成','未完成'],
          textStyle:{
            color:'#fff'
          }
        },
        toolbox: {
          show: true,
          feature: {
            mark: { show: true },
            saveAsImage: { show: true }
          }
        },
        color:['#9fe080','#ff7070'],
        series: [
          {
            name: 'Finishing Rate',
            type: 'pie',
            radius: [60, 120],
            center: ['50%', '40%'],
            roseType: 'area',
            itemStyle: {
              borderRadius: 8
            },
            data: [
              { value: this.qualifiedNum1, name: '完成' },
              { value: this.unqualifiedNum1, name: '未完成' }
            ]
          }
        ]
      }

      option && myChart2.setOption(option)

      this.$watch(
        () => [this.qualifiedNum1, this.unqualifiedNum1],
        () => {
          myChart2.setOption({
            series: [
              {
                data: [
                  { value: this.qualifiedNum, name: '完成' },
                  { value: this.unqualifiedNum, name: '未完成' }
                ]
              }
            ]
          })
        }
      )
    },
    getEchart3() {
      var chartDom = document.querySelector('.rightChart')
      var myChart = echarts.init(chartDom)
      var option
      ;(option = {
        title: {
          text: '设备维修完成率',
          left: 'center',
          textStyle:{
            color:'#fff'
          }
        },
        tooltip: {
          trigger: 'item',
          formatter: '{b}<br/>{a}: {c} ({d}%)<br/>'
        },
        color:['#9fe080','#ff7070'],
        legend: {
          orient: 'vertical',
          left: 'left',
          data:['完成','未完成'],
          textStyle:{
            color:'#fff'
          }
        },
        series: [
          {
            name: 'Percent Of Pass',
            type: 'pie',
            center: ['50%', '50%'],
            radius: '60%',
            data: [
              { value: this.qualifiedNum2, name: '完成' },
              { value: this.unqualifiedNum2, name: '未完成' }
            ],
            emphasis: {
              itemStyle: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              }
            }
          }
        ]
      }),
        option && myChart.setOption(option)
      this.$watch(
        () => [this.qualifiedNum2, this.unqualifiedNum2],
        () => {
          myChart.setOption({
            series: [
              {
                data: [
                  { value: this.qualifiedNum2, name: '完成' },
                  { value: this.unqualifiedNum2, name: '未完成' }
                ]
              }
            ]
          })
        }
      )
    },
    headClass() {
      return 'background:#337ab7'
    }
  },
  destroyed() {
    clearInterval(this.intervalId)
    clearInterval(this.intervalId1)
    clearInterval(this.intervalId2)
  }
}
</script>

<style lang="scss" scoped>
.home {
  width: 100%;
  background: linear-gradient(to bottom right, #050506, #180b7e);
}
.btn{
  position:absolute;
  top: 10px;
  right: 280px;
  color:#fff;
  background: linear-gradient(blue,red);
  font-size: 16px;
}
.header {
  width: 100%;
  height: 54px;
}
.body {
  display: flex;
  width: 100%;
  height: calc(100% - 54px);
}
.el-scrollbar__wrap{
  height: 100% !important;
}
.body .left {
  width: 30%;
  display: flex;
  flex-direction: column;
  padding: 5px;
}
.body .left .top{
  flex: 1;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin-bottom: 10px;
  z-index: 10;
}
.body .left .top .foot-line,
.body .left .center .foot-line,
.body .left .buttom .foot-line,
.body .center .top .foot-line,
.body .center .center .foot-line,
.body .center .buttom .foot-line,
.body .right .top .foot-line,
.body .right .center .foot-line,
.body .right .buttom .foot-line{
  position:absolute;
  bottom:0;
  width: 100%;
  left: 0;
}
.body .left .top .foot-line:before,
.body .left .center .foot-line:before,
.body .left .buttom .foot-line:before,
.body .center .top .foot-line:before,
.body .center .center .foot-line:before,
.body .center .buttom .foot-line:before,
.body .right .top .foot-line:before,
.body .right .center .foot-line:before,
.body .right .buttom .foot-line:before{
  border-left: 2px solid #02a6b5;
  left:0;
  position:absolute;
  width: 10px;
  height: 10px;
  content: "";
  border-bottom: 2px solid #02a6b5;
  bottom:0;
}
.body .left .top .foot-line:after,
.body .left .center .foot-line:after,
.body .left .buttom .foot-line:after,
.body .center .top .foot-line:after,
.body .center .center .foot-line:after,
.body .center .buttom .foot-line:after,
.body .right .top .foot-line:after,
.body .right .center .foot-line:after,
.body .right .buttom .foot-line:after{
  border-right: 2px solid #02a6b5;
  right:0;
  position:absolute;
  width: 10px;
  height: 10px;
  content: "";
  border-bottom: 2px solid #02a6b5;
  bottom:0;
}
.body .left .top:before,
.body .left .center:before,
.body .left .buttom:before,
.body .center .top:before,
.body .center .center:before,
.body .center .buttom:before,
.body .right .top:before,
.body .right .center:before,
.body .right .buttom:before{
  border-left: 2px solid #02a6b5;
  left:0;
  position:absolute;
  width: 10px;
  height: 10px;
  content: "";
  border-top: 2px solid #02a6b5;
  top:0;
}
.body .left .top:after,
.body .left .center:after,
.body .left .buttom:after,
.body .center .top:after,
.body .center .center:after,
.body .center .buttom:after,
.body .right .top:after,
.body .right .center:after,
.body .right .buttom:after{
  border-right: 2px solid #02a6b5;
  right:0;
  position:absolute;
  width: 10px;
  height: 10px;
  content: "";
  border-top: 2px solid #02a6b5;
  top:0;
}
// .body .left .top .leftTop {
//   width: 100%;
//   height: 300px;
// }
.body .left .center{
  flex:1;
  width: 100%;
  height: 58px;
  line-height: 58px;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin: 10px 0;
  z-index: 10;
}
.body .left .buttom {
  flex:1;
  width: 100%;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin: 10px 0;
  z-index: 10;
}
.body .center {
  width: 40%;
  display: flex;
  flex-direction: column;
  padding: 5px;
}

.body .center .top {
  flex:1;
  width: 100%;
  height: 58px;
  line-height: 58px;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin: 0 0 10px;
  z-index: 10;
}
.body .center .center{
  flex:1;
  width: 100%;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin: 10px 0;
  z-index: 10;
}

.body .center .buttom{
  flex:1;
  width: 100%;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin: 10px 0;
  z-index: 10;
}
.body .right {
  width: 30%;
  display: flex;
  flex-direction: column;
  padding: 5px;
}

.body .right .top {
  flex:1;
  width: 100%;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin: 0 0 10px;
  z-index: 10;
}
.body .right .center{
  flex:1;
  width: 100%;
  height: 58px;
  line-height: 58px;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin: 10px 0;
  z-index: 10;
}
.body .right .buttom {
  flex:1;
  width: 100%;
  position: relative;
  border:1px solid rgba(25,186,139,.17);
  background: hsla(0, 0%, 100%, .04);
  background-size: 100% auto;
  padding: 5px 5px;
  margin: 10px 0;
  z-index: 10;
}

@media screen and (max-width: 768px) {
  /* 屏幕宽度小于 768px 时的样式 */
  .body {
    flex-direction: column;
  }
}
.list-box {
  width: 100%;
  height: 100%;
  height: auto;
  /*border: 1px solid red;*/
  position: relative;
  text-align: center;

  .header-title {
    position: relative;
    z-index: 999;
    top: 0;
    background-color: #3265bd !important;
  }

  .el-row {
    display: flex;
    justify-content: space-around;
    border-bottom: 1px solid #0c65fc;
    margin: 0 !important;
  }

  .el-col {
    padding: 0 !important;
  }

  .el-row:nth-child(2n-1) {
    background: #0e2c75;
    /*border-bottom: 1px solid #0c65fc;*/
  }

  .el-col {
    width: 100%;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
    height: 40px;
    line-height: 40px;
    color: #fff;
    font-size: 12px;
  }
}
.message {
  padding: 10px 5px;
  font-size: 18px;
  text-align: center;
  color: #fff;
}
.leftChart {
  margin-top: 5px;
  padding: 5px;
  width: 100%;
  height: 430px;
}
.centerChart {
  margin-top: 5px;
  padding: 5px;
  width: 100%;
  height: 430px;
}
.rightChart {
  margin-top: 5px;
  padding: 5px;
  width: 100%;
  height: 430px;
}
</style>