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

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Hhmocon.Mes.Util.CommunicationUtil
{
    public class WeCharEnterpriseUtil
    {
        /// <summary>
        /// 向企业微信获得许可凭证AccessToken
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            IConfigurationRoot configuration = builder.Build();

            //corpig是企业id，corpsecret是企业秘匙
            string uri = $"https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={configuration["EnterpriseWeiChat:corpid"]}&corpsecret={configuration["EnterpriseWeiChat:corpsecret"]}";

            //创建请求
            WebRequest request = WebRequest.Create(uri);
            //请求设置
            request.Credentials = CredentialCache.DefaultCredentials;
            //分配已登录用户或正在模拟的用户的凭据//只適用于 NTLM、negotiate 和 Kerberos 型驗證。
            //表目前正在執行應用程式之安全性內容的系統認證。
            //針對用戶端應用程式，
            //這些通常是執行應用程式之使用者的使用者名稱、密碼和網域的 Windows 認證。
            //針對 ASP.NET 應用程式，預設認證是已登入使用者的使用者認證，或是正在模擬的使用者。

            //创建应答接收
            WebResponse response = request.GetResponse();//创建应答接收对象其实就是在完成请求并接收

            string accessToken;

            //创建应答读写流
            using (Stream streamResponse = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(streamResponse);
                string responseFromServer = reader.ReadToEnd();
                JObject res = (JObject)JsonConvert.DeserializeObject(responseFromServer);
                accessToken = res["access_token"].ToString();
                reader.Close();
            }
            //关闭响应
            response.Close();
            return accessToken;
        }

        /// <summary>
        /// 企业消息群发
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="user_ids"></param>
        public static void PostMail(string accessToken, List<string> user_ids, string message = "不明往事其源不解悲凉之深")
        {
            //POST的API
            string uri = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + accessToken;
            //创建请求
            WebRequest myWebRequest = WebRequest.Create(uri);
            //请求设置
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;//用户凭据
            myWebRequest.ContentType = "application/json;charset=UTF-8";//请求文本格式
            myWebRequest.Method = "POST";                               //请求格式
            //向服务器发送的内容
            using (Stream streamResponse = myWebRequest.GetRequestStream())//通过请求对象或者应答对象创建Stream来获取数据 是因为请求或者应答中包含了数据格式 不然直接创建一般的Stream会乱码
            {

                ConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();

                string touser = string.Join("|", user_ids);
                //创建JSON格式的发送内容
                JObject postedJObject = new JObject
                {
                    //在此处设置发送内容及对象
                    { "touser", $"{touser}" },
                    { "msgtype", "text" },
                    { "agentid", configuration["EnterpriseWeiChat:agentid"] }
                };

                JObject text = new JObject
                {
                    {"content",$"{message}" }
                };
                postedJObject.Add("text", text);
                postedJObject.Add("safe", 0);
                //将传送内容编码
                string paramString = postedJObject.ToString(Formatting.None, null);
                byte[] byteArray = Encoding.UTF8.GetBytes(paramString);
                //向请求中写入内容
                streamResponse.Write(byteArray, 0, byteArray.Length);
            }
            //创建应答
            WebResponse myWebResponse = myWebRequest.GetResponse();//发送请求获取应答
            //创建应答的读写流
            string responseFromServer;
            using (Stream streamResponse = myWebResponse.GetResponseStream())
            {
                StreamReader streamRead = new(streamResponse);
                responseFromServer = streamRead.ReadToEnd();
            }
            //关闭应答
            myWebResponse.Close();
        }
    }
}
