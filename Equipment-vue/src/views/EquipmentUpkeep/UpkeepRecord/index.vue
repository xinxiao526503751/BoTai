<template>
  <div class="flex-column">
    <div class="filter-container">
      <el-form :inline="true" :model="listQuery" class="demo-form-inline">
        <el-form-item label="计划编码：">
          <el-input
            v-model="listQuery.Q1"
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
        </el-form-item>
      </el-form>
    </div>
    <div class="app-container flex-item">
      <!--点检记录操作表区域  data指定数据源为数组-->
      <div class="flex-item fh1">
        <el-table
          ref="mainTable"
          :data="examRecList"
          :key="tableKey"
          highlight-current-row
          border
          stripe
          fit
          style="width: 100%"
          height="calc(100% - 55px)"
          :header-cell-style="headClass"
          @row-click="rowClick"
          @selection-change="handleRecChange"
        >
          <el-table-column align="center" type="index" width="50">
          </el-table-column>
          <el-table-column
            align="center"
            label="计划编码"
            prop="PLAN_ID"
            width="140px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="设备编码"
            prop="EQU_ID"
            min-width="80px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="设备名称"
            prop="EQU_NAME"
            min-width="80px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="保养项编号"
            prop="UPKEEP_ITEM_ID"
            min-width="80px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="保养项名称"
            prop="UPKEEP_ITEM_NAME"
            min-width="80px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="保养人员"
            prop="UPKEEP_PERSON"
            width="100px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="是否完成"
            prop="UPKEEP_RESULT"
            width="120px"
            show-overflow-tooltip
          >
          <template slot-scope="scope">
              <i
                class="el-icon-success"
                v-if="scope.row.UPKEEP_RESULT === '完成'"
                style="color: #67C23A;font-size: 16px;"
              ></i>
              <i class="el-icon-error" v-else style="color: red;font-size: 16px"></i>
              {{ scope.row.UPKEEP_RESULT }}
            </template>
          </el-table-column>
        </el-table>
        <pagination
          v-show="examPlanTotal > 0"
          :total="examPlanTotal"
          :page.sync="listQuery.pageIndex"
          :rows.sync="listQuery.pageSize"
          @pagination="handleCurrentChange"
        />
      </div>
      <!-- 点检结果，合格不合格div区域 -->
      <!-- <div class="flex-item">
        <div class="filter-container1" style="margin: 10px 0">
          保养结果
          <el-button
            size="mini"
            style="margin-left: 20px"
            @click="btnStart"
            type="success"
            disabled
            >完成</el-button
          >
          <el-button
            size="mini"
            style="margin-right: 250px"
            @click="btnStart"
            type="danger"
            disabled
            >未完成</el-button
          >
          <span
            >已完成&nbsp; {{ this.qualifieNumber }}/{{
              this.eqtExamItemTotal
            }}&nbsp;条</span
          >
          <span>&nbsp;完成：{{ this.qualifieNumber }}</span>
          <span>&nbsp;未完成：{{ this.unqualifieNumber }}</span>
        </div>
        <div class="flex-item fh1">
          <el-table
            ref="eqtExamItemTable"
            :data="eqtExamItemList"
            :key="tableKey"
            highlight-current-row
            border
            stripe
            fit
            :header-cell-style="headClass"
            style="width: 100%"
            height="200"
            @row-click="rowClickeqtExamItem"
            @selection-change="handleRecItemChange"
          >
            <el-table-column
              align="center"
              type="selection"
              width="55"
            ></el-table-column>
            <el-table-column
              align="center"
              label="保养项编码"
              prop="UPKEEP_ITEM_ID"
              min-width="100px"
            ></el-table-column>
            <el-table-column
              align="center"
              label="保养项名称"
              prop="UPKEEP_ITEM_NAME"
              min-width="80px"
            ></el-table-column>
            <el-table-column
              align="center"
              label="保养结果"
              prop="UPKEEP_RESULT"
              min-width="80px"
            >
            <span v-show="show1"><el-button type="danger" icon="el-icon-close" circle size="mini"></el-button> 未完成</span>
            <span v-show="show2"> <el-button type="primary" icon="el-icon-check" circle size="mini"></el-button> 完成</span>
            </el-table-column>
          </el-table>
        </div>
      </div> -->
    </div>
    <!-- <div class="filter-container1" style="margin-bottom:20px">
        <el-button
          size="medium "
          type="warning"
          icon="el-icon-circle-plus"
          >开始点检</el-button
        >
        <el-button
          size="medium "
          style="background-color:#67C23A; color: #fff" 
          icon="el-icon-success"
          >点检完成</el-button
        >
      </div> -->
    <!-- 选择设备的弹出框 -->
    <!-- <el-dialog
      width="900px"
      v-el-drag-dialog
      :modal="false"
      class="dialog-mini rule-form-dialog"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogEqptVisible"
      :close-on-click-modal="false"
    >
      <div style="height: 500px; overflow: auto">
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
                    @click="getAllEquipment"
                    >全部设备>></el-button
                  >
                </div>
                <el-tree
                  :data="locationsTree"
                  :expand-on-click-node="false"
                  default-expand-all
                  :props="defaultProps"
                  @node-click="handleNodeClick"
                  style="
                    width: 100%;
                    overflow-y: scroll;
                    overflow-x: hidden;
                    height: 420px;
                  "
                ></el-tree>
              </el-card>
            </el-col>
            <el-col :span="19" class="fh">
              <div class="bg-white fh">
                <el-row class="flex-item fh">
                  <el-table
                    ref="equipmentTable"
                    :data="equipmentList"
                    :key="equipmentTableKey"
                    style="width: 100%；overflow-x:hidden;"
                    height="475px"
                    @row-click="equipmentRowClick"
                    @selection-change="handleEqptSingleChange"
                    highlight-current-row
                    border
                    stripe
                    fit
                  >
                    <el-table-column type="index" width="55"> </el-table-column>
                    <el-table-column
                      align="center"
                      :label="'设备编号'"
                      prop="equipment_code"
                      min-width="120px"
                    ></el-table-column>
                    <el-table-column
                      align="center"
                      :label="'设备名称'"
                      prop="equipment_name"
                      min-width="120px"
                    ></el-table-column>
                  </el-table>
                </el-row>
              </div>
            </el-col>
          </el-row>
        </div>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button size="mini" @click="dialogEqptVisible = false"
          >取消</el-button
        >
        <el-button
          size="mini"
          v-if="dialogStatus == 'choose'"
          type="primary"
          @click="createEqptData"
          >确认</el-button
        >
      </div>
    </el-dialog> -->
    <!-- 新建点检任务弹出框 -->
    <!-- <el-dialog
      width="900px"
      v-el-drag-dialog
      :modal="false"
      class="dialog-mini rule-form-dialog"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogItemVisible"
    >
      <div style="height: 500px; overflow: auto">
        <div class="app-container flex-item">
          <el-row class="fh">
            <el-col :span="7" class="fh ls-border">
              <el-table
                ref="epmTable"
                :data="epmList"
                highlight-current-row
                border
                stripe
                fit
                style="width: 100%; overflow-x: hidden"
                height="470px"
                @row-click="epmRowClick"
                @selection-change="epmhandleSelection"
              >
                <el-table-column
                  align="center"
                  type="selection"
                  width="50"
                ></el-table-column>
                <el-table-column
                  align="center"
                  :label="'计划编码'"
                  prop="exam_plan_method_code"
                  min-width="90px"
                  show-overflow-tooltip
                ></el-table-column>
                <el-table-column
                  align="center"
                  :label="'计划名称'"
                  prop="exam_plan_method_name"
                  min-width="90px"
                  show-overflow-tooltip
                ></el-table-column>
              </el-table>
            </el-col>
            <el-col :span="17" class="fh">
              <el-row class="fh">
                <el-table
                  ref="examItemTable"
                  :data="examItemList"
                  :key="examItemTableKey"
                  highlight-current-row
                  border
                  stripe
                  fit
                  style="width: 100%"
                  height="475px"
                  @row-click="epmiRowClick"
                  @selection-change="examItemhandleSelChange"
                >
                  <el-table-column
                    align="center"
                    type="selection"
                    width="50"
                  ></el-table-column>
                  <el-table-column
                    align="center"
                    :label="'点检项编码'"
                    prop="examitem_code"
                    show-overflow-tooltip
                    min-width="90px"
                  ></el-table-column>
                  <el-table-column
                    align="center"
                    :label="'点检项名称'"
                    prop="examitem_name"
                    show-overflow-tooltip
                    min-width="90px"
                  ></el-table-column>
                  <el-table-column
                    align="center"
                    :label="'维保标准'"
                    prop="examitem_std"
                    show-overflow-tooltip
                    min-width="90px"
                  ></el-table-column>
                  <el-table-column
                    align="center"
                    :label="'值类型'"
                    prop="value_type"
                    show-overflow-tooltip
                    min-width="90px"
                  ></el-table-column>
                  <el-table-column
                    align="center"
                    :label="'创建时间'"
                    width="100px"
                    show-overflow-tooltip
                    prop="create_time"
                  ></el-table-column>
                </el-table>
              </el-row>
            </el-col>
          </el-row>
        </div>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button size="mini" @click="dialogItemVisible = false"
          >取消</el-button
        >
        <el-button
          size="mini"
          v-if="dialogStatus == 'createItem'"
          type="primary"
          @click="createData"
          >确认</el-button
        >
      </div>
    </el-dialog> -->
  </div>
</template>
  <script>
import { listToTreeSelect } from '@/utils'
import * as UpkeepPlan from '@/api/UpkeepPlan'
import * as EquUpkeepItem from '@/api/EquUpkeepItem'
import elDragDialog from '@/directive/el-dragDialog'
import Pagination from '@/components/Pagination'

export default {
  name: 'UpkeepRecord',
  components: {
    Pagination
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
      singleleSelection: [], //设备单选值
      examRecSelection: [],
      examRecItemSelection: [], // 列表checkbox选中的值
      multipleSelection2: [], // 列表checkbox选中的值
      multipleSelection3: [], // 列表checkbox选中的值
      tableKey: 0, //点检操作table
      equipmentTableKey: 0, //设备table
      examItemTableKey: 0, //点检项目table
      eqtExamItemList: null,
      examRecList: null, //表格绑定的数据
      equipmentList: null, //设备表格绑定的数据
      examItemList: null, //点检项目表格绑定的数据
      epmList: null, // 点检计划表格绑定的数据
      examPlanTotal: 0, //总数据条数
      equipmentTotal: 0, //设备总条数
      eqtExamItemTotal: 0,
      qualifieNumber: 0,
      unqualifieNumber: 0,
      listLoading: true,
      listQuery: {
        Q1:'',
        Q2:'',
        QId:'',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      recId: '', //传递给后端获取该点检单下的具体点检项目
      res: '2',
      location_id: '', //传递给后端获取地点树下设备
      locations: [], // 用户可访问到的地点列表
      locationsTree: [], // 用户可访问到的地点所有组成的树
      OnlyRootEquip: false,
      showDescription: false,
      equId: '', //设备id
      show1:false,
      show2:false,
      tempId: {
        equipment_id: undefined
      },
      dialogEqptVisible: false, //设备控制对话框的显示与隐藏
      dialogItemVisible: false, //控制点检计划对话框的显示与隐藏
      chkRoot: false, // 根节点是否选中
      treeDisabled: true, // 树选择框时候可用
      dialogStatus: '',
      textMap: {
        choose: '选择点检的设备',
        createItem: '新增点检任务',
        copy: '点检操作-复制',
        update: '点检操作-修改'
      },
      searchStatus: false,
      tableTotalData: [],
      //添加表单验证规则对象
      rules: {
        equipment_code: [
          {
            required: true,
            message: '点检操作编码不能为空',
            trigger: 'blur'
          }
        ],
        equipment_name: [
          {
            required: true,
            message: '点检操作名称不能为空',
            trigger: 'blur'
          }
        ]
      },
      checkItemRowData: '', //点检项目行数据
      downloadLoading: false,
      lastClickRow: null,
    }
  },
  //生命周期函数
  created() {
    this.getUpkeepRecordList()
  },
  mounted() {
  },
  methods: {
    //点检计划记录单行处理
    rowClick(row) {
      this.$refs.mainTable.setCurrentRow(row)
      this.qualifieNumber = 0
      this.unqualifieNumber = 0
    },
    handleRecChange(val) {
      this.examRecSelection = val
    },
    // 获取计划列表
    getUpkeepRecordList() {
      this.listLoading = true
      EquUpkeepItem.QueryEquUpkeepItem(this.listQuery).then((response) => {
        if(response.Code===200){
          this.examRecList = response.Data
          this.examPlanTotal = response.Total
          this.listLoading = false
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    //监听页码值改变的事件
    handleCurrentChange(val) {
      this.listQuery.pageIndex = val.page
      this.listQuery.pageSize = val.rows
      if(this.searchStatus){
        this.queryData(this.listQuery)
      }
      this.getUpkeepRecordList()
    },
    //模糊查询
    onQuery() {
      if (this.listQuery.Q1 === '') {
        this.$message.warning('请输入搜索条件')
        return
      }
      this.queryData(this.listQuery)
    },
    // 查询函数
    queryData(data) {
      this.listLoading=true
      EquUpkeepItem.QueryEquUpkeepItem(data).then((response) => {
        if (response.Code === 200) {
          this.examRecList = response.Data
          this.examPlanTotal = response.Total
          this.listLoading = false
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 输入框清除事件
    handleClear() {
      this.getUpkeepRecordList()
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
  padding: 0 10px;
  margin-bottom: 5px;
  font-size: 14px;
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
.filter-container1 {
  padding: 0 10px;
}
.filter {
  margin-bottom: 10px;
}
.fh1 {
  height: 100% !important;
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
</style>
  