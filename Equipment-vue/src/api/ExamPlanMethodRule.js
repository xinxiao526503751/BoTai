/*
 * @Author: your name
 * @Date: 2021-07-28 16:49:25
 * @LastEditTime: 2021-08-10 09:57:51
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\api\ExamPlanMethodRule.js
 */
import request from '@/utils/request'

// 添加点检计划-时间
export function GetRuleByExamPlanId(exam_plan_id) {
  return request({
    url: '/ExamPlanMethodRule/GetExamPlanMethodRuleByExamPlanId',
    method: 'post',
    params: {
      exam_plan_id: exam_plan_id
    }
  })
}

// 根据点检计划Id和分页数据获取点检计划下挂载的时间规则
export function Update(data) {
  return request({
    url: '/ExamPlanMethodRule/Update',
    method: 'post',
    data
  })
}

// 删除点检计划-时间
export function Delete(data) {
  return request({
    url: '/ExamPlanMethodRule/Delete',
    method: 'post',
    data
  })
}
