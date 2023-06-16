using Hhmocon.Mes.Application;
using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Hhmocon.Mes.WebApi.Controllers.UpLoadFiles
{
    /// <summary>
    /// 文件上传控制器  流式上传大文件
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "IOFile", IgnoreApi = false)]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        private readonly ILogger<UploadFilesController> _logger;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BaseFilesApp _baseFilesApp;
        private readonly PikachuApp _pikachuApp;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        public UploadFilesController(ILogger<UploadFilesController> logger, IConfiguration config, IWebHostEnvironment webHostEnvironment,
            BaseFilesApp baseFilesApp, PikachuApp pikachuApp)
        {
            _logger = logger;
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _baseFilesApp = baseFilesApp;
            _pikachuApp = pikachuApp;
        }

        [HttpPost]
        public async Task<Response<string>> UploadLargeFile()
        {
            HttpRequest request = HttpContext.Request;
            Response<string> response = new();
            //验证请求文本类型
            if (!request.HasFormContentType ||   //如果请求头不是表单
                !MediaTypeHeaderValue.TryParse(request.ContentType, out MediaTypeHeaderValue mediaTypeHeader) || //如果请求头类型不可以被刨析为媒体类型及其参数
                string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))//如果找不到http请求的边界
            {
                response.Message = "不支持的媒体类型";
                return response;  //返回 不支持的媒体类型结果 [报错]
            }

            MultipartReader reader = new MultipartReader(mediaTypeHeader.Boundary.Value, request.Body);
            MultipartSection section = await reader.ReadNextSectionAsync();

            string tempPath = _config["upfileInfo:FileStorePath"];

            // 从请求中获取第一个文件并保存它
            // 根据你的需求改写代码
            while (section != null)
            {

                //对文本进行格式转换
                bool hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                    out ContentDispositionHeaderValue contentDisposition);

                //格式转换成功  && 文本类型为表单  &&  文件名不为空
                if (hasContentDispositionHeader && contentDisposition.DispositionType.Equals("form-data") &&
                    !string.IsNullOrEmpty(contentDisposition.FileName.Value))
                {
                    string[] propertyArray = new string[2];
                    propertyArray = section.ContentDisposition.Split("filename=\"");
                    string fileName = propertyArray[1];
                    int specialCharArrayIndex = fileName.IndexOf("\"");
                    fileName = fileName.Substring(0, specialCharArrayIndex);

                    //文件路径
                    string saveToPath = Path.Combine(tempPath, fileName);

                    using (FileStream targetStream = System.IO.File.Create(saveToPath))//创建文件流
                    {
                        await section.Body.CopyToAsync(targetStream);//将文本内容异步复制进文件流
                    }
                    response.Message = "上传成功";

                    return response;
                }

                section = await reader.ReadNextSectionAsync();
            }
            response.Message = "未收到文件请求";
            return response;
        }

        /// <summary>
        /// 表单文件传输
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Response<string>> UploadLargeFiles(List<IFormFile> files, string ProcessId, string MaterialId)
        {
            Response<string> result = new Response<string>();
            try
            {

                if (ProcessId == null)
                {
                    throw new Exception("请选中工序");
                }
                if (MaterialId == null)
                {
                    throw new Exception("请选中物料");
                }

                //long size = files.Sum(f => f.Length);
                string temp = Directory.GetCurrentDirectory();
                temp = Path.Combine(temp, "..");
                //temp = temp.Replace("\\Hhmocon.Mes\\Hhmocon.Mes.WebApi", "");
                foreach (IFormFile formFile in files)
                {
                    List<base_files> base_FilesList = new();
                    if (formFile.Length > 0)
                    {
                        string filePath = temp;

                        filePath = Path.Combine(filePath, "MES_API_FILE");
                        filePath = Path.Combine(filePath, formFile.FileName);
                        using (FileStream stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        base_files base_Files = new();
                        base_Files.material_id = MaterialId;
                        base_Files.material_name = _pikachuApp.GetById<base_material>(MaterialId)?.material_name;
                        base_Files.process_id = ProcessId;
                        base_Files.file_path = filePath;
                        base_Files.file_size = formFile.Length;
                        base_Files.file_name = formFile.FileName;
                        base_Files.file_time = DateTime.Now;
                        base_Files.file_type = formFile.ContentType.Substring(formFile.ContentType.LastIndexOf(@".") + 1);
                        base_FilesList.Add(base_Files);
                    }


                    if (_baseFilesApp.Insert(base_FilesList) < 0)
                    {
                        throw new Exception("数据库写入失败");
                    }
                }
                result.Result = "文件传输成功";
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
            //return Ok(new { count = files.Count, size });

        }

        /// <summary>
        /// 根据工序Id获取文档列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<PageData> GetListByProcessId(PageReq req, string processId)
        {
            Response<PageData> result = new Response<PageData>();
            try
            {
                PageData pd = new PageData();
                long lcount = 0;
                pd.Data = _baseFilesApp.GetListByProcessId(req, processId, ref lcount);
                pd.Total = lcount;
                result.Result = pd;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }
    }
}
