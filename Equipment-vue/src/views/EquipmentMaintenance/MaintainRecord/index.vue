<template>
  <div class="flex-column">
    <div class="filter-container">
      <el-form :inline="true" :model="listQuery" class="demo-form-inline">
        <el-form-item label="设备名称：">
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
          :data="list"
          :key="tableKey"
          highlight-current-row
          border
          stripe
          fit
          height="calc(100% - 55px)"
          style="width: 100%"
          :header-cell-style="headClass"
          @row-click="rowClick"
          @selection-change="handleRecChange"
        >
          <el-table-column align="center" type="index" width="50">
          </el-table-column>
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
            label="维修项编码"
            prop="MAINTAIN_ITEM_ID"
            min-width="80px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="维修项名称"
            prop="MAINTAIN_ITEM_NAME"
            min-width="80px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="维修人员"
            prop="MAINTAIN_PERSON"
            width="100px"
            show-overflow-tooltip
          ></el-table-column>
          <el-table-column
            align="center"
            label="维修结果"
            prop="MAINTAIN_RESULT"
            width="140px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <i
                class="el-icon-success"
                v-if="scope.row.MAINTAIN_RESULT === '完成'"
                style="color: #67C23A;font-size: 16px;"
              ></i>
              <i class="el-icon-error" v-else style="color: red;font-size: 16px"></i>
              {{ scope.row.MAINTAIN_RESULT }}
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            label="是否停机"
            prop="IS_STOP"
            width="80px"
            show-overflow-tooltip
          >
          </el-table-column>
        </el-table>
        <pagination
          v-show="total > 0"
          :total="total"
          :page.sync="listQuery.pageIndex"
          :rows.sync="listQuery.pageSize"
          @pagination="handleCurrentChange"
        />
      </div>
      <!-- 点检结果，合格不合格div区域 -->
      <!-- <div class="flex-item">
        <div class="filter-container1" style="margin: 10px 0">
          点检结果
          <el-button
            size="mini"
            style="margin-left: 20px"
            @click="btnStart"
            type="success"
            disabled
            >合格</el-button
          >
          <el-button
            size="mini"
            style="margin-right: 250px"
            @click="btnStart"
            type="danger"
            disabled
            >不合格</el-button
          >
          <span
            >已完成&nbsp; {{ this.qualifieNumber }}/{{
              this.eqtExamItemTotal
            }}&nbsp;条</span
          >
          <span>&nbsp;合格：{{ this.qualifieNumber }}</span>
          <span>&nbsp;不合格：{{ this.unqualifieNumber }}</span>
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
              label="点检项编码"
              prop="CHECK_ITEM_ID"
              min-width="100px"
            ></el-table-column>
            <el-table-column
              align="center"
              label="点检项名称"
              prop="CHECK_ITEM_NAME"
              min-width="80px"
            ></el-table-column>
            <el-table-column
              align="center"
              label="点检结果"
              prop="CHECK_RESULT"
              min-width="80px"
            >
            <span v-if="show1"><el-button type="danger" icon="el-icon-close" circle size="mini"></el-button> 不合格</span>
            <span v-if="show2"> <el-button type="primary" icon="el-icon-check" circle size="mini"></el-button> 合格</span>
            </el-table-column>
          </el-table>
          <pagination
            slot="footer"
            v-show="eqtExamItemTotal > 0"
            :total="eqtExamItemTotal"
            :page.sync="listQueryEqtItem.pageIndex"
            :rows.sync="listQueryEqtItem.pageSize"
            @pagination="handleExamItemSizeChange"
          />
        </div>
      </div> -->
    </div>
  </div>
</template>
  <script>
import { listToTreeSelect } from '@/utils'
import * as EquMaintainItem from '@/api/EquMaintainItem'
import elDragDialog from '@/directive/el-dragDialog'
import Pagination from '@/components/Pagination'

export default {
  name: 'MaintainRecord',
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
      list: null, //表格绑定的数据
      equipmentList: null, //设备表格绑定的数据
      examItemList: null, //点检项目表格绑定的数据
      epmList: null, // 点检计划表格绑定的数据
      total: 0, //总数据条数
      equipmentTotal: 0, //设备总条数
      eqtExamItemTotal: 0,
      qualifieNumber: 0,
      unqualifieNumber: 0,
      listLoading: true,
      listQuery: {
        Q1: '',
        Q2: '',
        QId: '',
        pageIndex: 1,
        pageSize: 10,
        flag: 0 //是否过滤的标志，0：不过滤，其它：过滤
      },
      formInline: {
        Q1: ''
      },
      listQueryEquipment: {
        // 查询条件
        page: 1,
        rows: 50000,
        key: ''
      },
      listQueryEqtItem: {
        pageIndex: 1,
        pageSize: 10
      },
      recId: '', //传递给后端获取该点检单下的具体点检项目
      res: '2',
      location_id: '', //传递给后端获取地点树下设备
      locations: [], // 用户可访问到的地点列表
      locationsTree: [], // 用户可访问到的地点所有组成的树
      OnlyRootEquip: false,
      showDescription: false,
      equId: '', //设备id
      temp: {
        //添加表单数据
        equipment_id: undefined,
        equipment_code: '',
        equipment_name: '',
        is_stop_machine: '', //是否停机
        status: '', //状态
        result: '', //结果
        location_parentid: null,
        location_name: '',
        location_id: '',
        exam_method_type: '1'
      },
      show1: false,
      show2: false,
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
      downloadLoading: false,
      lastClickRow: null
    }
  },
  //生命周期函数
  created() {
    this.getMaintainList()
  },
  mounted() {},
  methods: {
    //点检计划记录单行处理
    rowClick(row) {
      this.$refs.mainTable.setCurrentRow(row)
      //this.recId = row.exam_plan_rec_id
      this.qualifieNumber = 0
      this.unqualifieNumber = 0
      this.equId = row.EQU_ID
      if (this.equId === null) {
        this.$message({
          type: 'info',
          message: '无点检项存在'
        })
        return
      }
      this.getEquCheckItem(this.equId)
    },
    handleRecChange(val) {
      this.examRecSelection = val
    },
    // 获取维修项数据列表
    getMaintainList() {
      this.listLoading = true
      this.listQuery.flag = 0
      EquMaintainItem.QueryEquMaintainItem(this.listQuery)
        .then((response) => {
          if (response.Code === 200) {
            this.list = response.Data
            this.total = response.Total
            this.listLoading = false
          }
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    //监听页码值改变的事件
    handleCurrentChange(val) {
      this.listQuery.pageIndex = val.page
      this.listQuery.pageSize = val.rows
      if (this.searchStatus) {
        this.queryData(this.listQuery)
        return
      }
      this.getMaintainList()
    },
    //模糊查询
    onQuery() {
      if (this.listQuery.Q1 === '') {
        this.$message.warning('请输入查询条件')
        return
      } else {
        this.queryData(this.listQuery)
      }
    },
    queryData(data) {
      this.listLoading = true
      this.listQuery.flag = 0
      EquMaintainItem.QueryEquMaintainItem(data)
        .then((response) => {
          if (response.Code === 200) {
            this.list = response.Data
            this.total = response.Total
            this.listLoading = false
          }
        })
        .catch(() => {
          this.$message({
            message: '网络错误，请稍后重试',
            type: 'error'
          })
        })
    },
    // 输入框清除事件
    handleClear() {
      this.getMaintainList()
    },
    // 改变为合格
    handleStart(row) {
      ExamPlanRecItem.qualifyAll(this.recId).then(() => {
        this.getExamPlanRecList()
        this.getExamPlanRecItemList()
        this.$notify({
          title: '成功',
          message: '操作成功',
          position: 'bottom-right',
          type: 'success',
          duration: 2000
        })
      })
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
  background-color: #fff;
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
  