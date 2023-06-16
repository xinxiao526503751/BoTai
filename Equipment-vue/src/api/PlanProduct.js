import request from '@/utils/request'
//获取刀具类型全部数据接口
export function GetAllPlan(){
    return request({
        url:'/PlanScheduling/GetAllPlanScheduling',
        method:'GET'
    })
}
//获取刀具类型数据分页查询接口
export function GetPlanPageList(params){
    return request({
        url:'/PlanScheduling/GetPlanSchedulingPageList',
        method:'GET',
        params:params
    })
}

//刀具类型数据模糊查询接口
export function QueryPlanList(params){
    return request({
        url:'/PlanScheduling/GetPlanSchedulingByString',
        method:'POST',
        params:params
    })
}
// 单个数据删除接口
export function DeletePlan(id){
    return request({
        url:'/PlanScheduling/DeletePlanSchedulingById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeletePlanByIds(ids){
    return request({
        url:'/PlanScheduling/DeletePlanSchedulingsById',
        method:'DELETE',
        data:ids
    })
}
// 添加数据接口(params参数会默认前端传递的参数出现在请求地址上，data参数是将传递的参数以请求体的形式传递给后端)
export function AddPlan(data){
    return request({
        url:'/PlanScheduling/AddPlanScheduling',
        method:'POST',
        data:data
    })
}
// 更新数据接口
export function UpdatePlan(id,params){
    return request({
        url:'/PlanScheduling/UpdatePlanSchedulingById',
        method:'PUT',
        params:{id:id},
        data:params
    })
}



