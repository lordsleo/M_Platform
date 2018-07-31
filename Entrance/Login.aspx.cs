//
//文件名：    Login.aspx.cs
//功能描述：  用户登陆
//创建时间：  2016/03/05
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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //账号（手机号或Iport账号）
            string strAccount = Request.Form["Account"];
            //密码
            string strPassword = Request.Form["Password"];

            ////账号（手机号或Iport账号）
            //string strAccount = Request.Params["Account"];
            ////密码
            //string strPassword = Request.Params["Password"];

            try
            {
                if (strAccount == null || strPassword == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，登陆失败！").DicInfo());
                    return;
                }

                //手机号登录&&Iport账号登录
                if (TokenTool.VerifyMobile(strAccount) == "ture")
                {
                    Json = GetInfoByMobileLogin(strAccount, strPassword);
                }//手机号登陆
                else
                {
                    Json = GetInfoByUserNameLogin(strAccount, strPassword);
                }//Iport账号登录
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：用户登陆数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

        #region 获取通过手机号登陆
        /// <summary>
        /// 获取通过手机号登陆
        /// </summary>
        /// <param name="account"手机号></param>
        /// <param name="password">密码</param>
        /// <returns>返回结果</returns>
        private string GetInfoByMobileLogin(string strAccount, string strPassword)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            string Json = string.Empty;

            string sql =
                string.Format(@"select b.code_user,a.password 
                                from tb_sys_user a, tb_sys_userinfo b 
                                where a.code_user=b.code_user and b.mobile='{0}'", 
                                strAccount);
            //判断用户（手机号）是否存在
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(sql);
            if (dt.Rows.Count <= 0)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "用户名或密码错误！").DicInfo());
                return Json;
            }

            if (!Identity.VerifyText(Format.Trim(strPassword), dt.Rows[0]["password"] as string))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "用户名或密码错误！").DicInfo());
                return Json;
            }

            Json = JsonConvert.SerializeObject(new DicPackage(true, Convert.ToString(dt.Rows[0]["code_user"]), null).DicInfo());
            return Json;
        }
        #endregion

        #region 获取通过用户名(Iport账号)登陆
        /// <summary>
        /// 获取通过用户名(Iport账号)登陆
        /// </summary>
        /// <param name="strAccount">用户名(Iport账号)</param>
        /// <param name="strPassword">密码</param>
        /// <returns>返回结果</returns>
        private string GetInfoByUserNameLogin(string strAccount, string strPassword)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();
            string Json = string.Empty;

            string sql =
                string.Format(@"select code_user,password 
                                from tb_sys_user 
                                where upper(logogram)='{0}'",
                                strAccount.ToUpper());
            //判断用户（Iport账号）是否存在
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(sql);
            if (dt0.Rows.Count == 0)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "用户名或密码错误！").DicInfo());
                return Json;
            }

            if (!Identity.VerifyText(Format.Trim(strPassword), dt0.Rows[0]["password"] as string))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "用户名或密码错误！").DicInfo());
                return Json;
            }

//            sql =
//                string.Format(@"select mobile 
//                                from tb_sys_userinfo 
//                                where code_user='{0}'",
//                                Convert.ToString(dt0.Rows[0]["code_user"]));
//            //判断用户（Iport账号）是否存在
//            var dt1 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(sql);
//            if ((dt1.Rows.Count == 0) || string.IsNullOrWhiteSpace(Convert.ToString(dt1.Rows[0]["mobile"])))
//            {
//                Json = JsonConvert.SerializeObject(new DicPackage(false, Convert.ToString(dt0.Rows[0]["code_user"]), "此账号未绑定手机号！").DicInfo());
//            }
//            else
//            {
//                Json = JsonConvert.SerializeObject(new DicPackage(true, Convert.ToString(dt0.Rows[0]["code_user"]), "登陆成功！").DicInfo());
//            }

            Json = JsonConvert.SerializeObject(new DicPackage(true, Convert.ToString(dt0.Rows[0]["code_user"]), null).DicInfo());           
            return Json;
        }
        #endregion
    }
}