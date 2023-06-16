<!--
 * @Author: gcy
 * @Date: 2021-09-28 09:52:54
 * @LastEditTime: 2021-10-08 11:27:25
 * @LastEditors: Please set LastEditors
 * @Description: 单位下拉框
 * @FilePath: \Mes_Vue\src\components\UnitSelect\index.vue
-->
<template>
  <!-- <div class="select"> -->
  <el-select
    size="mini"
    v-model="parm"
    placeholder="请选择单位"
    @change="handleChangeUnit"
    clearable
    filterable
  >
    <el-option
      v-for="item in unitOption"
      :key="item.parm_id"
      :label="item.parm_name"
      :value="item.parm_id"
    >
    </el-option>
  </el-select>
  <!-- </div> -->
</template>

<script>
import * as SysParm from '@/api/SysParm'

export default {
  name: 'UnitSelect',
  components: {},
  props: {
    parm_id: {
      type: String,
      default: () => {
        return null
      }
    },
    parm_name: {
      type: String,
      default: () => {
        return null
      }
    }
  },
  data() {
    return {
      parm: null, // 当前单位选中值
      unitOption: [] // 单位选项值
    }
  },
  computed: {},
  watch: {
    parm_id: {
      immediate: true,
      handler: function() {
        if (
          this.parm_id != null &&
          this.parm_id != '' &&
          this.parm_id != undefined
        ) {
          this.parm = this.parm_id
          // console.log(this.parm)
          // return
        }
      }
    },
    parm_name: {
      immediate: true,
      handler: function() {
        if (
          this.parm_name != null &&
          this.parm_name != '' &&
          this.parm_name != undefined
        ) {
          // this.initUnit(this.parm_name)
          let obj = {}
          obj = this.unitOption.find(item => {
            return item.parm_name === this.parm_name
          })
          this.parm = obj.parm_id
          // console.log(obj)
          // console.log(this.parm)
          // return
        }
      }
    }
  },
  mounted() {
    // this.parm = this.parm_id
    // if (
    //   this.parm_id != null &&
    //   this.parm_id != '' &&
    //   this.parm_id != undefined
    // ) {
    //   this.parm = this.parm_id
    //   // console.log(this.parm)
    // } else if (
    //   this.parm_name != null &&
    //   this.parm_name != '' &&
    //   this.parm_name != undefined
    // ) {
    //   // this.initUnit(this.parm_name)
    //   console.log(this.parm_name)
    //   console.log(this.unitOption)
    //   this.unitOption.forEach(item => {
    //     if (item.parm_name === this.parm_name) {
    //       this.parm = item.parm_id
    //     }
    //   })
    //   console.log(this.parm)
    // } else {
    //   this.parm = ''
    // }
  },
  created() {
    this.getUnitOption()
    // this.initUnit()
  },
  methods: {
    initUnit() {
      if (
        this.parm_id != null &&
        this.parm_id != '' &&
        this.parm_id != undefined
      ) {
        this.parm = this.parm_id
        console.log('parm_id!=null', this.parm)
      } else if (
        this.parm_name != null &&
        this.parm_name != '' &&
        this.parm_name != undefined
      ) {
        // this.initUnit(this.parm_name)
        console.log('parm_name', this.parm_name)
        console.log(this.unitOption)
        this.unitOption.forEach(item => {
          if (item.parm_name === this.parm_name) {
            this.parm = item.parm_id
          }
        })
        console.log('parm_name!=null', this.parm)
      } else {
        this.parm = ''
        console.log('no parm', this.parm)
      }
    },
    // 获取单位选项
    async getUnitOption() {
      let listQuery = {
        // 查询条件
        page: 1,
        rows: 999999,
        key: '',
        sort: 'asc',
        order: 'create_time'
      }
      let ParamTypeName = '单位'
      await SysParm.GetParamByParamType(listQuery, ParamTypeName).then(
        response => {
          this.unitOption = response.Result.Data // 动态获取列表数据
          // console.log(this.unitOption)
        }
      )
      // await this.initUnit()
    },
    handleChangeUnit(event) {
      if (event) {
        let obj = {}
        obj = this.unitOption.find(item => {
          return item.parm_id === event
        })
        this.$emit('handleChangeUnit', {
          parm_id: this.parm,
          parm_name: obj.parm_name
        })
      } else {
        this.$emit('handleChangeUnit', '')
      }
    }
  }
}
</script>

<style lang="scss" scoped>
// @import "@/styles/index.scss"
</style>
