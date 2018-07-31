using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ServiceInterface.Common;
using Leo;
using System.Web.Services.Protocols;
using YGSoft.IPort.Data;

namespace M_Platform
{
    /// <summary>
    /// ServiceMobile 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceMobile : System.Web.Services.WebService
    {
        //定义用户身份验证类变量authHeader
        public AuthHeader authHeader = new AuthHeader();

        #region Web服务
        #region 消息推送
        /// <summary>
        /// 消息推送推送
        /// </summary>
        /// <param name="cryptogram">校验码</param>
        /// <param name="receiverId">接收着用户编码</param>
        /// <param name="senderId">发送者用户编码（0表示系统用户）</param>
        /// <param name="appName">应用名称</param>
        /// <param name="title">标题</param>
        /// <param name="content">消息内容</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        [WebMethod]
        [SoapHeader("authHeader")]
        public Package<bool> Push(string receiverId, string senderId, string appName, string title, string message, string param)
        {
            try
            {
                ////验证是否有权访问
                //if (!authHeader.ValideUser(authHeader.UserName, authHeader.PassWord))
                //{
                //    return new Package<bool>(false, "没有访问权限！", false);
                //}

                ////消息推送至手机端
                //string strSql =
                //    string.Format("select * from tb_app_device where code_user='{0}' and appname='{1}'", receiverId, appName);
                //var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteTable(strSql);
                //if (dt.Rows.Count != 0)
                //{
                //    string deviceToken = dt.Rows[0]["devicetoken"].ToString();
                //    string deviceType = dt.Rows[0]["devicetype"].ToString();
                //    switch (deviceType)
                //    {
                //        case "iOS":
                //            {
                //                //IOS测试devicetoken  7a6f8056710252ff8561ed1611b1b82bc398eab45b8882c12fdb143a7fb15e46
                //                string URL = "http://168.100.1.218/test.php";
                //                System.Collections.Specialized.NameValueCollection PostVars = new System.Collections.Specialized.NameValueCollection(); //参数类
                //                message = HttpUtility.UrlEncode(message); //中文UTF8编码转化
                //                PostVars.Add("devicetoken", deviceToken);//参数USERid  
                //                PostVars.Add("message", message);//参数msg
                //                System.Net.WebClient wb = new System.Net.WebClient();
                //                byte[] byRemoteInfo = wb.UploadValues(URL, "POST", PostVars); //HTTP地址，POST请求，参数类
                //                string sRemoteInfo = System.Text.Encoding.Default.GetString(byRemoteInfo);//获取返回值
                //                if (sRemoteInfo == "Connected to APNS\r\nMessage successfully delivered\r\n")
                //                {
                //                    break;
                //                }
                //                else
                //                {
                //                    return new Package<bool>(true, "推送失败。", false);
                //                }
                //            }

                //        case "Android":
                //            {
                //                // 推送服务器地址
                //                const String url = "http://127.0.0.1:8080/androidpn/notification.do?action=send";

                //                // androidpn推送工具
                //                AndroidPn androidPush = new AndroidPn(url);

                //                // 单用户推送
                //                androidPush.PushToSingle(deviceToken, title, message, param);
                //                break;
                //            }
                //        default:
                //            return new Package<bool>(true, "暂不支持iOS,Android以外设备推送！", false);
                //    }
                //}

                ////获取应用名称与推送数据库关键字和推送数据库表名
                //string keyDataBase = DictionaryTool.GetKeyDataBase(appName.ToUpper());
                //string DBTable = DictionaryTool.GetDBTable(appName.ToUpper());
                //if (keyDataBase == string.Empty || DBTable == string.Empty)
                //{
                //    return new Package<bool>(true, "应用名称不存在！", false);
                //}

                //string strSql =
                //    string.Format("insert into {0} (receiverid,senderid,appname,title,message,params) values('{1}','{2}','{3}','{4}','{5}','{6}')", DBTable, receiverId, senderId, appName, title, message, param);
                //var dt = new Leo.Oracle.DataAccess(keyDataBase).ExecuteTable(strSql);

                return new Package<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new Package<bool>(false, string.Format("{0}：推送数据发生异常。{1}", ex.Source, ex.Message), false);
            }
        }

        #endregion

        #region 登陆
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">登陆账号（Iport用户/手机号）</param>
        /// <param name="password">密码</param>
        /// <param name="appName">名称名称</param>
        /// <returns>消息</returns>
        [WebMethod]
        [SoapHeader("authHeader")]
        public Package<string> Login(string account, string password, string appName)
        {
            try
            {
                //验证是否有权访问
                if (!authHeader.ValideUser(authHeader.UserName, authHeader.PassWord))
                {
                    return new Package<string>(false, "没有访问权限！", null);
                }

                //手机登录&&会员名登录（只有Iport用户名首次登陆才会在sph用户表插入用户信息）
                if (TokenTool.VerifyMobile(account) == "ture")
                {
                    return GetInfoByMobileLogin(account, password);
                }//手机号登陆
                else
                {
                    return GetInfoByUserNameLogin(account, password);   
                }
            }
            catch (Exception ex)
            {
                return new Package<string>(false, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message), null);
            }
        }

        #endregion

        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="password1">密码1</param>
        /// <param name="password2">密码2</param>
        /// <param name="appName">应用名称</param>
        /// <returns>消息</returns>
        [WebMethod]
        [SoapHeader("authHeader")]
        public Package<string> Register(string mobile, string password1, string password2, string appName)
        {
            try
            {
                //验证是否有权访问
                //if (!authHeader.ValideUser(authHeader.UserName, authHeader.PassWord))
                //{
                //    return new Package<string>(true, "没有访问权限！", null);
                //}

                //判断是否已注册
                string sql =
                    string.Format("select * from TB_SPH_USER  where account='{0}'", mobile);
                var dt = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
                if (dt.Rows.Count != 0)
                {
                    return new Package<string>(false, "手机号已注册！", null);
                }

                //手机号码验证
                string message = TokenTool.VerifyMobile(mobile);
                if (message != "ture")
                {
                    return new Package<string>(false, message, null);
                }

                //判断密码是的合理
                if (password1 != password2)
                {
                    return new Package<string>(false, "密码不一致！", null);
                }

                //App使用截止时间
                string timeType = "MONTH";
                int timeInterval = 3;
                DateTime endTime;
                switch (timeType)
                {
                    case "DAY":
                        endTime = DateTime.Now.AddDays(timeInterval);
                        break;
                    case "MONTH":
                        endTime = DateTime.Now.AddMonths(timeInterval);
                        break;
                    case "YEAR":
                        endTime = DateTime.Now.AddYears(timeInterval);
                        break;
                    default:
                        throw new Exception("错误的对象索引");
                }

                //注册
                sql =
                    string.Format(@"insert into TB_SPH_USER (account, password, endtime) 
                                    values ('{0}', '{1}',  to_date('{2}','YYYY-MM-DD HH24:MI:SS'))"
                                    , mobile, password1, endTime);
                dt = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
                //检查是否已插入
                sql =
                    string.Format(@"select * 
                                    from TB_SPH_USER 
                                    where account='{0}' and password='{1}' and endtime=to_date('{2}','YYYY-MM-DD HH24:MI:SS')"
                                    , mobile, password1, endTime);
                dt = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    return new Package<string>(false, "网络错误，请稍后再试！", null);
                }
                else
                {

                    if (Convert.ToString(dt.Rows[0]["account"]) != mobile || Convert.ToString(dt.Rows[0]["password"]) != password1 || Convert.ToString(dt.Rows[0]["endtime"]) != endTime.ToString())
                    {
                        return new Package<string>(false, "网络错误，请稍后再试！", null);
                    }
                    else
                    {
                        return new Package<string>(true, "注册成功！", null);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Package<string>(false, string.Format("{0}：获取数据发生异常。{1}", ex.Source, ex.Message), null);
            }
        }
        #endregion



        #endregion




        #region 获取通过手机登陆信息
        /// <summary>
        /// 获取通过手机登陆信息
        /// </summary>
        /// <param name="account"手机号></param>
        /// <param name="password">密码</param>
        /// <returns>返回结果</returns>
        private Package<string> GetInfoByMobileLogin(string account, string password)
        {
            string sql =
                string.Format("select * from TB_SPH_USER where account='{0}'", account);
            //判断用户（手机号）是否存在
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
            if (dt0.Rows.Count == 0)
            {
                return new Package<string>(false, "登陆账号错误！", null);
            }

            //判断是否为Iport用户
            if (Convert.ToString(dt0.Rows[0]["usertype"]) == "1")
            {
                sql =
                    string.Format("select password from TB_SYS_USER  where upper(logogram) = '{0}'", Convert.ToString(dt0.Rows[0]["logogram"]).ToUpper());
                var dt1 = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathIport).ExecuteTable(sql);
                if (dt1.Rows.Count == 0)
                {
                    return new Package<string>(false, "Iport账号可能变动，如有问题请联系客服！", null);
                }
                else
                {
                    if (!Identity.VerifyText(Format.Trim(password), Convert.ToString(dt1.Rows[0]["password"])))
                    {
                        return new Package<string>(false, "密码错误！", null);
                    }
                }
            }//sph用户表显示为Iport用户
            else
            {
                if (password != Convert.ToString(dt0.Rows[0]["password"]))
                {
                     return new Package<string>(false, "密码错误！", null);
                }
            }//sph用户表显示为非Iport用户

            return new Package<string>(true, "登陆成功！", Convert.ToString(dt0.Rows[0]["code_user"]));
        }
        #endregion
        #region 获取通过用户名登陆信息
        /// <summary>
        /// 获取通过用户名登陆信息
        /// </summary>
        /// <param name="account">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回结果</returns>
        private Package<string> GetInfoByUserNameLogin(string account, string password)
        {
            string codeUser = string.Empty;
            string sql =
                string.Format("select * from TB_SPH_USER where upper(logogram)='{0}'", account.ToUpper());
            //判断用户是否存在
            var dt0 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
            if (dt0.Rows.Count == 0)
            {
                sql =
                    string.Format("select code_user,password,code_company from TB_SYS_USER  where upper(logogram) = '{0}'", account.ToUpper());
                var dt1 = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathIport).ExecuteTable(sql);
                if (dt1.Rows.Count == 0)
                {
                    return new Package<string>(false, "登陆账号错误！", null);
                }//Iport用户表不存在
                else
                {
                    if (!Identity.VerifyText(Format.Trim(password), dt1.Rows[0]["password"] as string))
                    {
                        return new Package<string>(false, "密码错误！", null);
                    }

                    string mobile = string.Empty;
                    string codeCompany = string.Empty;
                    string iportCodeUser = string.Empty;
                    //获取Iport用户电话信息
                    sql =
                        string.Format("select mobile from TB_SYS_USERINFO  where code_user = '{0}'", Convert.ToString(dt1.Rows[0]["code_user"]));
                    var dt2 = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathIport).ExecuteTable(sql);
                    if (dt2.Rows.Count != 0)
                    {
                        mobile = Convert.ToString(dt2.Rows[0]["mobile"]);
                    }
                    codeCompany = Convert.ToString(dt1.Rows[0]["code_company"]);
                    iportCodeUser = Convert.ToString(dt1.Rows[0]["code_user"]);
                    


                    //查询此账号（手机号）是否已注册
                    sql =
                        string.Format("select * from TB_SPH_USER where account='{0}'", mobile);
                    //判断用户（手机号）是否存在
                    var dt3 = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
                    if (dt3.Rows.Count != 0)
                    {
                        return new Package<string>(false, "您已用手机号进行注册，可以联系客服！", null);
                    }

                    //App使用截止时间
                    string timeType = "MONTH";
                    int timeInterval = 3;
                    DateTime endTime;
                    switch (timeType)
                    {
                        case "DAY":
                            endTime = DateTime.Now.AddDays(timeInterval);
                            break;
                        case "MONTH":
                            endTime = DateTime.Now.AddMonths(timeInterval);
                            break;
                        case "YEAR":
                            endTime = DateTime.Now.AddYears(timeInterval);
                            break;
                        default:
                            throw new Exception("错误的对象索引");
                    }

                    //sph用户表插入新用户信息
                    sql =
                        string.Format(@"insert into TB_SPH_USER (account, logogram, code_company, endtime, usertype, iport_code_user) 
                                        values('{0}','{1}','{2}',to_date('{3}','YYYY-MM-DD HH24:MI:SS'),'{4}','{5}')"
                                        , mobile, account, codeCompany, endTime, "1", iportCodeUser);
                    var dt4 = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
                    //检查是否已插入
                    sql =
                        string.Format("select * from TB_SPH_USER where upper(logogram)='{0}'", account.ToUpper());
                    var dt5 = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathWlxgx).ExecuteTable(sql);
                    if (dt5.Rows.Count == 0)
                    {
                        return new Package<string>(false, "网络错误，请稍后再试！", null);
                    }
                    else
                    {
                        if (Convert.ToString(dt5.Rows[0]["account"]) != mobile || Convert.ToString(dt5.Rows[0]["code_company"]) != codeCompany || Convert.ToString(dt5.Rows[0]["usertype"]) != "1" || Convert.ToString(dt5.Rows[0]["endtime"]) != endTime.ToString() || Convert.ToString(dt5.Rows[0]["iport_code_user"]) != iportCodeUser)
                        {
                            return new Package<string>(false, "网络错误，请稍后再试！", null);
                        }
                        {
                            codeUser = Convert.ToString(dt5.Rows[0]["code_user"]);
                        }
                    }
                }//Iport用户表存在
            }//sph用户表不存在
            else
            {
                sql =
                    string.Format("select password from TB_SYS_USER  where upper(logogram) = '{0}'", account.ToUpper());
                var dt1 = new Leo.Oracle.DataAccess(Leo.RegistryKey.KeyPathIport).ExecuteTable(sql);
                if (dt1.Rows.Count == 0)
                {
                    return new Package<string>(false, "Iport账号可能变动，请联系客服！", null);
                }
                else
                {
                    if (!Identity.VerifyText(password, Convert.ToString(dt1.Rows[0]["password"])))
                    {
                        return new Package<string>(false, "密码错误！", null);
                    }
                }

                    codeUser = Convert.ToString(dt0.Rows[0]["code_user"]);
                }//sph用户表存在

            return new Package<string>(true, "登陆成功！", codeUser);
        }
        #endregion
    }
}
