import request from '@/utils/request'

// 添加设备点检项
export function AddEquCheckItem(data){
    return request({
        url:'/EquCheckItem/CreateEquCheckItem',
        method:'POST',
        data:data
    })
}

// 分页点检项分页查询
export function GetEquCheckItemPage(params){
    return request({
        url:'/EquCheckItem/GetEquCheckItemPage',
        method:'GET',
        params:params
    })
}

// 设备点检项模糊查询接口
export function QueryEquCheckItem(data){
    return request({
        url:'/EquCheckItem/QueryAndPage',
        method:'POST',
        data:data
    })
}

// 根据设备id获取设备点检项目接口
export function GetEquCheckItemByEquId(data){
    return request({
        url:'/EquCheckItem/GetByEquId',
        method:'POST',
        data:data
    })
}
// 设备点检项删除接口
export function DeleteByIds(ids){
    return request({
        url:'/EquCheckItem/DeleteEquCheckItemByIds',
        method:'DELETE',
        data:ids
    })
}

// 更新接口
export function UpdateCheckItem(id,data){
    return request({
        url:'/EquCheckItem/UpdateEquCheckItem',
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
        url:'/EquCheckItem/GetAll',
        method:'GET'
    })
}