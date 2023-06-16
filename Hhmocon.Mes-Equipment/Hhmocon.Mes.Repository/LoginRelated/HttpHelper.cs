using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Hhmocon.Mes.Repository.LoginRelated
{
    /// <summary>
    /// http请求类
    /// </summary>
    public class HttpHelper
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseIPAddress;

        /// <summary>
        /// </summary>
        /// <param name="ipaddress">请求的基础IP，例如：http://192.168.0.33:8080/</param>
        public HttpHelper(string ipaddress = "")
        {
            _baseIPAddress = ipaddress;
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseIPAddress) };
        }
        /// <summary>
        /// Get请求数据
        /// <para>最终以url参数的方式提交</para>>
        /// </summary>
        /// <param name="parameters">参数字典，可为空</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public string Get(Dictionary<string, string> parameters, string requestUri)
        {
            if (parameters != null)
            {
                string strParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(ConcatURL(requestUri), '?', strParam);
            }
            else
            {
                requestUri = ConcatURL(requestUri);
            }
            Task<string> result = _httpClient.GetStringAsync(requestUri);
            return result.Result;
        }

        /// <summary>
        /// 以json的方式POST数据，返回string类型
        /// <para>最终以json的方式放置在http体中</para>
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public string Post(object entity, string requestUri)
        {
            string request = string.Empty;
            if (entity != null)
            {
                request = JsonHelper.Serialize(entity);
            }

            HttpContent httpContent = new StringContent(request);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Post(requestUri, httpContent);
        }

        /// <summary>
        /// Post Dic数据
        /// <prar>最终以formurlencode的方式放置在http体中</prar>>
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="requestUri"></param>
        /// <returns>System.String</returns>
        public string PostDic(Dictionary<string, string> temp, string requestUri)
        {
            HttpContent httpContent = new FormUrlEncodedContent(temp);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return Post(requestUri, httpContent);
        }


        private string Post(string requestUri, HttpContent content)
        {
            Task<HttpResponseMessage> result = _httpClient.PostAsync(ConcatURL(requestUri), content);
            return result.Result.Content.ReadAsStringAsync().Result;
        }
        /// <summary>
        /// 把请求的URL相对路径合成绝对路径
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private string ConcatURL(string requestUrl)
        {
            return new Uri(_httpClient.BaseAddress, requestUrl).OriginalString;
        }

        private AuthenticationHeaderValue CreateBasicCredentials(string username, string password)
        {
            string toEncode = username + ":" + password;
            // The current HTTP specification says characters here are ISO-8859-1.
            // However, the draft specification for the next version of HTTP indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] toBase64 = encoding.GetBytes(toEncode);
            string parameter = Convert.ToBase64String(toBase64);

            return new AuthenticationHeaderValue("Basic", parameter);
        }
    }
}
