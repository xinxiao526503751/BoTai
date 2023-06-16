<template>
  <div class="flex-column">
    <div class="filter-container">
      <el-form :inline="true" :model="listQuery" class="demo-form-inline">
        <el-form-item label="编码：">
          <el-input
            v-model="listQuery.Q1"
            size="small"
            placeholder="请输入保养计划编码"
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
          <el-button
            type="success"
            icon="el-icon-plus"
            size="medium"
            round
            @click="onBinding"
            >绑定设备和保养项</el-button
          >
        </el-form-item>
      </el-form>
    </div>
    <div class="app-container flex-item">
      <div class="bg-white" style="height: 100%">
        <el-table
          ref="mainTable"
          height="calc(100% - 100px)"
          style="width: 100%"
          :key="tableKey"
          :data="list"
          :header-cell-style="headClass"
          @row-click="rowClick"
          @selection-change="handleSelectionChange"
          v-loading="listLoading"
          border
          fit
          stripe
          highlight-current-row
        >
          <el-table-column type="selection" align="center" width="55">
          </el-table-column>
          <el-table-column
            align="center"
            :label="'等级'"
            width="50px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.PLAN_LEVEL }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'计划编码'"
            fixed="left"
            min-width="150px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.PLAN_ID }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'设备名称'"
            min-width="100px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.EQU_NAME }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'设备保养项'"
            min-width="120px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.UPKEEP_ITEM_NAME }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'保养人员'"
            min-width="80px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.UPKEEP_PERSON }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'保养方式'"
            min-width="80px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.UPKEEP_METHOD }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'保养频率'"
            min-width="80px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.UPKEEP_FREQUENCY }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'是否手动'"
            min-width="80px"
            :formatter="setManual"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.IS_MANUAL }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'是否停机'"
            min-width="80px"
            :formatter="setStopMachine"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.IS_STOP }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'提前期'"
            min-width="100px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.LEAD_TIME }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'偏置期单位'"
            min-width="100px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.LEAD_TIME_UNIT }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'生效时间'"
            min-width="120px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.START_TIME }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'失效时间'"
            min-width="120px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.END_TIME }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'描述'"
            min-width="100px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.DES }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'是否保养'"
            min-width="80px"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.IS_UPKEEP }}</span>
            </template>
          </el-table-column>
          <!-- <el-table-column
            align="center"
            :label="'创建时间'"
            width="100"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.create_time }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'创建人'"
            width="100"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.create_by_name }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'修改时间'"
            width="100"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.modified_time }}</span>
            </template>
          </el-table-column>
          <el-table-column
            align="center"
            :label="'修改人'"
            width="100"
            show-overflow-tooltip
          >
            <template slot-scope="scope">
              <span>{{ scope.row.modified_by_name }}</span>
            </template>
          </el-table-column> -->
          <!-- 操作栏 -->
          <el-table-column align="center" width="200px" :label="'操作'">
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
          </el-table-column>
        </el-table>
        <pagination
          v-show="true"
          :total="total"
          :page.sync="listQuery.pageIndex"
          :rows.sync="listQuery.pageSize"
          @pagination="handleCurrentChange"
        />
        <div class="filter-container1" style="margin-bottom: 20px">
          <el-button
            size="medium "
            type="warning"
            icon="el-icon-circle-plus"
            @click="UpkeepBegin"
            >开始保养</el-button
          >
        </div>
      </div>
    </div>
    <!-- 弹出点检计划对话框 -->
    <el-dialog
      class="dialog-mini"
      :title="textMap[dialogStatus]"
      :visible.sync="dialogFormVisible"
      width="800px"
      :append-to-body="true"
      v-el-drag-dialog
      :close-on-click-modal="false"
    >
      <!-- 添加时间设置对话框 -->
      <el-dialog
        class="dialog-mini"
        width="800px"
        :title="'新增时间'"
        :visible.sync="dialogTimeFormVisible"
        :append-to-body="true"
        :before-close="handleCloseTime"
        v-el-drag-dialog
      >
        <el-form
          :rules="timeRules"
          ref="timeTemp"
          :model="timeTemp"
          label-position="right"
          label-width="100px"
        >
          <!-- 选择不同的时间设置 -->
          <el-row>
            <el-col :span="12">
              <el-form-item size="small" :label="'类型：'" prop="unit_mode">
                <el-select
                  v-model="timeTemp.unit_mode"
                  placeholder="请选择"
                  style="width: 280px"
                  clearable
                >
                  <el-option
                    v-for="item in modeOptions"
                    :key="item.unit_mode"
                    :label="item.modeLabel"
                    :value="item.unit_mode"
                  >
                  </el-option>
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item
                size="small"
                :label="'开始时间：'"
                v-show="timeTemp.unit_mode == 0"
                prop="trigger_time"
              >
                <el-date-picker
                  v-model="timeTemp.trigger_time"
                  type="datetime"
                  value-format="yyyy-MM-dd HH:mm:ss"
                  placeholder="选择日期时间"
                  style="width: 280px"
                  clearable
                >
                </el-date-picker>
              </el-form-item>
              <el-form-item
                size="small"
                :label="'间隔日期：'"
                v-show="timeTemp.unit_mode == 2"
                prop="occur_condition"
              >
                <el-input
                  v-model="timeTemp.occur_condition"
                  style="width: 280px"
                ></el-input>
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item
            size="small"
            :label="'可选星期：'"
            v-show="timeTemp.unit_mode == 1"
            prop="week_condition"
          >
            <el-checkbox-group
              v-model="timeTemp.week_condition"
              @change="getWeek"
              size="mini"
              style="margin-top: 12px; margin-left: 0px"
            >
              <el-checkbox
                v-for="i in weekList"
                :label="i.day"
                :key="i.day"
                border
                >{{ i.name }}</el-checkbox
              >
            </el-checkbox-group>
          </el-form-item>
          <el-form-item
            size="small"
            :label="'可选每月日期：'"
            v-show="timeTemp.unit_mode == 3"
            prop="month_condition"
          >
            <el-checkbox-group
              v-model="timeTemp.month_condition"
              @change="getMonth"
              size="mini"
              style="margin-top: 12px; margin-left: 0px"
            >
              <el-checkbox v-for="i in 31" :label="i" :key="i" border>{{
                i
              }}</el-checkbox>
            </el-checkbox-group>
          </el-form-item>

          <el-form-item size="small" :label="'描述：'" prop="remark">
            <el-input
              type="textarea"
              :rows="2"
              v-model="timeTemp.remark"
            ></el-input>
          </el-form-item>
        </el-form>
        <span slot="footer">
          <el-button size="mini" @click="handleCloseTime">关闭</el-button>
          <el-button size="mini" type="primary" @click="createTimeSet"
            >保存</el-button
          >
        </span>
      </el-dialog>

      <!-- 保养计划表单 -->
      <el-form
        :rules="rules"
        ref="dataForm"
        :model="temp"
        label-position="right"
        label-width="100px"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item size="small" :label="'计划编码：'" prop="PLAN_ID">
              <el-input v-model="temp.PLAN_ID" :disabled="true"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item size="small" :label="'计划等级：'" prop="PLAN_LEVEL">
              <el-select
                v-model="temp.PLAN_LEVEL"
                placeholder="请选择(A等级最高)"
                style="width: 280px"
                clearable
              >
                <el-option
                  v-for="item in plan_level"
                  :key="item.level"
                  :label="item.level"
                  :value="item.level"
                >
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-tabs type="border-card">
          <el-tab-pane label="基础信息">
            <el-row>
              <el-col :span="12">
                <el-form-item
                  size="small"
                  :label="'生效时间：'"
                  prop="START_TIME"
                >
                  <el-date-picker
                    v-model="temp.START_TIME"
                    type="datetime"
                    value-format="yyyy-MM-dd HH:mm:ss"
                    placeholder="生效时间"
                    style="width: 264px"
                  >
                  </el-date-picker>
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item
                  size="small"
                  :label="'失效时间：'"
                  prop="END_TIME"
                >
                  <el-date-picker
                    v-model="temp.END_TIME"
                    type="datetime"
                    value-format="yyyy-MM-dd HH:mm:ss"
                    placeholder="失效时间"
                    style="width: 264px"
                  >
                  </el-date-picker>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="8">
                <el-form-item
                  size="small"
                  :label="'保养人员：'"
                  prop="UPKEEP_PERSON"
                >
                  <el-input v-model="temp.UPKEEP_PERSON"></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  size="small"
                  :label="'保养方式：'"
                  prop="UPKEEP_METHOD"
                >
                  <el-input v-model="temp.UPKEEP_METHOD"></el-input>
                </el-form-item>
              </el-col>
              <el-col :span="8">
                <el-form-item
                  size="small"
                  :label="'保养频率：'"
                  prop="UPKEEP_FREQUENCY"
                >
                  <!-- <el-input v-model="temp.CHECK_FREQUENCY"></el-input> -->
                  <el-select
                    v-model="temp.UPKEEP_FREQUENCY"
                    placeholder="请选择"
                    clearable
                  >
                    <el-option
                      v-for="item in freOptions"
                      :key="item.frelabel"
                      :label="item.frelabel"
                      :value="item.frelabel"
                    >
                    </el-option>
                  </el-select>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="12">
                <el-form-item
                  size="small"
                  :label="'是否手动：'"
                  prop="IS_MANUAL"
                >
                  <el-select
                    v-model="temp.IS_MANUAL"
                    placeholder="请选择"
                    style="width: 264px"
                    clearable
                  >
                    <el-option
                      v-for="item in manualOptions"
                      :key="item.manuallabel"
                      :label="item.manuallabel"
                      :value="item.manuallabel"
                    >
                    </el-option>
                  </el-select>
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item size="small" :label="'是否停机：'" prop="IS_STOP">
                  <el-select
                    v-model="temp.IS_STOP"
                    placeholder="请选择"
                    style="width: 264px"
                    clearable
                  >
                    <el-option
                      v-for="item in manualOptions"
                      :key="item.manuallabel"
                      :label="item.manuallabel"
                      :value="item.manuallabel"
                    >
                    </el-option>
                  </el-select>
                </el-form-item>
              </el-col>
            </el-row>
            <el-row>
              <el-col :span="12">
                <el-form-item size="small" :label="'提前期：'" prop="LEAD_TIME">
                  <el-input v-model="temp.LEAD_TIME" style="width: 140px">
                  </el-input>
                  <el-select
                    v-model="temp.LEAD_TIME_UNIT"
                    placeholder="请选择"
                    style="width: 120px"
                    clearable
                  >
                    <el-option
                      v-for="item in timeOptions"
                      :key="item.timelabel"
                      :label="item.timelabel"
                      :value="item.timelabel"
                    >
                    </el-option>
                  </el-select>
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item
                  size="small"
                  :label="'添加人员：'"
                  prop="CREATED_BY"
                >
                  <el-input type="input" v-model="temp.CREATED_BY"></el-input>
                </el-form-item>
              </el-col>
            </el-row>
            <el-form-item size="small" :label="'计划描述：'" prop="DES">
              <el-input type="textarea" :rows="2" v-model="temp.DES"></el-input>
            </el-form-item>
          </el-tab-pane>
          <el-tab-pane label="时间设置">
            时间设置列表
            <el-button
              type="primary"
              size="mini"
              icon="el-icon-plus"
              style="margin-left: 10px"
              @click="handleTimeSet"
              >添加</el-button
            >
            <el-table
              ref="timeTable"
              height="200px"
              style="width: 100%; margin-top: 10px"
              :data="timeList"
              border
              fit
              stripe
              highlight-current-row
            >
              <el-table-column
                align="center"
                :label="'触发类型'"
                show-overflow-tooltip
              >
                <template slot-scope="scope">
                  <span>{{ mapmode[scope.row.unit_mode] }}</span>
                </template>
              </el-table-column>
              <el-table-column
                align="center"
                :label="'触发条件'"
                show-overflow-tooltip
              >
                <template slot-scope="scope">
                  <span>{{ scope.row.occur_condition }}</span>
                </template>
              </el-table-column>
              <el-table-column
                align="center"
                :label="'开始时间'"
                show-overflow-tooltip
              >
                <template slot-scope="scope">
                  <span>{{ scope.row.trigger_time }}</span>
                </template>
              </el-table-column>
              <el-table-column
                align="center"
                :label="'备注'"
                show-overflow-tooltip
              >
                <template slot-scope="scope">
                  <span>{{ scope.row.remark }}</span>
                </template>
              </el-table-column>
              <el-table-column
                align="center"
                width="60px"
                :label="'操作'"
                class-name="small-padding fixed-width"
                fixed="right"
              >
                <template slot-scope="scope">
                  <el-button
                    size="mini"
                    icon="el-icon-delete"
                    @click="handleTimeDelete(scope.$index, timeList)"
                  ></el-button>
                </template>
              </el-table-column>
            </el-table>
          </el-tab-pane>
        </el-tabs>
      </el-form>
      <span slot="footer">
        <el-button
          size="mini"
          v-if="dialogStatus == 'create'"
          type="primary"
          @click="createData"
          >保存</el-button
        >
        <el-button size="mini" v-else type="primary" @click="updateData"
          >保存</el-button
        >
        <el-button size="mini" @click="dialogFormVisible = false"
          >关闭</el-button
        >
      </span>
    </el-dialog>
    <!-- 弹出设备和点检项对话框 -->
    <el-dialog
      width="1300px"
      class="dialog-mini"
      :modal="false"
      v-el-drag-dialog
      :title="'设备和保养项-绑定'"
      :close-on-click-modal="false"
      :visible.sync="dialogInnerVisible"
    >
      <el-form
        :inline="true"
        :model="formInline2"
        class="demo-form-inline"
        :rules="rules2"
      >
        <el-form-item label="设备编码：">
          <el-input
            v-model="formInline2.equCode"
            size="small"
            placeholder="请输入设备编码"
            disabled
          ></el-input>
        </el-form-item>
        <el-form-item label="设备名称：">
          <el-input
            v-model="formInline2.equName"
            size="small"
            placeholder="请输入设备名称"
            disabled
          ></el-input>
        </el-form-item>
        <el-form-item label="保养项编码：">
          <el-input
            v-model="formInline2.itemCode"
            size="small"
            placeholder="请输入保养项编码"
            disabled
          ></el-input>
        </el-form-item>
        <el-form-item label="保养项名称：">
          <el-input
            v-model="formInline2.itemName"
            size="small"
            placeholder="请输入保养项名称"
            disabled
          ></el-input>
        </el-form-item>
      </el-form>
      <el-row :gutter="4" class="fh">
        <el-col :span="4">
          <el-card shadow="never" class="body-small fh" style="overflow: auto">
            <div slot="header" class="clearfix">
              <el-button type="text" style="padding: 0 11px" @click="getAll"
                >全部设备>></el-button
              >
            </div>
            <el-tree
              :data="equTypesTree"
              :expand-on-click-node="false"
              :props="defaultProps"
              accordion
              @node-click="handleNodeClick"
            ></el-tree>
          </el-card>
        </el-col>
        <el-col :span="20" class="fh">
          <el-table
            ref="singleTable"
            :data="upkeepItemList"
            :header-cell-style="headClass"
            highlight-current-row
            border
            stripe
            fit
            style="width: 100%"
            height="400"
            @row-click="setCurrent"
            @selection-change="handleSelectionChange2"
          >
            <el-table-column
              align="center"
              type="index"
              width="50"
            ></el-table-column>
            <el-table-column
              align="center"
              :label="'保养项目类型编码'"
              width="auto"
              prop="UPKEEP_ITEM_ID"
              show-overflow-tooltip
            ></el-table-column>
            <el-table-column
              align="center"
              :label="'保养项目类型名称'"
              width="auto"
              prop="UPKEEP_ITEM_NAME"
              show-overflow-tooltip
            ></el-table-column>
          </el-table>
          <!-- 分页 -->
          <pagination
            v-show="true"
            :total="total2"
            :page.sync="listQuery2.pageIndex"
            :rows.sync="listQuery2.pageSize"
            @pagination="handleCurrentChange2"
          />
        </el-col>
      </el-row>
      <div slot="footer">
        <el-button size="mini" @click="dialogInnerVisible = false"
          >关闭</el-button
        >
        <el-button size="mini" type="primary" @click="save">保存</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
// 分页组件
import Pagination from '@/components/Pagination'
// 树型数据组件
import Treeselect from '@riophae/vue-treeselect'
import * as UpkeepPlan from '@/api/UpkeepPlan'
import * as EquInfo from '@/api/EquInfo'
import * as UpkeepItem from '@/api/UpkeepItem'
import elDragDialog from '@/directive/el-dragDialog'
import { listToTreeSelect } from '@/utils'
import { TwoListToTree } from '@/utils'
import { values } from 'shelljs/commands'
import validateSchema from 'webpack/lib/validateSchema'
import debounce from 'lodash/debounce'
export default {
  name: 'UpkeepPlan',
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
      // tree配置项
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      formInline2: {
        equCode: '',
        equName: '',
        itemCode: '',
        itemName: ''
      },
      // 主展示表格数据
      list: null,
      upkeepItemList:null,
      tableTotalData: [], //表格临时数据
      tableTotalData2: [],
      listLoading: true,
      timeList: [], // 时间设置列表绑定的数据
      itemList: null, // 点检项列表绑定的数据
      searchStatus: false, //模糊查询标志
      dialogInnerVisible: false, //设备和点检项弹出窗显示标志
      // 分页参数
      listQuery: {
        Q1:'',
        Q2:'',
        QId:'',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      listQuery2: {
        Q1:'',
        Q2:'',
        QId:'',
        pageIndex: 1,
        pageSize: 10,
        flag:0
      },
      tableKey: 0, // 是否增加一栏
      total: 0, //分页总数据
      total2: 0, //点检项总数据
      // 主弹窗数据
      temp: {
        PLAN_LEVEL: '',
        PLAN_ID: '',
        EQU_ID: '',
        EQU_NAME: '',
        UPKEEP_ITEM_ID: '',
        UPKEEP_ITEM_NAME: '',
        UPKEEP_PERSON: '',
        UPKEEP_METHOD: '',
        UPKEEP_FREQUENCY: '',
        IS_MANUAL: '',
        IS_STOP: '',
        LEAD_TIME: '',
        LEAD_TIME_UNIT: '',
        START_TIME: '',
        END_TIME: '',
        DES: '',
        IS_UPKEEP: '',
        CREATED_BY: '',
        CREATED_TIME: ''
      },
      // 计划等级
      plan_level: [
        {
          level: 'A'
        },
        {
          level: 'B'
        },
        {
          level: 'C'
        },
        {
          level: 'D'
        }
      ],
      // 时间数据
      timeTemp: {},
      // 格式选择
      modeOptions: [
        {
          unit_mode: 0,
          modeLabel: '指定时间'
        },
        {
          unit_mode: 1,
          modeLabel: '每周'
        },
        {
          unit_mode: 2,
          modeLabel: '间隔天数'
        },
        {
          unit_mode: 3,
          modeLabel: '每月'
        }
      ],
      // 表格验证规则
      rules: {
        PLAN_LEVEL: [
          {
            required: 'true',
            message: '计划等级不能为空',
            trigger: 'blur'
          }
        ],
        START_TIME: [
          {
            required: 'true',
            message: '生效时间不能为空',
            trigger: 'blur'
          }
        ],
        END_TIME: [
          {
            required: 'true',
            message: '失效时间不能为空',
            trigger: 'blur'
          }
        ],
        CREATED_BY: [
          {
            required: 'true',
            message: '添加人员不能为空',
            trigger: 'blur'
          }
        ],
        IS_MANUAL: [
          {
            required: 'true',
            message: '请进行选择',
            trigger: 'blur'
          }
        ],
        IS_STOP: [
          {
            required: 'true',
            message: '请进行选择',
            trigger: 'blur'
          }
        ]
      },
      rules2: {
        equCode: [
          {
            required: 'true',
            message: '设备编码不能为空',
            trigger: 'blur'
          }
        ],
        equName: [
          {
            required: 'true',
            message: '设备名称不能为空',
            trigger: 'blur'
          }
        ],
        itemCode: [
          {
            required: 'true',
            message: '保养项编码不能为空',
            trigger: 'blur'
          }
        ],
        itemName: [
          {
            required: 'true',
            message: '保养项名称不能为空',
            trigger: 'blur'
          }
        ]
      },
      // 时间格式验证规则
      timeRules: {},
      //弹窗状态
      dialogStatus: '',
      textMap: {
        create: '计划-新增',
        update: '计划-修改'
      },
      // 周显示列表
      weekList: [
        // 可选星期
        {
          day: 1,
          name: '星期一'
        },
        {
          day: 2,
          name: '星期二'
        },
        {
          day: 3,
          name: '星期三'
        },
        {
          day: 4,
          name: '星期四'
        },
        {
          day: 5,
          name: '星期五'
        },
        {
          day: 6,
          name: '星期六'
        },
        {
          day: 7,
          name: '星期日'
        }
      ],
      // 事件单位选择
      timeOptions: [
        {
          lead_time_unit: '0',
          timelabel: '分钟'
        },
        {
          lead_time_unit: '1',
          timelabel: '小时'
        },
        {
          lead_time_unit: '2',
          timelabel: '天'
        }
      ],
      manualOptions: [
        { is_manual: 1, manuallabel: '是' },
        { is_manual: 0, manuallabel: '否' }
      ],
      freOptions: [
        {
          frelabel: '一天一次'
        },
        {
          frelabel: '一天三次'
        },
        {
          frelabel: '两天一次'
        }
      ],
      //偏置期单位
      mapunit: {
        0: '分钟',
        1: '小时',
        2: '天'
      },
      nodeClickStatus: false, //节点点击标志
      dialogFormVisible: false, // 主弹窗显示标志
      dialogTimeFormVisible: false, // 时间列表设置弹出框
      multipleSelection: [], // 列表checkbox选中的值
      multipleSelection2: [], //点检项checkbox选择行
      showDescription: false, // 绑定table是否显示ID
      equTypesTree: null, //设备类型树
      nodeId: null, //属性数据节点Id
      equTypeId: '', //设备类型Id
      equTypeName: '', //设备类型名称
      checkItemId: '', //点检项Id
      checkItemName: '', //点检项名称
      tempList: '' //临时数据
    }
  },
  created() {
    this.getUpKeepPlanPageList()
    this.getUpkeepItem()
    this.getTree()
  },
  methods: {
    getWeek(value) {
      this.timeTemp.occur_condition = value.join(',')
    },
    getMonth(value) {
      this.timeTemp.occur_condition = value.join(',')
    },
    // 保养计划分页数据
    getUpKeepPlanPageList(){  
      this.listLoading = true
      UpkeepPlan.QueryUpkeepPlan(this.listQuery).then(response=>{
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
    // 模糊查询按钮
    onQuery() {
      if (this.listQuery.Q1 === '') {
        this.$message.warning('请输入查询条件')
        return
      } else {
        this.queryAndPage(this.listQuery)
      }
    },
    // 模糊查询接口函数
    queryAndPage(data) {
      this.listLoading=true
      UpkeepPlan.QueryUpkeepPlan(data).then(response=>{
        if(response.Code===200){
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
    //搜索框数据清除
    handleClear() {
      this.getUpKeepPlanPageList()
    },
    // 新增按钮
    onAdd() {
      // 弹出点检计划添加框
      this.timeList = []
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm'].clearValidate()
        this.resetTemp()
      })
    },
    // 新增接口函数
    addUpkeepPlan(addData) {
      UpkeepPlan.AddUpkeepPlan(addData).then((response) => {
        if (response.Code === 200) {
          this.dialogFormVisible = false
          this.$notify({
            title: '成功',
            message: '创建成功',
            type: 'success',
            duration: 2000
          })
        }
        this.getUpKeepPlanPageList()
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    //新增窗口保存函数
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          if (this.temp.LEAD_TIME === '') {
            this.temp.LEAD_TIME = 0
          }
          this.addUpkeepPlan(this.temp)
        }
      })
    },
    // 删除按钮
    onDelete() {
      var ids = []
      var array = this.multipleSelection
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
          this.deleteUpkeepPlanByIds(ids)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 批量删除接口函数
    deleteUpkeepPlanByIds(ids) {
      UpkeepPlan.DeleteByIds(ids).then((response) => {
        if (response.Code === 200) {
          this.$notify({
            title: '成功',
            message: '删除成功',
            type: 'success',
            duration: 2000
          })
        }
        this.getUpKeepPlanPageList()
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 绑定按钮
    onBinding() {
      if (this.multipleSelection.length === 0) {
        this.$notify({
          title: '提示信息',
          message: '请先选择一条点检计划记录',
          type: 'warning',
          position: 'bottom-right',
          duration: 2000
        })
        return
      } else if (this.multipleSelection.length > 1) {
        this.$notify({
          title: '提示信息',
          message: '请只选择一条点检计划记录',
          type: 'warning',
          position: 'bottom-right',
          duration: 2000
        })
        return
      }
      this.dialogInnerVisible = true
    },
    // 设备和点检项保存按钮
    save() {
      if (
        this.formInline2.equCode === '' ||
        this.formInline2.equName === '' ||
        this.formInline2.itemCode === '' ||
        this.formInline2.itemName === ''
      ) {
        this.$notify({
          title: '提示信息',
          message: '设备或点检项未绑定，请进行绑定',
          type: 'warning',
          duration: 2000
        })
        return
      }
      this.temp.EQU_ID = this.formInline2.equCode
      this.temp.EQU_NAME = this.formInline2.equName
      this.temp.UPKEEP_ITEM_ID = this.formInline2.itemCode
      this.temp.UPKEEP_ITEM_NAME = this.formInline2.itemName
      this.bindEquAndItem(this.temp.PLAN_ID, this.temp)
      // 绑定之后重置表单
      this.resetformInline2()
      // 进行再次请求表单数据
      this.getUpkeepItem()
    },
    //更新接口函数
    bindEquAndItem(id, data) {
      UpkeepPlan.UpdateUpkeepPlan(id, data)
        .then((response) => {
          if (response.Code === 200) {
            this.dialogInnerVisible = false
            this.$notify({
              title: '成功',
              message: '绑定成功',
              type: 'success',
              duration: 2000
            })
          } else {
            this.$notify({
              title: '失败',
              message: '绑定失败',
              type: 'warning',
              duration: 2000
            })
          }
          this.getUpKeepPlanPageList()
        })
        .catch(() => {
          this.$message({
            type: 'warning',
            message: '绑定失败，请点击数据行'
          })
        })
    },
    // 主表格行点击事件
    rowClick(row) {
      this.$refs.mainTable.clearSelection()
      this.$refs.mainTable.toggleRowSelection(row)
      this.temp = row
      //console.log('------行数据', this.temp)
    },
    // 属性数据节点点击事件
    handleNodeClick(data) {
      this.formInline2.equCode = data.id
      this.formInline2.equName = data.label
      this.nodeId = data.id
      this.listQuery2.QId=data.id
      if (data.children != null) {
          this.getUpkeepItemData(this.listQuery2)
      }
    },
    // 节点点击函数
    getUpkeepItemData(data){
      UpkeepItem.GetItemsWithTypeId(data).then(response=>{
        if(response.Code===200){
          this.nodeClickStatus=true
          this.upkeepItemList=response.Data
          this.total2 = response.Total
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    //主表格选框点击
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    // 点检项目表选框点击
    handleSelectionChange2(val) {
      this.multipleSelection2 = val
      if(this.multipleSelection2.length===0){
        return;
      };
      this.formInline2.itemCode = this.multipleSelection2[0].UPKEEP_ITEM_ID
      this.formInline2.itemName = this.multipleSelection2[0].UPKEEP_ITEM_NAME
    },
    // 设备和点检项弹窗数据行点击事件
    setCurrent(val) {
      this.$refs.singleTable.clearSelection()
      this.$refs.singleTable.toggleRowSelection(val)
      this.formInline2.itemCode = val.UPKEEP_ITEM_ID
      this.formInline2.itemName = val.UPKEEP_ITEM_NAME
    },
    // 获取所有
    getAll() {
      this.getUpkeepItem()
    },
    // 获取树
    async getTree() {
      var tempEquType
      var tempEquInfo
      await EquInfo.GetAllEquType().then((response) => {
        tempEquType = response.Data.map(function (item, index, input) {
          return {
            id: item.EQU_TYPE_ID,
            label: item.EQU_TYPE_NAME,
            parentId: null
          }
        })
      })
      await EquInfo.GetAllEquInfo().then((response) => {
        tempEquInfo = response.Data.map(function (item, index, input) {
          return {
            id: item.EQU_ID,
            label: item.EQU_NAME,
            parentId: item.TYPE_ID
          }
        })
      })
      var treeData = TwoListToTree(tempEquType, tempEquInfo)
      this.equTypesTree = treeData
      //console.log('--------树型数据',treeData)
    },
    // 点检项获取函数
    getUpkeepItem() {
      UpkeepItem.QueryUpkeepItem(this.listQuery2).then((response) => {
        if(response.Code===200){
          this.upkeepItemList=response.Data
          this.total2 = response.Total
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 弹出添加设备和点检项弹出框
    handleCreateEqptItem(row) {
      this.exam_plan_method_id = row.exam_plan_method_id
      this.dialogEqptItemFormVisible = true
      this.itemList = null
    },
    //更新按钮
    handleUpdate(row) {
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 更新弹窗保存按钮
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const id = this.temp.PLAN_ID
          //console.log('------更新数据',this.temp)
          this.updateUpkeepPlan(id, this.temp)
        }
      })
    },
    // 更新函数(使用的跟绑定函数是同一个)
    updateUpkeepPlan(id, data) {
      UpkeepPlan.UpdateUpkeepPlan(id, data).then((response) => {
        if (response.Code === 200) {
          this.dialogFormVisible = false
          this.$notify({
            title: '成功',
            message: '更新成功',
            type: 'success',
            duration: 2000
          })
        } else {
          this.$notify({
            title: '失败',
            message: '更新失败',
            type: 'warning',
            duration: 2000
          })
        }
        this.getUpKeepPlanPageList()
      })
    },
    //单行删除按钮
    handleDelete(row) {
      this.$confirm('此操作将永久删除该行数据，是否继续？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
        center: true
      })
        .then(() => {
          let id = row.PLAN_ID
          this.deleteUpkeepPlan(id)
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    // 单行删除接口函数
    deleteUpkeepPlan(id) {
      UpkeepPlan.DeleteUpkeepPlan(id).then((response) => {
        if (response.Code === 200) {
          this.$message({
            type: 'success',
            message: '删除成功'
          })
          if (this.total % this.listQuery.pageSize === 1) {
            this.listQuery.pageIndex = this.listQuery.pageIndex - 1
          }
          if (this.total === 1) {
            this.listQuery.pageIndex = 1
          }
          this.getUpKeepPlanPageList()
        }
      })
    },
    // 开始保养按钮
    UpkeepBegin() {
      //debugger;
      if (this.multipleSelection.length === 0) {
        this.$notify({
          title: '提示信息',
          message: '请先选择保养计划，再进行开始保养',
          type: 'warning',
          duration: 2000
        })
        return
      }
      const sentData = new Set()
      for (let i = 0; i < this.multipleSelection.length; i++) {
        const element = this.multipleSelection[i]

        if (element.EQU_NAME === null || element.UPKEEP_ITEM_NAME === null) {
          this.$notify({
            title: '提示信息',
            message: `${element.PLAN_ID}计划未绑定设备或保养项目，请先绑定再进行保养`,
            type: 'warning',
            duration: 2000
          })
          continue
        }
        // 更改数据
        element.IS_UPKEEP = '是'
        const id = element.PLAN_ID
        const flag = id + '-' + i.toString()
        if (sentData.has(flag)) { // 判断是否需要发送该元素
          continue; // 跳过当前元素的处理
        }
        sentData.add(flag)

        this.beginUpkeep(id, element);
        this.$bus.$emit('getData', {flag, data:element});
      }
    },
    // 开始点检更新函数
    beginUpkeep(id, data) {
      UpkeepPlan.UpdateUpkeepPlan(id, data).then((response) => {
        if(response.Code===200){
          this.getUpKeepPlanPageList()
        }
      }).catch(()=>{
        this.$message({
          message:'网络错误，请稍后重试',
          type:'error'
        })
      })
    },
    // 主列表重置
    resetTemp() {
      this.temp = {
        PLAN_LEVEL: '',
        EQU_ID: null,
        EQU_NAME: null,
        UPKEEP_ITEM_ID: null,
        UPKEEP_ITEM_NAME: null,
        UPKEEP_PERSON: '',
        UPKEEP_METHOD: '',
        UPKEEP_FREQUENCY: '',
        IS_MANUAL: '',
        IS_STOP: '',
        LEAD_TIME: '',
        LEAD_TIME_UNIT: '',
        START_TIME: '',
        END_TIME: '',
        DES: '',
        IS_UPKEEP: '',
        CREATED_BY: ''
      }
    },
    // 关闭新增时间弹出框
    handleCloseTime() {
      this.dialogTimeFormVisible = false
      this.dialogFormVisible = true
    },
    // 分页点击事件
    handleCurrentChange(val) {
      this.listQuery.pageIndex = val.page
      this.listQuery.pageSize = val.rows
      if(this.searchStatus){
        this.queryAndPage(this.listQuery)
        return
      }
      this.getUpKeepPlanPageList()
    },
    handleCurrentChange2(val) {
      this.listQuery2.pageIndex = val.page
      this.listQuery2.pageSize = val.rows
      if(this.nodeClickStatus){
        this.getUpkeepItemData(this.listQuery2)
        return
      }
      this.getUpkeepItem()
    },
    // 设置计划时间弹出框
    handleTimeSet() {
      this.resetTimeTemp()
      this.dialogFormVisible = false
      this.dialogTimeFormVisible = true
      this.$nextTick(() => {
        // 在下次dom更新时清除表单验证规则，但不清除表单字段
        this.$refs['timeTemp'].clearValidate()
      })
    },
    // 重置formInline2
    resetformInline2() {
      this.formInline2 = {
        equCode: '',
        equName: '',
        itemCode: '',
        itemName: ''
      }
    },
    // 重置时间列表
    resetTimeTemp() {},
    createTimeSet() {},
    setManual(row, column) {
      return row.is_manual === 1 ? '是' : '否'
    },
    setStopMachine(row, column) {
      return row.is_stop_machine === 1 ? '是' : '否'
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
.text {
  font-size: 16px;
}
.app-container .clearfix {
  position: relative;
}
.app-container .clearfix i {
  position: absolute;
  top: 8px;
  right: 10px;
  font-size: 20px;
  color: #00a0ff;
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
.el-card {
  border: none;
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
.filter-container1 {
  text-align: right;
  margin-right: 20px;
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