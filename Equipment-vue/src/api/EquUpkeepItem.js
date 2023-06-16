import request from '@/utils/request'

// 添加设备点检项
export function AddEquUpkeepItem(data){
    return request({
        url:'/EquUpkeepItem/CreateEquUpkeepItem',
        method:'POST',
        data:data
    })
}

// 分页点检项分页查询
export function GetEquUpkeepItemPage(params){
    return request({
        url:'/EquUpkeepItem/GetEquUpkeepItemPage',
        method:'GET',
        params:params
    })
}

// 设备点检项模糊查询接口
export function QueryEquUpkeepItem(data){
    return request({
        url:'/EquUpkeepItem/QueryAndPage',
        method:'POST',
        data:data
    })
}

// 根据设备id获取设备点检项目接口
export function GetEquUpkeepItemByEquId(data){
    return request({
        url:'/EquUpkeepItem/GetByEquId',
        method:'POST',
        data:data
    })
}
// 设备点检项删除接口
export function DeleteByIds(ids){
    return request({
        url:'/EquUpkeepItem/DeleteEquUpkeepItemByIds',
        method:'DELETE',
        data:ids
    })
}

// 更新接口
export function UpdateUpkeepItem(id,data){
    return request({
        url:'/EquUpkeepItem/UpdateEquUpkeepItem',
        method:'PUT',
        params:{id:id},
        data:data
    })
}

export function GetWorkPlaceTree(){
    return request({
        url:'/WorkPlace/GetAll',
        method:'GET'
    })
}

export function GetAll(){
    return request({
        url:'/EquUpkeepItem/GetAll',
        method:'GET'
    })
}