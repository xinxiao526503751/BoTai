using Hhmocon.Mes.Application.Base;
using Hhmocon.Mes.Repository.Domain;
using Hhmocon.Mes.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hhmocon.Mes.WebApi.Controllers.Files.DeleteFiles
{
    [Route("api/[controller]/[action]")]
    [EnableCors(PolicyName = "_hhmoconmesAllowSpecificOrigins")]
    [ApiExplorerSettings(GroupName = "IOFile", IgnoreApi = false)]
    [ApiController]
    public class DeleteFilesController : ControllerBase
    {

        private readonly PikachuApp _pikachuApp;
        public DeleteFilesController(PikachuApp pikachuApp)
        {
            _pikachuApp = pikachuApp;
        }

        [HttpPost]
        public Response<ContentResult> DeleteFile(string fileId)
        {
            Response<ContentResult> response = new();
            try
            {
                base_files _files = _pikachuApp.GetById<base_files>(fileId);
                if (System.IO.File.Exists(_files.file_path))
                {
                    //删除文件
                    System.IO.File.Delete(_files.file_path);
                    _files.delete_mark = 1;
                    _pikachuApp.Update(_files);
                    response.Message = "删除成功";
                }
                else
                {
                    string[] a = new string[1] { fileId };
                    _pikachuApp.DeleteMask<base_files>(a);
                    throw new Exception("未找到要删除的文件,已将失效记录清除");
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Code = 500;
                response.Message = $"{ex.Message}";
                return response;
            }

        }
    }
}
