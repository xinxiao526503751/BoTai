import request from '@/utils/request'
//获取刀具类型全部数据接口
export function GetAllProcessing(){
    return request({
        url:'/Processing/GetAllProcessing',
        method:'GET'
    })
}
//获取刀具类型数据分页查询接口
export function GetProcessingPageList(params){
    return request({
        url:'/Processing/GetProcessingPageList',
        method:'GET',
        params:params
    })
}

//刀具类型数据模糊查询接口
export function QueryProcessingList(params){
    return request({
        url:'/Processing/GetProcessingByString',
        method:'POST',
        params:params
    })
}
// 单个数据删除接口
export function DeleteProcessing(id){
    return request({
        url:'/Processing/DeleteProcessingById',
        method:'DELETE',
        params:{
            id:id
        }
    })
}
// 批量删除接口
export function DeleteProcessingByIds(ids){
    return request({
        url:'/Processing/DeleteProcessingsById',
        method:'DELETE',
        data:ids
    })
}
// 添加数据接口(params参数会默认前端传递的参数出现在请求地址上，data参数是将传递的参数以请求体的形式传递给后端)
export function AddProcessing(data){
    return request({
        url:'/Processing/AddProcessing',
        method:'POST',
        data:data
    })
}
// 更新数据接口
export function UpdateProcessing(id,params){
    return request({
        url:'/Processing/UpdateProcessingById',
        method:'PUT',
        params:{id:id},
        data:params
    })
}



