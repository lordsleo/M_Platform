using Leo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M_Platform.Entrance
{
    /// <summary>
    /// 注册接口，参数项工具
    /// </summary>
    public class RegisterParamsTool
    {
        public static string[] GetParams(string strAppName)
        { 
            switch (strAppName)
            {
                //滴滴进港
                case "DDJG":
                    string[] DDJG = { "Mobile", "Password1", "Password2" };    
                    return DDJG;
                //速配货
                case "SPH":
                    string[] SPH = { "Mobile", "Logogram", "Password1", "Password2" };
                    return SPH;
                default:
                    return null;
            }
        }
    }
}