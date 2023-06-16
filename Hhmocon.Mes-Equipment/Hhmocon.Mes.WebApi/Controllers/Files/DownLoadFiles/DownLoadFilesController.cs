using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Hhmocon.Mes.WebApi.
    s.DownLoadFiles
{
    /// <summary>
    /// 流式下载大文件控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "IOFile", IgnoreApi = false)]
    [ApiController]
    public class DownLoadFilesController : ControllerBase
    {
        private readonly IConfiguration _config;
        public DownLoadFilesController(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// DownloadBigFile用于下载大文件，循环读取大文件的内容到服务器内存，然后发送给客户端浏览器
        /// 测试发现文件损坏
        /// </summary>
        [HttpPost]
        public ActionResult DownloadBigFileNot(string fileName)
        {
            string temp = Directory.GetCurrentDirectory();
            temp = Path.Combine(temp, "..");
            string filePath = temp;
            filePath = Path.Combine(filePath, fileName);

            int bufferSize = 1024;//这就是ASP.NET Core循环读取下载文件的缓存大小，这里我们设置为了1024字节，也就是说ASP.NET Core每次会从下载文件中读取1024字节的内容到服务器内存中，然后发送到客户端浏览器，这样避免了一次将整个下载文件都加载到服务器内存中，导致服务器崩溃

            MemoryStream memoryStream = new();
            using (FileStream stream = new(filePath, FileMode.Open))
            {
                string na = stream.Name;
                stream.CopyTo(memoryStream);
            }

            int dotIndex = fileName.LastIndexOf(".");
            string fileExt = fileName.Substring(dotIndex);
            //获取文件的ContentType
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            Response.ContentType = provider.Mappings[fileExt]; ;//根据下载的不同文件设置不同的ContentType

            string contentDisposition = "attachment;" + "filename=" + HttpUtility.UrlEncode(fileName);//在Response的Header中设置下载文件的文件名，这样客户端浏览器才能正确显示下载的文件名，注意这里要用HttpUtility.UrlEncode编码文件名，否则有些浏览器可能会显示乱码文件名
            Response.Headers.Add("Content-Disposition", new string[] { contentDisposition });

            //使用FileStream开始循环读取(3.1之后默认异步读取，想允许同步需要在服务中设置)要下载文件的内容
            using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read))
            {
                using (Response.Body)//调用Response.Body.Dispose()并不会关闭客户端浏览器到ASP.NET Core服务器的连接，之后还可以继续往Response.Body中写入数据
                {
                    long contentLength = fs.Length;//获取下载文件的大小
                    Response.ContentLength = contentLength;//在Response的Header中设置下载文件的大小，这样客户端浏览器才能正确显示下载的进度

                    //Memory<byte> buffer;
                    byte[] buffer;

                    long hasRead = 0;//变量hasRead用于记录已经发送了多少字节的数据到客户端浏览器

                    //如果hasRead小于contentLength，说明下载文件还没读取完毕，继续循环读取下载文件的内容，并发送到客户端浏览器
                    while (hasRead < contentLength)
                    {
                        //HttpContext.RequestAborted.IsCancellationRequested可用于检测客户端浏览器和ASP.NET Core服务器之间的连接状态，如果HttpContext.RequestAborted.IsCancellationRequested返回true，说明客户端浏览器中断了连接
                        if (HttpContext.RequestAborted.IsCancellationRequested)
                        {
                            //如果客户端浏览器中断了到ASP.NET Core服务器的连接，这里应该立刻break，取消下载文件的读取和发送，避免服务器耗费资源
                            break;
                        }
                        buffer = new byte[bufferSize];

                        int currentRead = fs.Read(buffer, 0, bufferSize);//从数据流中读取bufferSize大小的内容到服务器内存中

                        Response.Body.Write(buffer, 0, currentRead);//发送读取的内容数据到客户端浏览器
                        Response.Body.Flush();//注意每次Write后，要及时调用Flush方法，及时释放服务器内存空间

                        hasRead += currentRead;//更新已经发送到客户端浏览器的字节数
                    }
                }
            }

            return new EmptyResult();
        }



        /// <summary>
        /// 代码作废，异步下载目前不知道怎么通过swagger测试，也缺少前端代码进行测试，不知道这种写法能不能用
        /// 2021/11/2
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DownloadBigFileAsync(string fileName)
        {
            string temp = Directory.GetCurrentDirectory();
            temp = Path.Combine(temp, "..");
            string filePath = temp;
            filePath = Path.Combine(filePath, fileName);

            int bufferSize = 1024;//这就是ASP.NET Core循环读取下载文件的缓存大小，这里我们设置为了1024字节，也就是说ASP.NET Core每次会从下载文件中读取1024字节的内容到服务器内存中，然后发送到客户端浏览器，这样避免了一次将整个下载文件都加载到服务器内存中，导致服务器崩溃

            MemoryStream memoryStream = new MemoryStream();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                string na = stream.Name;
                await stream.CopyToAsync(memoryStream);
            }

            int dotIndex = fileName.LastIndexOf(".");
            string fileExt = fileName.Substring(dotIndex);
            //获取文件的ContentType
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            Response.ContentType = provider.Mappings[fileExt]; ;//根据下载的不同文件设置不同的ContentType

            string contentDisposition = "attachment;" + "filename=" + HttpUtility.UrlEncode(fileName);//在Response的Header中设置下载文件的文件名，这样客户端浏览器才能正确显示下载的文件名，注意这里要用HttpUtility.UrlEncode编码文件名，否则有些浏览器可能会显示乱码文件名
            Response.Headers.Add("Content-Disposition", new string[] { contentDisposition });

            //使用FileStream开始循环读取(3.1之后默认异步读取，想允许同步需要在服务中设置)要下载文件的内容
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (Response.Body)//调用Response.Body.Dispose()并不会关闭客户端浏览器到ASP.NET Core服务器的连接，之后还可以继续往Response.Body中写入数据
                {
                    long contentLength = fs.Length;//获取下载文件的大小
                    Response.ContentLength = contentLength;//在Response的Header中设置下载文件的大小，这样客户端浏览器才能正确显示下载的进度

                    Memory<byte> buffer;

                    long hasRead = 0;//变量hasRead用于记录已经发送了多少字节的数据到客户端浏览器

                    //如果hasRead小于contentLength，说明下载文件还没读取完毕，继续循环读取下载文件的内容，并发送到客户端浏览器
                    while (hasRead < contentLength)
                    {
                        //HttpContext.RequestAborted.IsCancellationRequested可用于检测客户端浏览器和ASP.NET Core服务器之间的连接状态，如果HttpContext.RequestAborted.IsCancellationRequested返回true，说明客户端浏览器中断了连接
                        if (HttpContext.RequestAborted.IsCancellationRequested)
                        {
                            //如果客户端浏览器中断了到ASP.NET Core服务器的连接，这里应该立刻break，取消下载文件的读取和发送，避免服务器耗费资源
                            break;
                        }

                        buffer = new byte[bufferSize];

                        int currentRead = await fs.ReadAsync(buffer);//从下载文件中读取bufferSize(1024字节)大小的内容到服务器内存中
                        ReadOnlyMemory<byte> readOnlyMemory = buffer;

                        await Response.Body.WriteAsync(readOnlyMemory);//发送读取的内容数据到客户端浏览器
                        await Response.Body.FlushAsync();//注意每次Write后，要及时调用Flush方法，及时释放服务器内存空间

                        hasRead += currentRead;//更新已经发送到客户端浏览器的字节数
                    }
                }
            }

            return new EmptyResult();
        }


        /// <summary>
        /// 文件流的方式输出 MEMI类型映射下载        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public FileStreamResult DownLoad(string file)
        {

            string temp = Directory.GetCurrentDirectory();
            temp = Path.Combine(temp, "..");
            string filePath = Path.Combine(_config["upfileInfo:FileStorePath"], temp);
           
            filePath = Path.Combine(filePath, "MES_API_FILE");
            filePath = Path.Combine(filePath, file);

            string addrUrl = filePath;
            if (!System.IO.File.Exists(addrUrl))
            {
                throw new Exception("文件不存在");
            }
            // FileStream stream = System.IO.File.OpenRead(addrUrl);
            int dotIndex = file.LastIndexOf(".");
            if (dotIndex < 0)
            {
                throw new Exception("文件格式不对");
            }
            string fileExt = file.Substring(dotIndex);
            //获取文件的ContentType
            FileExtensionContentTypeProvider provider = new();
            //MEMI类型映射
            string memi = provider.Mappings[fileExt];

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            FileStreamResult Result = File(fileStream, memi, addrUrl);
            return Result;

        }



    }


}
