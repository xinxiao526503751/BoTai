/* 
┏━━━━━━━━━━━＼／━━━━━━━━━━━┓      
┃┏━━━━━━━━━━━━━━━━━━━━━━┓┃
     ------------------------------------------    
       Author           : TengSea   
       Created          : Mouth-Day-Year                              
       Last Modified By : TengSea                                 
       Last Modified On : Mouth-Day-Year                                                               
       Description      : 
     __________________________________________
     Copyright (c) TengSea. All rights reserved.
 ┃┗━━━━━━━━━━━━━━━━━━━━━━┛┃                            
 ┗━━━━━━━━━∪━━━━∪━━━━━━━━━┛
 */

using Hhmocon.Mes.Application.Response;
using Hhmocon.Mes.Repository;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Repository.Response;
using Hhmocon.Mes.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hhmocon.Mes.Application.Base
{
    /// <summary>
    /// 产品-缺陷关联App
    /// </summary>
    public class BaseDefectiveProductApp
    {
        private readonly BaseDefectiveProductRepostiory _baseDefectiveProductRepository;
        private readonly PikachuRepository _pikachuRepository;
        private readonly IAuth _auth;
        private readonly ILogger<BaseDefectiveProductApp> _logger;
        public BaseDefectiveProductApp(BaseDefectiveProductRepostiory IBaseDefectiveProductRepostiory, PikachuRepository pikachuRepository, IAuth auth, ILogger<BaseDefectiveProductApp> logger)
        {
            _baseDefectiveProductRepository = IBaseDefectiveProductRepostiory;
            _pikachuRepository = pikachuRepository;
            _auth = auth;
            _logger = logger;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public base_defective_product Insert(base_defective_product data)
        {
            //取ID
            data.defective_product_id = CommonHelper.GetNextGUID();
            data.defective_type_id = _pikachuRepository.GetById<base_defective_reason>(data.defective_reason_id)?.defective_type_id;
            if (data.defective_type_id == null)
            {
                throw new Exception("未能根据原因找到对应找到不合格类型");
            }

            data.process_name = _pikachuRepository.GetById<base_process>(data.process_id)?.process_name;
            data.defective_reason_name = _pikachuRepository.GetById<base_defective_reason>(data.defective_reason_id)?.defective_reason_name;
            data.modified_time = Time.Now;
            data.create_time = DateTime.Now;
            data.create_by = _auth.GetUserAccount(null);
            data.create_by_name = _auth.GetUserName(null);
            data.modified_by = _auth.GetUserAccount(null);
            data.modified_by_name = _auth.GetUserName(null);

            if (_pikachuRepository.Insert(data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// defectivestatistic不合格产品统计页面的环状图
        /// 查询日期范围(year-mouth ~ year-mouth)内不和格原因的前8名
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public List<GetProductsNumberByTimeDonotChartResponse> GetProductsNumberByTimeDonotChartApp(DateTime StartTime, DateTime EndTime)
        {
            try
            {
                PageData pd = new PageData();
                List<GetProductsNumberByTimeDonotChartResponse> data = new();

                //如果查询的开始时间大于结束时间要报错
                if (DateTime.Compare(StartTime, EndTime) > 0)
                {
                    throw new Exception("对不合格产品查询的开始时间不能迟于结束时间");
                }
                //找到所有不合格原因
                List<base_defective_reason> base_Defective_Reasons = _pikachuRepository.GetAll<base_defective_reason>();
                //收集前8个数量最多的不合格原因的收集柜
                List<GetProductsNumberByTimeDonotChartResponse> DonotChartResponses = new();

                foreach (base_defective_reason base_Defective_Reason in base_Defective_Reasons)
                {
                    int value = 0;
                    //找到不合格原因下 时间范围内 的产品
                    List<base_defective_product> Defective_Products = _baseDefectiveProductRepository.GetByTimeScopeAndReason(StartTime, EndTime, base_Defective_Reason);

                    //统计数量
                    foreach (base_defective_product temp_product in Defective_Products)
                    {
                        value += temp_product.defective_num;
                    }


                    GetProductsNumberByTimeDonotChartResponse TempResponse = new();
                    TempResponse.name = base_Defective_Reason.defective_reason_name;
                    TempResponse.value = value;
                    if (TempResponse.value != 0)
                    {
                        //如果收集柜的数量不到8
                        if (DonotChartResponses.Count != 8)
                        {
                            //放进收集柜
                            DonotChartResponses.Add(TempResponse);
                            continue;
                        }
                    }

                    //如果收集柜的数量满了8
                    //对收集柜的内容按照数量从大到小排序
                    DonotChartResponses = DonotChartResponses.OrderByDescending(c => c.value).ToList();
                    //如果有不合格原因产品数量大于收集柜已有的最少原因的产品数，就换进去
                    DonotChartResponses.ForEach(c => { if (c.value < value) { c = TempResponse; } });


                }

                return DonotChartResponses;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw new Exception(exception.InnerException?.Message ?? exception.Message);
            }
        }

        public List<GetProductsNumberByTimeDonotChartResponse> GetProductsNumberByTimeApp(DateTime StartTime, DateTime EndTime)
        {
            try
            {
                PageData pd = new PageData();
                List<GetProductsNumberByTimeDonotChartResponse> data = new();

                //如果查询的开始时间大于结束时间要报错
                if (DateTime.Compare(StartTime, EndTime) > 0)
                {
                    throw new Exception("对不合格产品查询的开始时间不能小于结束时间");
                }
                //找到所有不合格原因
                List<base_defective_reason> base_Defective_Reasons = _pikachuRepository.GetAll<base_defective_reason>();
                //收集前8个数量最多的不合格原因的收集柜
                List<GetProductsNumberByTimeDonotChartResponse> DonotChartResponses = new();

                foreach (base_defective_reason base_Defective_Reason in base_Defective_Reasons)
                {
                    //找到不合格原因下 时间范围内 的产品
                    List<base_defective_product> Defective_Products = _baseDefectiveProductRepository.GetByTimeScopeAndReason(StartTime, EndTime, base_Defective_Reason);
                    int value = Defective_Products.Count;

                    //统计数量
                    foreach (base_defective_product temp_product in Defective_Products)
                    {
                        value += temp_product.defective_num;
                    }


                    GetProductsNumberByTimeDonotChartResponse TempResponse = new();
                    TempResponse.name = base_Defective_Reason.defective_reason_name;
                    TempResponse.value = value;

                    //如果收集柜的数量不到8
                    if (DonotChartResponses.Count != 8)
                    {
                        //放进收集柜
                        DonotChartResponses.Add(TempResponse);
                        continue;
                    }
                    //如果收集柜的数量满了8
                    //对收集柜的内容按照数量从大到小排序
                    DonotChartResponses = DonotChartResponses.OrderByDescending(c => c.value).ToList();
                    //如果有不合格原因产品数量大于收集柜已有的最少原因的产品数，就换进去
                    if (DonotChartResponses[7].value < value)
                    {
                        DonotChartResponses[7] = TempResponse;
                    }
                }

                return DonotChartResponses;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw new Exception(exception.InnerException?.Message ?? exception.Message);
            }
        }

        /// <summary>
        /// 时间范围内按照月份统计不同不合格类及数据
        /// </summary>
        /// <returns></returns>
        public UnqulifiedStaticsByTime UnqualifiedStatisticsByTimeApp(string StartTime, string EndTime)
        {
            try
            {
                string[] SplitArray_Start = StartTime.Split('-');
                int StartYear = SplitArray_Start[0].ToInt();
                int StartMouth = SplitArray_Start[1].ToInt();
                if (StartMouth > 12 || StartMouth < 0)
                {
                    throw new Exception("检索条件的开始时间的月份数据异常");
                }
                string[] SplitArray_End = EndTime.Split('-');
                int EndYear = SplitArray_End[0].ToInt();
                int EndMouth = SplitArray_End[1].ToInt();

                int Gap_Year = EndYear - StartYear;//间隔多少年
                int Gap_Mouth = 0;
                //计算间隔月份
                if (Gap_Year >= 2)
                {
                    Gap_Mouth = (Gap_Year - 1) * 12 + EndMouth + (12 - StartMouth);
                }
                else if (Gap_Year == 1)
                {
                    Gap_Mouth = (12 - StartMouth) + EndMouth;
                }
                else if (Gap_Year == 0)
                {
                    Gap_Mouth = EndMouth - StartMouth;
                }


                //制作初始表头
                List<right_header> right_Headers = new();
                right_header right_HeaderTemp1 = new();
                right_HeaderTemp1.label = "年-月";
                right_HeaderTemp1.key = "Year-Mouth";
                right_Headers.Add(right_HeaderTemp1);
                right_header right_HeaderTemp2 = new();
                right_HeaderTemp2.label = "总数";
                right_HeaderTemp2.key = "Total";
                right_Headers.Add(right_HeaderTemp2);

                List<base_defective_reason> reasons = _pikachuRepository.GetAll<base_defective_reason>();

                //表格数据
                List<Dictionary<string, string>> header_Datas = new();

                foreach (base_defective_reason base_Defective_Reason in reasons)
                {
                    //制作表头
                    right_header right_Header = new();
                    right_Header.label = base_Defective_Reason.defective_reason_name;
                    right_Header.key = base_Defective_Reason.defective_reason_code;
                    right_Headers.Add(right_Header);
                }


                for (int i = 0, temp_mouth = StartMouth; i < Gap_Mouth + 1; i++, temp_mouth++)//遍历月份
                {
                    Dictionary<string, string> header_Data = new();


                    if (temp_mouth > 12)
                    {
                        temp_mouth = 1;
                        StartYear += 1;
                    }

                    //制作表格数据
                    string time = StartYear.ToString() + "-" + temp_mouth + "-01 00:00:00";
                    //当月开始时间 xx-xx-xx xx:xx:xx
                    DateTime StartTime_YMD = time.ToDate();


                    //大月
                    string[] BigMouth = { "01", "03", "05", "10", "12" };
                    //小月
                    string[] SmallMouth = { "04", "06", "07", "08", "09", "11" };
                    //平月
                    string[] nonleapMouth = { "02" };


                    //如果月份是大月
                    if (BigMouth.Contains(temp_mouth.ToString().PadLeft(2, '0')))
                    {
                        time = StartYear.ToString() + "-" + temp_mouth + "-31 23:59:59";
                    }
                    else if (SmallMouth.Contains(temp_mouth.ToString().PadLeft(2, '0')))
                    {
                        time = StartYear.ToString() + "-" + temp_mouth + "-30 23:59:59";
                    }
                    else if (nonleapMouth.Contains(temp_mouth.ToString().PadLeft(2, '0')))
                    {
                        //闰年
                        if (StartYear % 400 == 0)//能被400整除是闰年
                        {
                            time = StartYear.ToString() + "-" + temp_mouth + "-29 23:59:59";
                        }
                        //能被4整除不能被100整除的是闰年
                        else if (StartYear % 4 == 0 && StartYear % 100 != 0)
                        {
                            time = StartYear.ToString() + "-" + temp_mouth + "-29 23:59:59";
                        }
                        else
                        {
                            time = StartYear.ToString() + "-" + temp_mouth + "-28 23:59:59";
                        }
                    }

                    //当月结束时间
                    DateTime EndTime_YMD = time.ToDate();

                    header_Data.Add("Year-Mouth", StartTime_YMD.GetDateTimeFormats('y')[0].ToString());

                    //当月总数
                    int persentMouthTotal = 0;
                    //遍历reason
                    foreach (base_defective_reason base_Defective_Reason in reasons)
                    {
                        //按照reason查找当月的数据
                        List<base_defective_product> MouthProducts = _baseDefectiveProductRepository.GetByTimeScopeAndReason(StartTime_YMD, EndTime_YMD, base_Defective_Reason);

                        //同一原因下瑕疵产品总数
                        int reason_product_total = 0;

                        //统计数量
                        foreach (base_defective_product temp_product in MouthProducts)
                        {
                            reason_product_total += temp_product.defective_num;
                        }
                        //制作表
                        header_Data.Add($"{base_Defective_Reason.defective_reason_code}", $"{reason_product_total}");

                        persentMouthTotal += reason_product_total;
                    }
                    header_Data.Add("Total", $"{persentMouthTotal}");
                    header_Datas.Add(header_Data);
                }

                UnqulifiedStaticsByTime unqulifiedStaticsByTime = new();
                unqulifiedStaticsByTime.headerDatas = header_Datas;
                unqulifiedStaticsByTime.right_Headers = right_Headers;

                return unqulifiedStaticsByTime;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw new Exception(exception.InnerException?.Message ?? exception.Message);
            }

        }
    }
}
