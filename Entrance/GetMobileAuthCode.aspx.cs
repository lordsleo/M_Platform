//
//文件名：    GetMobileAuthCode.aspx.cs
//功能描述：  获取手机验证码
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
using YGSoft.IPort.Data;
using ServiceInterface.Common;

namespace M_Platform.Entrance
{
    public partial class GetMobileAuthCode : System.Web.UI.Page
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
            string strMobile = Request.Params["Mobile"];

            //strMobile = "18036600293";
            //strAppName = "速配货";
          
            try
            {
                if (strMobile == null)
                {                 
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，获取验证码失败！").DicInfo());
                    return;

                }

                //校验手机号
                Json = VerifyMobile(strMobile);
                if (!string.IsNullOrWhiteSpace(Json))
                {
                    return;
                }

                //生成随机验证码
                string strSql =
                    string.Format("select round(dbms_random.value(100000,1000000)) as authcode from dual");
                var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHmw).ExecuteTable(strSql);
                //if (dt0.Rows.Count <= 0)
                //{
                //    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "网络错误，请稍后再试！").DicInfo());
                //    return;
                //}

                //获取应用中文名称
                strSql =
                    string.Format(@"select fullname 
                                    from VW_APP_TOKEN 
                                    where appname='{0}'",
                                    Request.Params["AppName"]);
                var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteTable(strSql);
                //if (dt.Rows.Count <= 0)
                //{
                //    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "网络错误，请稍后再试！").DicInfo());
                //    return;                 
                //}

                string strMessage =
                    string.Format("【{0}】您本次获取的随机验证码是：{1}。如非本人使用，敬请忽略本信息。", Convert.ToString(dt1.Rows[0]["fullname"]), Convert.ToString(dt0.Rows[0]["authcode"]));
                
                //发送至短信机
                ServiceSendMessage.mobileSoapClient send = new ServiceSendMessage.mobileSoapClient();
                bool isSuccess = send.sendmessage(strMobile, strMessage, "短信系统", "短信系统");
                if (isSuccess)
                {
                    //保存动态验证码（MobileCenter）
                    string strDynamicIntervalTime = ConfigTool.GetWebConfigKey("DynamicIntervalTime");
                    DateTime dynamicBeginTime = DateTime.Now;
                    DateTime dynamicEndTime = dynamicBeginTime.AddSeconds(Convert.ToDouble(strDynamicIntervalTime));
                    strSql =
                        string.Format(@"insert into TB_APP_MOBILE_AUTHCODE (mobile,dynamic_authcode,dynamic_begintime,dynamic_endtime) 
                                        values({0},'{1}',to_date('{2}','YYYY-MM-DD HH24:MI:SS'),to_date('{3}','YYYY-MM-DD HH24:MI:SS'))",
                                        strMobile, Identity.RijndaelEncode(Convert.ToString(dt0.Rows[0]["authcode"])), dynamicBeginTime, dynamicEndTime);              
                    new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteNonQuery(strSql);
                    Json = JsonConvert.SerializeObject(new DicPackage(true, null, null).DicInfo());
                }
                else       
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "线路问题，请直接输入验证码六个1").DicInfo());
                }              
                
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取手机验证码数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

        #region 校验手机号
        /// <summary>
        /// 校验手机号
        /// </summary>
        /// <param name="strMobile">手机号</param>
        /// <returns></returns>
        private string VerifyMobile(string strMobile)
        {
            //判断是否已注册
            string strJson = string.Empty;
            string sql =
                string.Format("select * from TB_SYS_USERINFO  where mobile='{0}'", strMobile);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(sql);
            if (dt.Rows.Count >= 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "手机号已注册！").DicInfo());
                return strJson;
            }

            //手机号验证
            string strMessage = TokenTool.VerifyMobile(strMobile);
            if (strMessage != "ture")
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, strMessage).DicInfo());
                return strJson;
            }

            return strJson;
        }
        #endregion  
    }
}
