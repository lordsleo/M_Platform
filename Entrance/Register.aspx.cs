//
//文件名：    Register.aspx.cs
//功能描述：  用户注册（Iport）
//创建时间：  2016/03/18
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
    public partial class Register : System.Web.UI.Page
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
            //速记码（Iport登陆账户）
            string strLogogram = Request.Form["Logogram"];
            //密码1
            string strPassword1 = Request.Form["Password1"];
            //密码2
            string strPassword2 = Request.Form["Password2"];
            //用户名称
            string strUserName = Request.Form["UserName"];
            //公司编码
            string strCompanyCode = Request.Form["CompanyCode"];
            //部门编码
            string strDepartmentCode = Request.Form["DepartmentCode"];
            //用户类型编码
            string strUserTypeCode = Request.Form["UserTypeCode"];
            //员工工号
            string strWorkNo = Request.Form["WorkNo"];
            //电话
            string strPhone = Request.Form["Phone"];
            //QQ
            string strQQ = Request.Form["QQ"];
            //邮箱
            string strEmail = Request.Form["Email"];

            ////手机号
            //string strMobile = Request.Params["Mobile"];
            ////速记码（Iport登陆账户）
            //string strLogogram = Request.Params["Logogram"];
            ////密码1
            //string strPassword1 = Request.Params["Password1"];
            ////密码2
            //string strPassword2 = Request.Params["Password2"];
            ////用户名称
            //string strUserName = Request.Params["UserName"];
            ////公司编码
            //string strCompanyCode = Request.Params["CompanyCode"];
            ////部门编码
            //string strDepartmentCode = Request.Params["DepartmentCode"];
            ////用户类型编码
            //string strUserTypeCode = Request.Params["UserTypeCode"];
            ////员工工号
            //string strWorkNo = Request.Params["WorkNo"];
            ////电话
            //string strPhone = Request.Params["Phone"];
            ////QQ
            //string strQQ = Request.Params["QQ"];
            ////邮箱
            //string strEmail = Request.Params["Email"];

            strMobile = Format.Trim(strMobile);
            strPassword1 = Format.Trim(strPassword1);
            strPassword2 = Format.Trim(strPassword2);
            strLogogram = Format.Trim(strLogogram);
            strUserName = Format.Trim(strUserName);
            strPhone = Format.Trim(strPhone);
            strQQ = Format.Trim(strQQ);
            strEmail = Format.Trim(strEmail);

            //strMobile = "18036600293";
            //strPassword1 = "123456";
            //strPassword2 = "123456";

            try
            {
                //校验注册参数
                string[] strParamArray = RegisterParamsTool.GetParams(Request.Params["AppName"]);
                if (strParamArray == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，注册失败！").DicInfo());
                    return;
                }
                for (int iParam = 0; iParam < strParamArray.Length; iParam++)
                {
                    string strParam = "" + strParamArray[iParam] + "";
                    if (Request.Params[strParam] == null)
                    {
                        Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，注册失败！").DicInfo());
                        return;
                    }
                }

                //注册数据集
                RegisterE registerE = new RegisterE();
                registerE.strMobile = strMobile;
                registerE.strPassword1 = strPassword1;
                registerE.strPassword2 = strPassword2;
                if (string.IsNullOrWhiteSpace(strLogogram))
                {
                    //生成用户速记码
                    string strSql =
                        string.Format("select sq_user_logogram.nextval as num from dual");
                    var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(strSql);
                    strLogogram = "IPORT" + Convert.ToString(dt.Rows[0]["num"]);
                }
                registerE.strLogogram = strLogogram;
                if (string.IsNullOrWhiteSpace(strUserName))
                {
                    registerE.strUserName = strLogogram;
                }
                else
                {
                    registerE.strUserName = strUserName;
                }
                if (!string.IsNullOrWhiteSpace(strCompanyCode))
                {
                    registerE.strCompanyCode = strCompanyCode;
                }
                if (!string.IsNullOrWhiteSpace(strDepartmentCode))
                {
                    registerE.strDepartmentCode = strDepartmentCode;
                }
                if (!string.IsNullOrWhiteSpace(strUserTypeCode))
                {
                    registerE.strUserTypeCode = strUserTypeCode;
                }
                if (!string.IsNullOrWhiteSpace(strWorkNo))
                {
                    registerE.strWorkNo = strWorkNo;
                }
                if (!string.IsNullOrWhiteSpace(strPhone))
                {
                    registerE.strPhone = strPhone;
                }
                if (!string.IsNullOrWhiteSpace(strQQ))
                {
                    registerE.strQQ = strQQ;
                }
                if (!string.IsNullOrWhiteSpace(strEmail))
                {
                    registerE.strEmail = strEmail;
                }

                //注册Iport
                IRegister iRegister = new IRegister(registerE);
                Json = iRegister.RegisterIportUser();
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：用户注册数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;

    }
}