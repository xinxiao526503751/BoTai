import request from '@/utils/request'

// 获取点检计划分页数据接口
export function GetCheckPlanPage(params){
    return request({
        url:'/CheckPlan/GetPageList',
        method:'GET',
        params:params
    })
}
// 单行删除接口
export function DeleteCheckPlan(id){
    return request({
        url:'/CheckPlan/DeleteById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeleteByIds(ids){
    return request({
        url:'/CheckPlan/DeleteByIds',
        method:'DELETE',
        data:ids
    })
}
// 模糊查询接口
export function QueryCheckPlan(data){
    return request({
        url:'/CheckPlan/QueryAndPage',
        method:'POST',
        data:data
    })
}
// 新增接口
export function AddCheckPlan(data){
    return request({
        url:'/CheckPlan/CreateCheckPlan',
        method:'POST',
        data:data
    })
}

// 更新接口
export function UpdateCheckPlan(id,data){
    return request({
        url:'/CheckPlan/UpdateCheckPlan',
        method:'PUT',
        params:{id:id},
        data:data
    })
}