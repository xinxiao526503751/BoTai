using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hhmocon.Mes.Repository.LoginRelated
{
    /// <summary>
    /// JWT帮助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJWT(TokenModelJWT tokenModel)
        {
            string iss = Appsettings.App("Audience", "Issuer");
            string aud = Appsettings.App("Audience", "Audience");
            string secret = Appsettings.App("Audience", "Secret");//指定一个密钥（secret）

            List<Claim> claims = new List<Claim>
                    {
                        //下边为Claim的默认配置
                    new Claim(JwtRegisteredClaimNames.Jti, tokenModel.UId),
                    new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,

                    //这个就是过期时间，目前是过期1天，可自定义，注意JWT有自己的缓冲过期时间
                    new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(24*60*60)).ToUnixTimeSeconds()}"),//过期时间
                    new Claim(JwtRegisteredClaimNames.Iss,iss),//签发人
                    new Claim(JwtRegisteredClaimNames.Aud,aud),//受众

                    new Claim(ClaimTypes.Name,tokenModel.Account),

                   };
            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            string encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJWT SerializeJWT(string jwtStr)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object account;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Name, out account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            TokenModelJWT tm = new TokenModelJWT
            {
                UId = jwtToken.Id,
                Account = account != null ? account.ToString() : ""
            };
            return tm;
        }

        /// <summary>
        /// 判断是否是jwt字符串
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static bool CanReadToken(string jwtStr)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.CanReadToken(jwtStr);
        }
    }



    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJWT
    {
        /// <summary>
        /// Id
        /// </summary>
        public string UId { get; set; }
        /// <summary>
        /// Account
        /// </summary>
        public string Account { get; set; }
    }

}

