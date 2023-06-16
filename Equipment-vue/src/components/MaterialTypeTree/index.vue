<!--
 * @Author: gcy
 * @Date: 2021-09-26 19:50:02
 * @LastEditTime: 2021-09-26 20:01:41
 * @LastEditors: Please set LastEditors
 * @Description: 物料类型树
 * @FilePath: \Mes_Vue\src\components\MaterialTypeTree\index.vue
-->
<template>
  <div class="tree">
    <div class="app-container flex-item">
      <TreeSelect
        ref="materialTypeTree"
        :isCheck="isCheck"
        :isShowExpand="isShowExpand"
        :isShowFilter="isShowFilter"
        :multiple="multiple"
        :treeProps="props"
        :treeList="materialTypeTree"
        :accordion="accordion"
        :expandNode="expandNode"
        @handleNodeClick="handleNodeClickMaterialType"
      ></TreeSelect>
    </div>
  </div>
</template>

<script>
import { listToTreeSelect } from '@/utils'
import TreeSelect from '@/components/TreeSelect'
import * as BaseMaterialType from '@/api/BaseMaterialType'

export default {
  name: 'MaterialTypeTree',
  components: {
    TreeSelect
  },
  props: {
    // 是否显示全选按钮
    isCheck: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否显示展开收缩按钮
    isShowExpand: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否需要关键字过滤
    isShowFilter: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否可多选，默认单选
    multiple: {
      type: Boolean,
      default() {
        return false
      }
    },
    // 自动收起
    accordion: {
      type: Boolean,
      default: () => {
        return false
      }
    },
    // 是否在点击节点的时候展开或者收缩节点， 默认值为 true，如果为 false，则只有点箭头图标的时候才会展开或者收缩节点。
    expandNode: {
      type: Boolean,
      default() {
        return true
      }
    }
  },
  data() {
    return {
      props: {
        // 配置项（必选）
        id: 'id',
        label: 'label',
        // pid: 'parentId',
        children: 'children'
        // disabled:true
      },
      materials: [], // 获取物料列表
      materialTypeTree: [] // 获取物料类型树
    }
  },
  mounted() {
    this.getMaterialTypeTree()
  },
  methods: {
    handleNodeClickMaterialType(data) {
      this.$emit('handleNodeClickMaterialType', data)
    },
    // 获取物料类型树
    getMaterialTypeTree() {
      // 获取物料类型树
      var _this = this // 记录vuecomponent
      BaseMaterialType.GetAll().then(response => {
        _this.materials = response.Result.map(function(item, index, input) {
          return {
            id: item.material_type_id,
            label: item.material_type_name,
            parentId: item.material_type_parentid || null
            // parentId: item.material_type_parentid
          }
        })
        var materialtypestmp = JSON.parse(JSON.stringify(_this.materials))
        _this.materialTypeTree = listToTreeSelect(materialtypestmp)
      })
    }
  }
}
</script>
