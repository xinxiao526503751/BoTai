<template>
  <div class="flex-column">
    <div class="filter-container">
      <el-form :inline="true" :model="listQuery" class="demo-form-inline">
        <el-form-item label="维修项名称：">
          <el-input
            v-model="listQuery.Q2"
            size="small"
            placeholder="请输入搜索字段"
            :clearable="true"
            @clear="handleClear"
          ></el-input>
        </el-form-item>
        <el-form-item>
          <el-button
            type="primary"
            icon="el-icon-search"
            size="medium"
            round
            @click="onQuery"
            >查询</el-button
          >
          <el-button
            type="success"
            icon="el-icon-circle-plus-outline"
            size="medium"
            round
            @click="onAdd"
            >新增</el-button
          >
          <el-button
            type="danger"
            icon="el-icon-delete"
            size="medium"
            round
            @click="onDelete"
            >删除</el-button
          >
        </el-form-item>
      </el-form>
    </div>
    <div class="app-container flex-item flex-column">
      <el-row :gutter="4" class="fh">
        <el-col :span="4" class="fh ls-border">
          <el-card shadow="never" class="body-small fh" style="overflow: auto">
            <div slot="header" class="clearfix">
              <el-button type="text" style="padding: 0 11px" @click="getAll"
                >全部维修项>></el-button
              >
            </div>
            <el-tree
              :data="equipmentTree"
              :expand-on-click-node="false"
              :props="defaultProps"
              @node-click="handleNodeClick"
            ></el-tree>
          </el-card>
        </el-col>
        <el-col :span="20" class="fh">
          <div class="flex-column">
            <!-- 主table区域 地点下的设备 -->
            <div class="demo-card table fh1">
              <el-table
                class="maintable"
                :header-cell-style="headClass"
                ref="mainTable"
                :key="tableKey"
                :data="list"
                style="width: 100%"
                height="calc(100% - 55px)"
                @row-click="rowClick"
                @selection-change="handleSelectionChange"
                v-loading="listLoading"
                border
                fit
                stripe
                highlight-current-row
              >
                <el-table-column align="center" type="selection" width="45">
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="120px"
                  :label="'维修项编码'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.MAINTAIN_ITEM_ID }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="80px"
                  :label="'维修项名称'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.MAINTAIN_ITEM_NAME }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  min-width="120px"
                  :label="'描述'"
                  show-overflow-tooltip
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.DES }}</span>
                  </template>
                </el-table-column>

                <!-- 操作栏 -->
                <!-- <el-table-column align="center" width="200px" :label="'操作'">
                  <template slot-scope="scope">
                    <el-button
                      size="mini"
                      round
                      type="primary"
                      icon="el-icon-edit"
                      @click="handleUpdate(scope.row)"
                      >更新</el-button
                    >
                    <el-button
                      size="mini"
                      round
                      type="danger"
                      icon="el-icon-delete"
                      @click="handleDelete(scope.row)"
                      >删除</el-button
                    >
                  </template>
                </el-table-column> -->
              </el-table>
              <!-- 分页 -->
              <pagination
                v-show="total > 0"
                :total="total"
                :page.sync="listQuery.pageIndex"
                :rows.sync="listQuery.pageSize"
                @pagination="handleCurrentChange"
              />
            </div>
          </div>
        </el-col>
      </el-row>
    </div>

    <!-- 弹出框 -->
    <el-dialog
      width="500px"
      v-el-drag-dialog
      :modal="false"
      :close-on-click-modal="false"
      class="dialog-mini rule-form-dialog"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogFormVisible"
    >
      <el-form :rules="rules" ref="dataForm" :model="temp" label-width="110px">
        <el-form-item
          :label="'维修项类型：'"
          size="small"
          prop="TYPE_ID"
        >
          <el-input v-model="temp.TYPE_ID" :disabled="true">
            <el-button
              slot="append"
              icon="el-icon-more"
              @click="btnTypeList"
            ></el-button>
          </el-input>
        </el-form-item>
        <el-form-item label="维修项名称：" prop="MAINTAIN_ITEM_NAME" size="small">
          <el-input v-model="temp.MAINTAIN_ITEM_NAME"></el-input>
        </el-form-item>
        <el-form-item label="值类型：" size="small" prop="value_type">
          <el-select
            style="width: 350px"
            v-model="temp.value_type"
            placeholder="请选择"
            @change="handleChange($event, id)"
          >
            <el-option
              v-for="item in value_typeGroup"
              :key="item.id"
              :label="item.value_type_name"
              :value="item.id"
            >
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item
          label="关联参数："
          size="small"
          prop="associated"
          v-if="valueStatus == 'displays'"
        >
          <el-select
            style="width: 350px"
            v-model="temp.associated"
            placeholder="请选择"
          >
            <el-option
              v-for="item in associatedGroup"
              :key="item.id"
              :label="item.associated_name"
              :value="item.id"
            >
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item :label="'说明：'" prop="DES">
          <el-input
            type="textarea"
            :autosize="{ minRows: 2, maxRows: 4 }"
            v-model="temp.DES"
          >
          </el-input>
        </el-form-item>
        <el-form-item label="添加员工：" prop="CREATED_BY" size="small">
          <el-input v-model="temp.CREATED_BY"></el-input>
        </el-form-item>
        <!-- <el-form-item :label="'标准：'">
          <el-input
            type="textarea"
            :autosize="{ minRows: 2, maxRows: 4 }"
            v-model="temp.examitem_std"
          >
          </el-input>
        </el-form-item> -->
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button
          size="mini"
          v-if="dialogStatus == 'create'"
          type="primary"
          @click="createData"
          >保存</el-button
        >
        <el-button
          size="mini"
          v-else-if="dialogStatus == 'update'"
          type="primary"
          @click="updateData"
          >保存</el-button
        >
        <el-button size="mini" @click="dialogFormVisible = false"
          >关闭</el-button
        >
      </div>
    </el-dialog>
    <!-- 点检项类型弹出框 -->
    <el-dialog
      width="700px"
      class="dialog-mini"
      :modal="false"
      v-el-drag-dialog
      :title="'维修项类型-选择'"
      :close-on-click-modal="false"
      :visible.sync="dialogInnerVisible"
    >
      <div style="overflow: auto">
        <el-table
          ref="singleTable"
          :data="equTypeList"
          :header-cell-style="headClass"
          highlight-current-row
          border
          stripe
          height="200"
          fit
          style="width: 100%"
          @row-click="setCurrent"
        >
          <el-table-column
            align="center"
            type="index"
            width="50"
          ></el-table-column>
          <el-table-column
            align="center"
            :label="'维修项目类型编码'"
            width="auto"
            prop="EQU_TYPE_ID"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            :label="'维修项目类型名称'"
            width="auto"
            prop="EQU_TYPE_NAME"
            show-overflow-tooltip
          ></el-table-column>
        </el-table>
        <!-- 分页 -->
        <pagination
          v-show="total2 > 0"
          :total="total2"
          :page.sync="listQuery2.pageIndex"
          :rows.sync="listQuery2.pageSize"
          @pagination="handleCurrentChange2"
        />
      </div>
      <div slot="footer">
        <el-button size="mini" type="primary" @click="chooseType"
          >保存</el-button
        >
        <el-button size="mini" @click="dialogInnerVisible = false"
          >关闭</el-button
        >
      </div>
    </el-dialog>
  </div>
</template>

<script>
// 分页组件
import Pagination from '@/components/Pagination'
// 树型数据组件
import Treeselect from '@riophae/vue-treeselect'
import * as EquInfo from '@/api/EquInfo'
import * as MaintainItem from '@/api/MaintainItem'
import elDragDialog from '@/directive/el-dragDialog'
import { listToTreeSelect } from '@/utils'
import { TwoListToTree } from '@/utils'

export default {
  name: 'MaintainItems',
  //两个子组件
  components: {
    Pagination,
    Treeselect
  },
  directives: {
    elDragDialog
  },
  data() {
    return {
      value_typeGroup: [
        {
          id: '1',
          value_type_name: '输入型(数字)'
        },
        {
          id: '2',
          value_type_name: '输入型(文字)'
        },
        {
          id: '3',
          value_type_name: '是否型'
        },
        {
          id: '4',
          value_type_name: '选择型'
        }
      ],
      associatedGroup: [
        {
          id: '1',
          associated_name: '条码格式'
        },
        {
          id: '2',
          associated_name: '设备定义参数'
        }
      ],
      // tree配置项
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      textMap: {
        create: '维修-新增',
        update: '维修-修改'
      },
      // 弹窗展示flag
      dialogFormVisible: false,
      // 内部弹窗显示状态值
      dialogInnerVisible: false,
      // 弹窗属性参数
      dialogStatus: '',
      // 搜索框参数
      formInline: {
        Q2: ''
      },
      // 主展示表格数据
      list: null,
      listLoading: true, //列表加载
      // 设备类型选择弹窗数据
      equTypeList: null,
      // 分页参数
      listQuery: {
        Q1:'',
        Q2:'',
        QId:'',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      listQuery2:{
        pageIndex: 1,
        pageSize: 10
      },
      // 弹窗参数汇总
      temp: {
        MAINTAIN_ITEM_NAME: '',
        TYPE_ID: '',
        DES: '',
        CREATED_BY: ''
      },
      // 表单规则验证
      rules: {
        MAINTAIN_ITEM_NAME: [
          {
            required: true,
            message: '维修项名称不能为空',
            trigger: 'blur'
          }
        ],
        TYPE_ID: [
          {
            required: true,
            message: '维修项类型不能为空',
            trigger: 'blur'
          }
        ],
        CREATED_BY: [
          {
            required: true,
            message: '添加员工不能为空',
            trigger: 'blur'
          }
        ]
      },
      tableTotalData: [], //临时数据
      equipmentTree: null, //设备树
      searchStatus: false, //搜索标志
      nodeClickStatus: false, //节点点击标志
      tableKey: 0, //主设备
      total: 0, //分页数据总数
      total2:0,//设备类型分页
      locationOptions: '', //车间地点选择
      eqTypeOptions: '', //设备类型选择
      valueStatus: '', // 值类型的状态
      equTypes: [], //设备类型数据存放
      checkItems: [], //设备点检项数据存放
      tempCheckItems: [], //临时设备点检项数据
      tempEquTypes: [], //临时设备类型数据
      tempTreeData: null, //树型临时数据
      nodeId: null, //节点数据的TYPE_ID
      multipleSelection: [], //主设备列表选择的值
      typeId: null //类型ID
    }
  },
  mounted() {},
  created() {
    this.getEquTypePage()
    this.getMaintainItemsPageList()
    this.getTree()
  },
  methods: {
    // 获取设备类型分页数据
    getEquTypePage(){
      EquInfo.GetEquTypePage(this.listQuery2).then((response)=>{
        if(response.Code===200){
          this.equTypeList=response.Data
          this.total2=response.Total
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    async getTree() {
      this.listLoading = true
      var tempEquType
      var tempMaintainItem
      await EquInfo.GetAllEquType().then((response) => {
        tempEquType = response.Data.map(function (item, index, input) {
          return {
            id: item.EQU_TYPE_ID,
            label: item.EQU_TYPE_NAME,
            parentId: null
          }
        })
      })
      await MaintainItem.GetAll().then((response) => {
        tempMaintainItem = response.Data.map(function (item, index, input) {
          return {
            id: item.MAINTAIN_ITEM_ID,
            label: item.MAINTAIN_ITEM_NAME,
            parentId: item.TYPE_ID
          }
        })
      })

      var treeData = TwoListToTree(tempEquType, tempMaintainItem)
      this.equipmentTree = treeData
      this.listLoading = false
      //console.log('------树型数据',treeData)
    },
    //获取维修项目分页数据
    getMaintainItemsPageList(){
      this.listLoading = true
      MaintainItem.QueryItems(this.listQuery).then(response=>{
        if(response.Code===200){
          this.list = response.Data
          this.total = response.Total
          this.listLoading = false
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 新型按钮
    onAdd() {
      this.valueStatus = 'displays'
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm'].clearValidate()
        this.resetTemp()
      })
    },
    // 新增弹窗保存按钮
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.addMaintainItem(this.temp)
        }
      })
    },
    //新增接口函数
    addMaintainItem(addData) {
      MaintainItem.AddItem(addData).then((response) => {
        if (response.Code === 200) {
          this.dialogFormVisible = false
          this.$notify({
            title: '成功',
            message: '创建成功',
            type: 'success',
            duration: 2000
          })
        }
        this.getMaintainItemsPageList()
        this.getTree()
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 模糊查询按钮
    onQuery() {
      if (this.listQuery.Q2 === '') {
        this.$message.warning('请输入查询条件')
        return
      } else {
        this.queryAndPage(this.listQuery)
      }
    },
    //模糊查询函数
    queryAndPage(data) {
      this.listLoading=true
      MaintainItem.QueryItems(data).then((response) => {
        if (response.Code === 200) {
          this.searchStatus=true
          this.list = response.Data
          this.total = response.Total
          this.listLoading = false
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 删除按钮
    onDelete() {
      var ids = []
      var array = this.multipleSelection
      for (let i = 0; i < array.length; i++) {
        ids.push(array[i].MAINTAIN_ITEM_ID)
      }
      this.$confirm(`是否确定删除选中的多条数据?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          this.deleteMaintainItemByIds(ids)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 可批量删除函数
    deleteMaintainItemByIds(ids) {
      MaintainItem.DeleteByIds(ids).then((response) => {
        if (response.Code === 200) {
          this.$notify({
            title: '成功',
            message: '删除成功',
            type: 'success',
            duration: 2000
          })
        }
        this.getMaintainItemsPageList()
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 树型数据上方的汉字点击事件
    getAll() {
      //this.getMaintainItemsPageList()
    },
    // 树型数据节点点击事件
    handleNodeClick(data) {
      this.nodeId = data.id
      this.listQuery.QId=data.id
      if (data.children != null) {
        this.nodeClickStatus=true
        this.getMaintainItemByEquId(this.listQuery)
      }
    },
    //节点点击函数
    getMaintainItemByEquId(data){
      this.listLoading=true
      MaintainItem.GetItemsWithTypeId(data).then(response=>{
        if(response.Code===200){
          this.list = response.Data
          this.total = response.Total
          this.listLoading = false
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 分页页码变化事件
    handleCurrentChange(val) {
      this.listQuery.pageIndex = val.page
      this.listQuery.pageSize = val.rows
      if(this.searchStatus){
        this.queryAndPage(this.listQuery)
        return
      }
      if(this.nodeClickStatus){
        this.getMaintainItemByEquId(this.listQuery)
        return
      }
      this.getMaintainItemsPageList()
    },
    handleCurrentChange2(val){
      this.listQuery2.pageIndex = val.page
      this.listQuery2.pageSize = val.rows
      this.getEquTypePage()
    },
    // 搜索输入框清除
    handleClear() {
      this.getMaintainItemsPageList()
    },
    // 主表格行点击事件
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
    },
    // 选中值类型时触发关联参数事件
    handleChange() {
      this.temp.value_type = val
      if (val == 1) {
        this.valueStatus == 'displays'
      } else this.valueStatus == ''
    },
    // 设备类型弹窗表格行点击事件
    setCurrent(row) {
      //console.log('-----行数据',row)
      this.typeId = row.EQU_TYPE_ID
      this.$refs.singleTable.clearSelection()
      this.$refs.singleTable.toggleRowSelection(row)
    },
    // 设备类型弹窗保存按钮
    chooseType() {
      this.temp.TYPE_ID = this.typeId
      this.dialogInnerVisible = false
    },
    // 选框改变事件
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    // 更新点击按钮
    handleUpdate() {
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 单个删除点击按钮
    handleDelete() {},
    // 更新弹窗保存按钮
    updateData() {},
    // 弹窗点检类型点击按钮
    btnTypeList() {
      this.dialogInnerVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 重置新增表单函数
    resetTemp(){
      this.temp={
        MAINTAIN_ITEM_NAME: '',
        TYPE_ID: '',
        DES: '',
        CREATED_BY: ''
      }
    },
    // 设置表头颜色
    headClass() {
      return 'background:#337ab7'
    }
  }
}
</script>

<style lang="scss" scoped>
.filter-container {
  padding: 0 10px;
  margin-bottom: 5px;
  font-size: 14px;
  // background-color: #fff;
}
.filter-container .el-form {
  height: 40px;
}
.filter-container .el-form .el-form-item .el-button[data-v-501bf07c] {
  padding-bottom: 7px;
}
.filter-container .el-row {
  padding: 10px 0;
}
.el-button--medium.is-round {
  padding: 7px 20px !important;
}
.app-container {
  padding: 0;
}
.text {
  font-size: 16px;
}
.item {
  margin-bottom: 18px;
}
.clearfix:before,
.clearfix:after {
  display: table;
  content: '';
}
.clearfix:after {
  clear: both;
}
.el-card__header {
  padding: 12px 20px;
}
.body-small.dialog-mini .el-dialog__body .el-form {
  padding-right: 0px;
  padding-top: 0px;
}
.fh1 {
  background-color: #fff;
  height: 100% !important;
}

.fh3 {
  height: 35px !important;
}
.row {
  display: flex;
  flex-wrap: wrap;
}
.el-row .el-card {
  min-width: 100%;
  height: 100% !important;
  margin-right: 20px;
  transition: all 0.5s;
}
.el-tree .el-tree-node__expand-icon.expanded {
  -webkit-transform: rotate(0deg);
  transform: rotate(0deg);
}
.el-tree .el-icon-caret-right:before {
  content: '\e723';
}
.el-tree .el-tree-node__expand-icon.expanded.el-icon-caret-right:before {
  content: '\e722';
}
</style>