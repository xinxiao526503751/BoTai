import request from '@/utils/request'

// 获取点检计划分页数据接口
export function GetUpkeepPlanPage(params){
    return request({
        url:'/UpkeepPlan/GetPageList',
        method:'GET',
        params:params
    })
}
// 单行删除接口
export function DeleteUpkeepPlan(id){
    return request({
        url:'/UpkeepPlan/DeleteById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeleteByIds(ids){
    return request({
        url:'/UpkeepPlan/DeleteByIds',
        method:'DELETE',
        data:ids
    })
}
// 模糊查询接口
export function QueryUpkeepPlan(data){
    return request({
        url:'/UpkeepPlan/QueryAndPage',
        method:'POST',
        data:data
    })
}
// 新增接口
export function AddUpkeepPlan(data){
    return request({
        url:'/UpkeepPlan/CreateUpkeepPlan',
        method:'POST',
        data:data
    })
}

// 更新接口
export function UpdateUpkeepPlan(id,data){
    return request({
        url:'/UpkeepPlan/UpdateUpkeepPlan',
        method:'PUT',
        params:{id:id},
        data:data
    })
}