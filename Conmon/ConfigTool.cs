//
//文件名：    FileTool.aspx.cs
//功能描述：  文件（文本、图片、语音等）工具类
//创建时间：  2015/07/09
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;

namespace ServiceInterface.Common
{
    public class ConfigTool
    {
        /// <summary>
        /// 获取Web.config应用设置。
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>设置值</returns>
        public static string GetWebConfigKey(string key)
        {
            return WebConfigurationManager.AppSettings[key];
        }
    }
}