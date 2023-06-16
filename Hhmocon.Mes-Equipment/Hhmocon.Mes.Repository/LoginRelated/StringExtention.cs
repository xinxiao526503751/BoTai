using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Hhmocon.Mes.Repository.LoginRelated
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtention
    {
        /// <summary>
        /// 用于判断是否为空字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsBlank(this string s)
        {
            return s == null || (s.Trim().Length == 0);
        }

        /// <summary>
        /// 用于判断是否不为空字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotBlank(this string s)
        {
            return !s.IsBlank();
        }

        /// <summary>
        /// 判断是否为有效的Email地址
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string s)
        {
            if (s.IsBlank())
            {
                return false;
            }

            const string pattern = @"^[A-Za-z\d]+([-_.][A-Za-z\d]+)*@([A-Za-z\d]+[-.])+[A-Za-z\d]{2,4}$";
            return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        /// 验证是否是合法的电话号码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsVaildPhone(this string s)
        {
            return s.IsBlank() || Regex.IsMatch(s, @"^\+?((\d{2,4}(-)?)|(\(\d{2,4}\)))*(\d{0,16})*$");
        }

        /// <summary>
        /// 验证是否合法的手机号码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsVaildMobile(this string s)
        {
            return !s.IsBlank() && Regex.IsMatch(s, @"^\+?\d{0,4}?[1][3-8]\d{9}$");
        }

        /// <summary>
        /// 验证是否合法的邮箱
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsVaildZipCode(this string s)
        {
            return s.IsBlank() || Regex.IsMatch(s, @"[1-9]\d{5}(?!\d)");
        }

        /// <summary>
        /// 将字符串转换成MD5加密字符串
        /// </summary>
        /// <param name="orgStr"></param>
        /// <returns></returns>
        public static string ToMD5(this string orgStr)
        {
            MD5 md5Hasher = MD5.Create();
            //Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(orgStr));
            // Create a new Stringbulider to collect the bytes
            //and create a string
            StringBuilder sBuilder = new StringBuilder();
            //Loop through each byte of the hashed data
            //and format each one as a hexadecimal string
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            //Return the hexadecimal string
            return sBuilder.ToString();
        }

        /// <summary>
        /// 验证是否是合法的传真
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsValidFax(this string s)
        {
            return s.IsBlank() || Regex.IsMatch(s, @"(^[0-9]{3,4}\-[0-9]{7,8}$)|(^[0-9]{7,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)");
        }

        /// <summary>
        /// 手机号中间四位打码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string MaskMobile(this string content)
        {
            const string overPlay = "****";
            const int start = 3;
            const int end = 7;
            return string.IsNullOrEmpty(content) ? "" : content.Overlay(overPlay, start, end);
        }

        /// <summary>
        /// 邮箱账号打码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string MaskEmail(this string email)
        {
            const string overPlay = "****";
            if (string.IsNullOrEmpty(email))
            {
                return "";
            }
            const string at = "@";
            if (!email.Contains(at))
            {
                return email;
            }
            //这里主要逻辑是需要保留邮箱的注册商
            //后四位打码，不足四位，除第一位都打码
            int length = email.IndexOf(at, StringComparison.Ordinal);
            string content = email.Substring(0, length);
            string mask;
            if (content.Length > 4)
            {
                mask = content.Overlay(overPlay, content.Length - 4, content.Length);
            }
            else
            {
                mask = content.Substring(0, 1) + overPlay;
            }
            return mask + email.Substring(length);

        }

        /// <summary>
        /// 身份证打码操作
        /// 中间8位打码，生日年月日
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string MaskIdCard(this string idCard)
        {
            const string overPlay = "********";
            const int start = 6;
            const int end = 14;
            return string.IsNullOrEmpty(idCard) ? "" : idCard.Overlay(overPlay, start, end);
        }

        /// <summary>
        /// 银行卡除了后面其他打码
        /// </summary>
        /// <param name="bankCard"></param>
        /// <returns></returns>
        public static string MaskBankCard(this string bankCard)
        {
            int end = bankCard.Length - 4;
            StringBuilder overPlay = new StringBuilder();
            for (int i = 0; i < end; i++)
            {
                overPlay.Append("*");
            }
            return bankCard.Overlay(overPlay.ToString(), 0, end);
        }

        /// <summary>
        /// 密码全部打码
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string MaskPwd(this string pwd)
        {
            int end = pwd.Length;
            StringBuilder overPlay = new StringBuilder();
            for (int i = 0; i < end; i++)
            {
                overPlay.Append("*");
            }
            return pwd.Overlay(overPlay.ToString(), 0, end);
        }

        /// <summary>
        /// 中文姓名，除了第一位不打码
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string maskName(this string name)
        {
            int end = name.Length;
            StringBuilder overPlay = new StringBuilder();
            for (int i = 1; i < end; i++)
            {
                overPlay.Append("*");
            }
            return name.Overlay(overPlay.ToString(), 1, end);
        }

        /// <summary>
        /// overlays part of a string with another string
        /// </summary>
        /// <param name="str">the string to do overlay in,maybe null</param>
        /// <param name="overlay">the string to overlay,maybe null</param>
        /// <param name="start">the position to start overlaying at</param>
        /// <param name="end">the position to stop overlaying before </param>
        /// <returns></returns>
        public static string Overlay(this string str, string overlay, int start, int end)
        {
            if (str == null)
            {
                return null;
            }
            if (overlay == null)
            {
                overlay = string.Empty;
            }
            int len = str.Length;
            if (start < 0)
            {
                start = 0;
            }
            if (start > len)
            {
                start = len;
            }
            if (end < 0)
            {
                end = 0;
            }
            if (end > len)
            {
                end = len;
            }

            if (start <= end)
            {
                return new StringBuilder(len + start - end + overlay.Length + 1)
                    .Append(str.Substring(0, start))
                    .Append(overlay)
                    .Append(str.Substring(end))
                    .ToString();
            }

            int temp = start;
            start = end;
            end = temp;
            return new StringBuilder(len + start - end + overlay.Length + 1)
                .Append(str.Substring(0, start))
                .Append(overlay)
                .Append(str.Substring(end))
                .ToString();
        }
    }
}
