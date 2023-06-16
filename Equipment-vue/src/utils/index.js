/*
 * @Author: your name
 * @Date: 2021-06-17 09:42:11
 * @LastEditTime: 2021-08-30 16:12:12
 * @LastEditors: Please set LastEditors
 * @Description: In User Settings Edit
 * @FilePath: \mes\src\utils\index.js
 */
/**
 * Created by jiachenpan on 16/11/18.
 */
/* eslint-disable camelcase */
/**
 * Parse the time to string
 * @param {(Object|string|number)} time
 * @param {string} cFormat
 * @returns {string | null}
 */
export function parseTime(time, cFormat) {
  if (arguments.length === 0 || !time) {
    return null
  }
  const format = cFormat || '{y}-{m}-{d} {h}:{i}:{s}'
  let date
  if (typeof time === 'object') {
    date = time
  } else {
    if (typeof time === 'string') {
      if (/^[0-9]+$/.test(time)) {
        // support "1548221490638"
        time = parseInt(time)
      } else {
        // support safari
        // https://stackoverflow.com/questions/4310953/invalid-date-in-safari
        time = time.replace(new RegExp(/-/gm), '/')
      }
    }

    if (typeof time === 'number' && time.toString().length === 10) {
      time = time * 1000
    }
    date = new Date(time)
  }
  const formatObj = {
    y: date.getFullYear(),
    m: date.getMonth() + 1,
    d: date.getDate(),
    h: date.getHours(),
    i: date.getMinutes(),
    s: date.getSeconds(),
    a: date.getDay()
  }
  const time_str = format.replace(/{([ymdhisa])+}/g, (result, key) => {
    const value = formatObj[key]
    // Note: getDay() returns 0 on Sunday
    if (key === 'a') {
      return ['日', '一', '二', '三', '四', '五', '六'][value]
    }
    return value.toString().padStart(2, '0')
  })
  return time_str
}

export function formatTime(format) {
  const now = new Date()
  var o = {
    'M+': now.getMonth() + 1, // month
    'd+': now.getDate(), // day
    'h+': now.getHours(), // hour
    'm+': now.getMinutes(), // minute
    's+': now.getSeconds(), // second
    'q+': Math.floor((now.getMonth() + 3) / 3), // quarter
    S: now.getMilliseconds() // millisecond
  }
  if (/(y+)/.test(format)) {
    format = format.replace(
      RegExp.$1,
      (now.getFullYear() + '').substr(4 - RegExp.$1.length)
    )
  }
  for (var k in o) {
    if (new RegExp('(' + k + ')').test(format)) {
      format = format.replace(
        RegExp.$1,
        RegExp.$1.length === 1 ? o[k] : ('00' + o[k]).substr(('' + o[k]).length)
      )
    }
  }
  return format
}

// 将list转成tree，使用前注意把array进行深拷贝
export function listToTreeSelect(array, parent, tree) {
  tree = typeof tree !== 'undefined' ? tree : []
  parent =
    typeof parent !== 'undefined'
      ? parent
      : {
        id: null
      }

  var children = array.filter((val, index) => {
    return val.parentId === parent.id
  })

  if (children.length > 0) {
    if (parent.id === null) {
      tree = children
    } else {
      parent['children'] = children
    }

    children.forEach((val, index) => {
      listToTreeSelect(array, val)
    })
  }

  return tree
}

// 将两个list合并成一个tree
export function TwoListToTree(array1, array2) {
  let root = {}; // 根节点
  array1.forEach((node) => {
    node.children = [];
    root[node.id] = node;
  });

  array2.forEach((rel) => {
    let parent = root[rel.parentId];
    if (parent) {
      parent.children.push({ label: rel.label,id: rel.id, parentId: rel.parentId });
    }
  });
  return Object.values(root);
}

export function arrayToTree(arr, root) {
  var tree = [], map = {};
  for (var i = 0; i < arr.length; i++) {
      var node = arr[i];
      node.children = [];
      map[node.id] = i; // 建立id与数组下标的映射
      if (node.parentId === root) { // 找到根节点
          tree.push(node);
      } else { // 将非根节点挂在其父节点下面
          var parentIndex = map[node.parentId];
          if (parentIndex !== undefined) {
              arr[parentIndex].children.push(node);
          }
      }
  }
  // 循环遍历树，将 children 长度为 0 的节点的 children 字段删除
  function removeEmptyChildren(node) {
      if (node.children.length === 0) {
          delete node.children;
      } else {
          for (var i = 0; i < node.children.length; i++) {
              removeEmptyChildren(node.children[i]);
          }
      }
  }
  for (var i = 0; i < tree.length; i++) {
      removeEmptyChildren(tree[i]);
  }
  return tree;
}
/**
 * @description        数组转树形数据
 * @param {数据数组}    list
 * @param {树结构配置}  config
 */
export function listToTree(list, config) {
  let conf = {}
  Object.assign(conf, config)
  const nodeMap = new Map()
  const result = []
  const { id, children, pid } = conf
  for (const node of list) {
    // node[children] = node[children] || [];
    nodeMap.set(node[id], node)
  }
  for (const node of list) {
    const parent = nodeMap.get(node[pid])
      ; (parent
        ? parent.children
          ? parent.children
          : (parent.children = [])
        : result
      ).push(node)
  }
  return result
}
