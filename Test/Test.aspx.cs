//
//文件名：    Test.aspx.cs
//功能描述：  接口测试
//创建时间：  2016/03/19
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using ServiceInterface.Common;

namespace M_Platform.Test
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //参数1
            string strParam1 = Request.Params["Param1"];
            //参数2
            string strParam2 = Request.Params["Param2"];

            try
            {
                if (strParam1 == null || strParam2 == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，登陆失败！").DicInfo());
                    return;
                }

                string[] strArray = new string[4];
                strArray[0] = strParam1;
                strArray[1] = strParam2;
                strArray[2] = Request.Params["AppName"];
                strArray[3] = Request.Params["Sign"];

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());

                

            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：用户登陆数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}