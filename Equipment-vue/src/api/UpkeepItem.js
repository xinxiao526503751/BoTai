import request from '@/utils/request'

//获取所有点检项目
export function GetAllUpkeepItem(){
    return request({
        url:'/UpkeepItem/GetAll',
        method:'GET'
    })
}

// 获取点检项目分页数据
export function GetUpkeepItemPage(data){
    return request({
        url:'/UpkeepItem/GetPageList',
        method:'GET',
        data:data
    })
}

// 根据type_id获取点检项目接口
export function GetItemsWithTypeId(data){
    return request({
        url:'/UpkeepItem/GetByTypeId',
        method:'POST',
        data:data
    })
}

//点检项可批量删除接口
export function DeleteByIds(ids){
    return request({
        url:'/UpkeepItem/DeleteByIds',
        method:'DELETE',
        data:ids
    })
}

//点检项新增接口
export function AddUpkeepItem(data){
    return request({
        url:'/UpkeepItem/CreateUpkeepItem',
        method:'POST',
        data:data
    })
}
// 点检项模糊查询接口
export function QueryUpkeepItem(data){
    return request({
        url:'/UpkeepItem/QueryAndPage',
        method:'POST',
        data:data
        }
    )
}