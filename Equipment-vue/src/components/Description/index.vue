<!--
 * @Author: your name
 * @Date: 2021-09-06 10:48:36
 * @LastEditTime: 2021-09-06 14:11:36
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\components\Description\index.vue
-->
<template>
  <div class="des">
    <el-descriptions
      class="margin-top"
      :column="column"
      :size="size"
      :colon="colon"
    >
      <el-descriptions-item label="设备名称:">{{
        temp.equipment_name
      }}</el-descriptions-item>
    </el-descriptions>
  </div>
</template>

<script>
export default {
  name: 'Description',
  props: {
    column: {
      type: Number,
      default: () => {
        return 0
      }
    },
    size: {
      type: String,
      default: () => {
        return 'default'
      }
    },
    colon: {
      type: Boolean,
      default: () => {
        return false
      }
    }
  }
}
</script>
