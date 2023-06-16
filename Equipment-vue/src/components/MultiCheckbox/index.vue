<!--
 * @Author: your name
 * @Date: 2021-08-24 15:19:39
 * @LastEditTime: 2021-08-25 16:15:07
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\components\MultiCheckbox\index.vue
-->
<template>
  <div class="multi-check" :style="{ width: width }">
    <div class="multi-check-item" v-if="isCheckAll">
      <el-checkbox
        label="全选"
        :indeterminate="isIndeterminate"
        v-model="checkAll"
        @change="handlerChange(0, null, $event)"
        >全选 {{ totalLabel }}</el-checkbox
      >
    </div>
    <div class="multi-check-item" v-for="item in dataList" :key="item.code">
      <el-checkbox
        v-model="item.isChecked"
        :indeterminate="item.isIndeterminate"
        :label="item.code"
        :value="item.value"
        @change="handlerChange(1, item, $event)"
        >{{ item.label }}</el-checkbox
      >
      <div class="multi-check-item" style="display:inline-block">
        <el-checkbox
          v-model="child.isChecked"
          @change="handlerChange(2, item, $event)"
          v-for="child in item.children || []"
          :key="child.code"
          :label="child.value"
          >{{ child.label }}</el-checkbox
        >
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: 'MultiCheckbox',
  props: {
    isCheckAll: {
      //是否显示全选
      type: Boolean,
      default: false
    },
    dataList: {
      //数据集合
      type: Array,
      required: true
    },
    width: {
      type: String,
      default: '100%'
    }
  },
  data() {
    return {
      checkAll: false,
      isIndeterminate: false,
      totalLabel: ''
    }
  },
  watch: {
    dataList: {
      handler() {
        const isChangeSum = this.dataList.reduce((prev, cur) => {
          return prev + +(cur.isChange || 0)
        }, 0) //计算改变的个数
        const isCheckSum = this.dataList.reduce((prev, cur) => {
          return prev + +(cur.isChecked || 0)
        }, 0) //统计选择的次数
        this.checkAll = isCheckSum && isCheckSum === this.dataList.length
        this.isIndeterminate = !!isChangeSum
        if (isCheckSum && isCheckSum === this.dataList.length) {
          this.isIndeterminate = false
        }
        this.totalLabel =
          isChangeSum === 0 ? '' : `已选择(${isChangeSum})个分类`
      },
      immediate: true,
      deep: true
    }
  },
  computed: {
    getDataList() {
      let parentList = []
      let childList = []
      this.dataList.forEach(item => {
        if (item.isChecked) parentList.push(item.value)
        ;(item.children || []).forEach(child => {
          if (child.isChecked) {
            childList.push(child.value)
          }
        })
      })
      return [parentList, childList]
    }
  },
  methods: {
    handlerCheckAll(isChecked) {
      this.dataList.forEach(item => {
        this.$set(item, 'isChecked', isChecked)
        this.$set(item, 'isChange', isChecked)
        if (isChecked) this.$set(item, 'isIndeterminate', false)
        item.children.forEach(child => {
          this.$set(child, 'isChecked', isChecked)
        })
      })
    },
    handlerChange($type, $row, $event) {
      let isChecked = !$event.target ? $event : $event.target.checked
      if ($type === 0) {
        //全选
        this.handlerCheckAll(isChecked)
        this.$emit('change', this.getDataList[0], this.getDataList[1])
        return
      }
      if ($type === 1) {
        $row.children.forEach(item => {
          this.$set(item, 'isChecked', isChecked)
        })
      }
      const checkCount = $row.children.reduce((prev, cur) => {
        let check = 0
        if (cur.isChecked === undefined) {
          check = 0
        } else {
          check = +cur.isChecked
        }
        return prev + +check
      }, 0) //统计选择的次数
      this.$set($row, 'isChange', checkCount === 0 ? false : true)
      this.$set(
        $row,
        'isChecked',
        checkCount && checkCount === $row.children.length ? true : false
      )
      this.$set(
        $row,
        'isIndeterminate',
        checkCount && checkCount < $row.children.length ? true : false
      )

      this.$emit('change', this.getDataList[0], this.getDataList[1])
    }
  }
}
</script>
<style lang="scss" scoped>
.multi-check {
  position: relative;
  text-align: left;

  .multi-check-item {
    width: 100%;
    background-color: #f9fafb;
    // padding-top: 10px;
    // padding-bottom: 10px;
    // padding-left: 25px;
    padding: 15px;
    border-radius: 6px;
  }
}
</style>
