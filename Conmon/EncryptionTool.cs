using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ServiceInterface.Conmon
{
    public class EncryptionTool
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strText">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));          
            return BitConverter.ToString(result).Replace("-", "");
        }  
    }
}