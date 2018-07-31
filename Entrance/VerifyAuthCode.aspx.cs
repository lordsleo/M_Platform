//
//文件名：    VerifyAuthCode.aspx.cs
//功能描述：  校验手机验证码
//创建时间：  2016/03/11
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
using Leo;
using ServiceInterface.Common;
using YGSoft.IPort.Data;


namespace M_Platform.Entrance
{
    public partial class VerifyAuthCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //手机号
            string strMobile = Request.Form["Mobile"];
            //验证码
            string strAuthCode = Request.Form["AuthCode"];

            ////手机号
            //string strMobile = Request.Params["Mobile"];
            ////验证码
            //string strAuthCode = Request.Params["AuthCode"];

            //strMobile = "18036600293";
            //strAuthCode = "603862";

            try
            {
                if (strMobile == null || strAuthCode == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，校验手机验证码失败！").DicInfo());
                    return;

                }

                string strSql =
                    string.Format(@"select * 
                                    from TB_APP_MOBILE_AUTHCODE 
                                    where mobile={0} and dynamic_endtime > sysdate 
                                    order by createtime desc", strMobile);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "验证码已超时！").DicInfo());
                    return;
                }

                //校验验证码
                if (Identity.RijndaelDecode(dt.Rows[0]["dynamic_authcode"].ToString()) != strAuthCode)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "验证码不匹配！").DicInfo());
                }
                else
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(true, null, null).DicInfo());
                }          
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：校验手机验证码数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}