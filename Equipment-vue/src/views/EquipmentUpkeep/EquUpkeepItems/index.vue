<template>
  <div class="flex-column">
    <div class="filter-container">
      <el-form :inline="true" :model="listQuery" class="demo-form-inline">
        <el-form-item label="设备名称：">
          <el-input
            v-model="listQuery.Q2"
            size="small"
            placeholder="请输入搜索条件"
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
        </el-form-item>
      </el-form>
    </div>
    <div class="app-container flex-item">
      <el-row :gutter="4" class="fh">
        <el-col :span="4" class="fh ls-border">
          <el-card shadow="never" class="body-small fh" style="overflow: auto">
            <div slot="header" class="clearfix">
              <el-button
                type="text"
                style="padding: 0 11px"
                @click="getAllEquipment"
                >全部设备>></el-button
              >
            </div>
            <el-tree
              :data="locationsTree"
              :expand-on-click-node="false"
              :props="defaultProps"
              @node-click="handleNodeClick"
            ></el-tree>
          </el-card>
        </el-col>
        <el-col :span="20" class="fh">
          <div class="bg-white fh">
            <el-row class="flex-item fh1">
              <!--地点下挂载的设备表区域  data指定数据源Equipmentlist为数组-->
              <el-table
                ref="singleTable"
                :data="list"
                :key="tableKey"
                style="width: 100%"
                height="calc(100% - 55px)"
                :header-cell-style="headClass"
                @row-click="rowClick"
                @selection-change="handleSingleSelectionChange"
                highlight-current-row
                border
                stripe
                fit
              >
                <el-table-column
                  align="center"
                  :label="'设备ID'"
                  prop="EQU_ID"
                  v-if="showDescription"
                  min-width="160px"
                >
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'设备编号'"
                  prop="EQU_ID"
                  min-width="120px"
                ></el-table-column>
                <el-table-column
                  align="center"
                  :label="'设备名称'"
                  prop="EQU_NAME"
                  min-width="120px"
                ></el-table-column>
              </el-table>
              <!-- 分页区域 -->
              <pagination
                v-show="total > 0"
                :total="total"
                :page.sync="listQuery.pageIndex"
                :rows.sync="listQuery.pageSize"
                @pagination="handleCurrentChange"
              />
            </el-row>
            <div class="app-container flex-item btns" slot="header" style="position:relative">
              <el-button
                size="mini"
                type="danger"
                style="margin-left: 20px;"
                round
                icon="el-icon-delete"
                @click="btnDel"
                >删除保养项</el-button
              >
              <el-button
                size="mini"
                type="success"
                style="position: absolute; right:15%"
                icon="el-icon-check"
                @click="qualify"
                >完成</el-button
              >
              <el-button
                size="mini"
                type="warning"
                style="position: absolute; right:5%"
                icon="el-icon-close"
                @click="noQualify"
                >未完成</el-button
              >
            </div>
            <!-- 设备挂载保养项目区域 -->
            <div class="flex-item fh2">
              <el-table
                ref="eqtExamItemTable"
                :data="eqtExamItemList"
                :key="eqtExamItemTableKey"
                highlight-current-row
                border
                stripe
                fit
                style="width: 100%"
                height="330"
                :header-cell-style="headClass"
                @row-click="rowClickEqtExamItemTable"
                @selection-change="eqtItemSelectionChange"
              >
                <el-table-column
                  align="center"
                  type="selection"
                  width="55"
                ></el-table-column>
                <el-table-column
                  align="center"
                  :label="'计划编号'"
                  prop="PLAN_ID"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.PLAN_ID }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'保养项编码'"
                  prop="UPKEEP_ITEM_ID"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.UPKEEP_ITEM_ID }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'保养项名称'"
                  prop="UPKEEP_ITEM_NAME"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.UPKEEP_ITEM_NAME }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'保养员工'"
                  prop="UPKEEP_PERSON"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.UPKEEP_PERSON }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  align="center"
                  :label="'保养结果'"
                  prop="UPKEEP_RESULT"
                  show-overflow-tooltip
                  min-width="80px"
                >
                  <template slot-scope="scope">
                    <span>{{ scope.row.UPKEEP_RESULT }}</span>
                  </template>
                </el-table-column>
                <!-- <el-table-column
                  align="center"
                  :label="'创建时间'"
                  min-width="100px"
                  show-overflow-tooltip
                  prop="create_time"
                ></el-table-column> -->
                <!-- <el-table-column
                  align="center"
                  :label="'操作'"
                  width="100px"
                  class-name="small-padding fixed-width"
                >
                  <template slot-scope="scope">
                    <el-button
                      size="mini"
                      icon="el-icon-edit"
                      @click="handleUpdate(scope.row)"
                    ></el-button>
                    <el-button
                      size="mini"
                      icon="el-icon-delete"
                      @click="handleDelete(scope.row)"
                    ></el-button>
                  </template>
                </el-table-column> -->
              </el-table>
              <pagination
                slot="footer"
                v-show="eqtExamItemTotal > 0"
                :total="eqtExamItemTotal"
                :page.sync="listQuery2.pageIndex"
                :rows.sync="listQuery2.pageSize"
                @pagination="handleCurrentChange2"
              />
            </div>
          </div>
        </el-col>
      </el-row>
    </div>
    <!-- 添加，复制，设备保养项的弹出框 -->
    <!-- <el-dialog
      width="1000px"
      v-el-drag-dialog
      :modal="false"
      :close-on-click-modal="false"
      class="dialog-mini rule-form-dialog"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogTableVisible"
    >
      <div style="height: 500px; overflow: auto">
        <div class="flex-column">

          <div class="app-container flex-item">
            <div class="app-container flex-item">
              <el-row :gutter="4" class="fh">
                <el-col :span="5" class="fh ls-border">
                  <el-card
                    shadow="never"
                    class="body-small fh"
                    style="overflow: auto"
                  >
                    <div slot="header" class="clearfix">
                      <el-button
                        type="text"
                        style="padding: 0 11px"
                        @click="getAllExamitem"
                        >全部保养项目>></el-button
                      >
                    </div>
                    <el-tree
                      :data="examItemsTree"
                      :expand-on-click-node="false"
                      default-expand-all
                      :props="defaultProps"
                      @node-click="handleNodeClickExamItem"
                      style="
                        width: 100%;
                        overflow-y: scroll;
                        overflow-x: hidden;
                        height: 370px;
                      "
                    ></el-tree>
                  </el-card>
                </el-col>
                <el-col :span="19" class="fh">
                  <div class="bg-white fh">
                    <el-table
                      ref="examItemTable"
                      :data="examItemList"
                      :key="examItemTableKey"
                      highlight-current-row
                      border
                      stripe
                      fit
                      style="width: 100%; overflow-x: hidden"
                      height="430px"
                      @row-click="rowClickExamItemTable"
                      @selection-change="ItemhandleSelectionChange"
                    >
                      <el-table-column
                        align="center"
                        type="selection"
                        width="50"
                      ></el-table-column>
                      <el-table-column
                        align="center"
                        :label="'保养项编码'"
                        prop="UPKEEP_ITEM_ID"
                        min-width="80px"
                        show-overflow-tooltip
                      ></el-table-column>
                      <el-table-column
                        align="center"
                        :label="'保养项名称'"
                        prop="UPKEEP_ITEM_NAME"
                        min-width="100px"
                        show-overflow-tooltip
                      ></el-table-column>
                      <el-table-column
                        align="center"
                        :label="'创建人'"
                        prop="CREATED_BY"
                        width="90px"
                        show-overflow-tooltip
                      ></el-table-column>
                    </el-table>
                  </div>
                </el-col>
              </el-row>
            </div>
          </div>
        </div>
      </div>

      <div slot="footer">
        <el-button size="mini" @click="dialogTableVisible = false"
          >关闭</el-button
        >
        <el-button
          size="mini"
          v-if="dialogStatus == 'create'"
          type="primary"
          @click="createData"
          >保存</el-button
        >
      </div>
    </el-dialog> -->
    <!-- 修改项的弹出框 -->
    <!-- <el-dialog
      width="500px"
      v-el-drag-dialog
      :modal="false"
      :close-on-click-modal="false"
      class="dialog-mini rule-form-dialog"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogFormVisible"
    >
      <el-form
        :rules="rules"
        ref="dataForm"
        :model="tempUpdate"
        label-width="110px"
      >
        <el-form-item label="默认值：" prop="default_value" size="small">
          <el-input v-model="tempUpdate.default_value"></el-input>
        </el-form-item>
        <el-form-item label="最大值：" prop="max_value" size="small">
          <el-input v-model="tempUpdate.max_value"></el-input>
        </el-form-item>
        <el-form-item label="最小值：" prop="min_value" size="small">
          <el-input v-model="tempUpdate.min_value"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer">
        <el-button size="mini" @click="dialogFormVisible = false"
          >关闭</el-button
        >
        <el-button
          size="mini"
          v-if="dialogStatus == 'update'"
          type="primary"
          @click="updateData"
          >保存</el-button
        >
      </div>
    </el-dialog> -->
  </div>
</template>
<script>
import { listToTreeSelect } from '@/utils'
import * as EquUpkeepItem from '@/api/EquUpkeepItem'
import * as EquInfo from '@/api/EquInfo'
import * as UpkeepItem from '@/api/UpkeepItem'
import Treeselect from '@riophae/vue-treeselect'
import '@riophae/vue-treeselect/dist/vue-treeselect.css'
import elDragDialog from '@/directive/el-dragDialog'
import Pagination from '@/components/Pagination'
import { forEach } from 'shelljs/commands'

export default {
  name: 'EquUpkeepItem',
  components: {
    Pagination,
    Treeselect
  },
  directives: {
    elDragDialog
  },
  data() {
    return {
      defaultProps: {
        // tree配置项
        children: 'children',
        label: 'label'
      },
      singleleSelection: [], // 设备单选值
      multipleSelection: [], // 列表checkbox多选中的值
      eqtItemMultiSelection: [],
      tableKey: 0, // 设备table
      eqtExamItemTableKey: 0, //设备下点检项目table
      examItemTableKey: 0, // 点检项目table
      list: null, // 设备表格绑定的数据
      examItemList: null, // 点检项目表格绑定的数据
      eqtExamItemList: null, // 设备对应的点检项目绑定的数据
      total: 0, // 总数据条数
      eqtExamItemTotal: 0,
      examItemTotal: 0, // 点检总条数
      listLoading: true,
      listQuery: {
        Q1:'',
        Q2:'',
        QId:'',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      searchStatus:false,
      listQuery2: {
        Q1:'',
        Q2:'',
        QId:'',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      result: [
        {
          level: '合格'
        },
        {
          level: '不合格'
        },
      ],
      planData: [],
      codeKey: '',
      nameKey: '',
      nameExamItemKey: '',
      equipmentId: '', //设备Id
      type_id: '', // 获取点检树下的列表
      location_id: null, // 传递给后端获取地点树下设备
      locations: [], // 用户可访问到的地点列表
      locationsTree: [], // 用户可访问到的地点所有组成的树
      examItems: [], // 用户可访问到的点检列表
      examItemsTree: [], // 用户可访问到的所有点检类型组成的树
      // OnlyRootEquip: false,
      showDescription: false,
      showExamItemDescription: false,
      temp: {
        // 添加表单数据传给后台
        equipment_id: undefined,
        examitem_ids: [],
        method_type: '1'
      },
      tempUpdate: {
        exam_equipment_item_id: undefined,
        equipment_id: undefined,
        equipment_code: '',
        equipment_name: '',
        default_value: '', // 默认值
        max_value: '', // 最大值
        min_value: '', // 最小值
        method_type: '1'
      },
      addData: {
        PLAN_ID: '',
        EQU_ID: '',
        EQU_NAME: '',
        UPKEEP_ITEM_ID: '',
        UPKEEP_ITEM_NAME: '',
        UPKEEP_PERSON: '',
        UPKEEP_RESULT: '',
        CREATED_BY: ''
      },
      dialogFormVisible: false, // 控制Table对话框的显示与隐藏
      dialogTableVisible: false,
      chkRoot: false, // 根节点是否选中
      treeDisabled: true, // 树选择框时候可用
      dialogStatus: '',
      textMap: {
        create: '选择保养项查询',
        update: '编辑保养项配置'
      },
      // 添加表单验证规则对象
      rules: {
        default_value: [
          {
            required: true,
            message: '默认值不能为空！',
            trigger: 'blur'
          }
        ],
        max_value: [
          {
            required: true,
            message: '最大值不能为空',
            trigger: 'blur'
          }
        ],
        min_value: [
          {
            required: true,
            message: '最小值不能为空',
            trigger: 'blur'
          }
        ]
      },
      downloadLoading: false,
      receovedIds:[],
      receivedIds:null,
      checkItemRowData:null,//点检项行数据
      checkIds:[],
    }
  },
  // 生命周期函数
  created() {
    this.getEquUpkeepItemPageList()
  },
  mounted() {
    // 钩子函数
    this.getEquType()
    this.getWorkPlaceTree()
    this.receiveData()
  },
  computed: {},
  methods: {
    // 获取从保养计划传入的数据
    receiveData(){
      this.receivedIds=new Set();
      //this.planData2=new Map();

      this.$bus.$on('getData',(payload)=>{
        // console.log('----payload',payload)
        // console.log('------========---------')
        const {flag,data}=payload;
        if(this.receivedIds.has(flag)){
          return;
        }

        this.receivedIds.add(flag);
        this.planData.push(data)
        //this.planData2.set(flag,data)
        //console.log('---------',this.planData2)
        this.planData.forEach(element=>{
          this.addEquUpkeepItem(element)
        });
        this.planData=[];
      })
    },
    // 获取数据保养项分页数据
    getEquUpkeepItemPageList(){
      this.listLoading=true
      this.listQuery.flag=1
      EquUpkeepItem.QueryEquUpkeepItem(this.listQuery).then(response=>{
        if(response.Code===200){
          this.list = response.Data
          this.total = response.Total
          this.listLoading=false
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 根据设备编号获取保养项目
    getUpkeepItemByEquId(data) {
      EquUpkeepItem.GetEquUpkeepItemByEquId(data).then((response) => {
        if(response.Code===200){
          this.eqtExamItemList = response.Data
          this.eqtExamItemTotal = response.Total
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 设备单行处理,当某一行被点击时会触发该事件
    rowClick(row) {
      this.$refs.singleTable.setCurrentRow(row)
      //this.equipmentId = row.EQU_ID
      this.listQuery2.QId=row.EQU_ID
      this.getUpkeepItemByEquId(this.listQuery2)
    },
    // 设备下保养项目行处理
    rowClickEqtExamItemTable(row) {
      this.checkItemRowData=row
      this.$refs.eqtExamItemTable.clearSelection()
      this.$refs.eqtExamItemTable.toggleRowSelection(row)
    },
    // 点击地点树的事件
    handleNodeClick(data) {
      var id = data.EQU_TYPE_ID
    },
    // 获取全部设备
    getAllEquipment() {
    },
    // 处理设备单选事件
    handleSingleSelectionChange(val) {
      this.singleleSelection = val
    },
    eqtItemSelectionChange(val) {
      this.eqtItemMultiSelection = val
    },
    // 合格按钮
    qualify(){
      if(this.eqtItemMultiSelection.length===0){
        this.$notify({
          title: ' 提示信息',
          message: '至少选择一条数据',
          position: 'bottom-right',
          type: 'warning',
          duration: 2000
        })
        return
      }
      this.eqtItemMultiSelection.forEach(element=>{
        const planId=element.PLAN_ID
        element.UPKEEP_RESULT='完成'

        this.updateResult(planId,element)
      })
    },
    // 更新函数
    updateResult(id,data){
      EquUpkeepItem.UpdateUpkeepItem(id,data).then((response)=>{
        if(response.Code===200){
          this.$notify({
            title: ' 提示信息',
            message: '修改成功',
            position: 'bottom-right',
            type: 'success',
            duration: 2000
          })
        }
        this.getUpkeepItemByEquId()
      })
    },
    // 不合格按钮
    noQualify(){
      if(this.eqtItemMultiSelection.length===0){
        this.$notify({
          title: ' 提示信息',
          message: '至少选择一条数据',
          position: 'bottom-right',
          type: 'warning',
          duration: 2000
        })
        return
      }
      this.eqtItemMultiSelection.forEach(element=>{
        const planId=element.PLAN_ID
        element.UPKEEP_RESULT='未完成'

        this.updateResult(planId,element)
      })
    },
    // 获取设备类型树
    getEquType() {
      EquInfo.GetAllEquType().then((response) => {
        var tempEquType = response.Data.map(function (item, index, input) {
          return {
            id: item.EQU_TYPE_ID,
            label: item.EQU_TYPE_NAME,
            parentId: null
          }
        })
        var equType = JSON.parse(JSON.stringify(tempEquType))
        this.examItemsTree = listToTreeSelect(equType)
      })
    },
    // 获取车间地点树
    getWorkPlaceTree(){
      EquUpkeepItem.GetWorkPlaceTree().then((response)=>{
        if(response.Code===200)
        {
          var tempTree=response.Data.map(function(item,index,input){
            return {
              id:item.PlaceId,
              label:item.PlaceName,
              parentId:item.ParentPlaceId||null
            }
          })
          var treeTemp=JSON.parse(JSON.stringify(tempTree))
          this.locationsTree=listToTreeSelect(treeTemp)
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 搜索框进行处理筛选
    onQuery() {
      if(this.listQuery.Q2===''){
        this.$message.warning('请输入搜索条件')
        return
      }
      this.listQuery.flag=1
      this.queryAndPage(this.listQuery)
    },
    // 模糊搜索分页函数
    queryAndPage(data){
      EquUpkeepItem.QueryEquUpkeepItem(data).then(response=>{
        if(response.Code===200){
          this.searchStatus=true
          this.list = response.Data
          this.total = response.Total
          this.listLoading=false
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 监听页码值改变的事件
    handleCurrentChange(val) {
      this.listQuery.pageIndex = val.page
      this.listQuery.pageSize = val.rows
      if(this.searchStatus){
        this.getUpkeepItemByEquId(this.listQuery)
        return
      }
      this.getEquUpkeepItemPageList()
    },
    // 搜索框清除控件
    handleClear(){
      this.getEquUpkeepItemPageList()
    },
    handleCurrentChange2(val){
      this.listQuery2.pageIndex = val.page
      this.listQuery2.pageSize = val.rows
      this.getUpkeepItemByEquId(this.listQuery2)
    },
    // 间接新增
    addEquUpkeepItem(data) {
      if (data === null) {
        return
      }
      this.addData.PLAN_ID = data.PLAN_ID
      this.addData.EQU_ID = data.EQU_ID
      this.addData.EQU_NAME = data.EQU_NAME
      this.addData.UPKEEP_ITEM_ID = data.UPKEEP_ITEM_ID
      this.addData.UPKEEP_ITEM_NAME = data.UPKEEP_ITEM_NAME
      this.addData.UPKEEP_PERSON = data.UPKEEP_PERSON
      this.addData.UPKEEP_RESULT = '未完成'
      this.addData.CREATED_BY = data.CREATED_BY
      //console.log('--------新增数据',this.addData)
      this.createEquUpkeepItem(this.addData)
    },
    // 新增函数
    createEquUpkeepItem(data) {
      EquUpkeepItem.AddEquUpkeepItem(data).then((response) => {
        if(response.Code===200){
          this.getEquUpkeepItemPageList()
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 删除
    btnDel() {
      if (this.eqtItemMultiSelection.length < 1) {
        this.$message({
          message: '至少删除一条数据',
          type: 'warning'
        })
        return
      }
      var ids = []
      var array = this.eqtItemMultiSelection
      for (let i = 0; i < array.length; i++) {
        ids.push(array[i].PLAN_ID)
      }
      this.$confirm(`是否确定删除选中的多条数据?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          this.deleteEquUpkeepItem(ids)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 删除点检项函数
    deleteEquUpkeepItem(ids) {
      EquUpkeepItem.DeleteByIds(ids).then((response) => {
        if (response.Code === 200) {
          this.$notify({
            title: '成功',
            message: '删除成功',
            type: 'success',
            duration: 2000
          })
        } 
        this.getEquUpkeepItemPageList()
        this.getUpkeepItemByEquId(this.listQuery2)
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    handleCreate() {
      this.dialogStatus = 'create'
      this.dialogTableVisible = true
    },
    // 设置表头颜色
    headClass() {
      return 'background:#337ab7'
    }
  }
}
</script>

<style scoped>
.filter-container {
  height: 50px;
  padding: 0 10px;
  font-size: 14px;
  /* background-color: #fff; */
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
  width: 100%;
  height: calc(100% - 50px);
  padding: 0;
}
.app-container .btns {
  height: 50px;
  padding: 10px 0;
}
.filter-container1 {
  margin-top: 10px;
  font-size: 14px;
}
.text {
  font-size: 14px;
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
/* 上下同，左右同 */
.el-card__header {
  padding: 12px 20px;
}
.body-small.dialog-mini .el-dialog__body .el-form {
  padding-right: 0px;
  padding-top: 0px;
}
.el-select.filter-item.el-select--small {
  width: 100%;
}

.fh1 {
  height: 50%;
}
.fh1 .table{
  height: 100%;
}
.fh2 {
  height: 40%;
}
</style>
