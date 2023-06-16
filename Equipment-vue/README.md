# mes

> A Vue.js project

## Build Setup

``` bash
# install dependencies
npm install

# serve with hot reload at localhost:8080
npm run dev

# build for production with minification
npm run build

# build for production and view the bundle analyzer report
npm run build --report
```

For a detailed explanation on how things work, check out the [guide](http://vuejs-templates.github.io/webpack/) and [docs for vue-loader](http://vuejs.github.io/vue-loader).

## 出入库界面

### 入库弹框

子组件：

（1）弹框显隐：dialogVisible

（2）入库类型查询

（3）物料查询：

1、获取物料类型树

2、获取物料列表

3、将选中的物料名称显示在输入框内

（4）库位

1、点击仓库后会获取该仓库下的库位；没有点击仓库时会获取所有库位

（5）数量

decimal(18, 4)代表小数和整数加起来总共18位，其中小数4位，小数点也占一位，所以整数最多只有13位