//
//文件名：    IRegister.aspx.cs
//功能描述：  注册接口
//创建时间：  2016/03/18
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Leo;
using ServiceInterface.Common;

namespace M_Platform.Entrance
{
    public class IRegister
    {
        /// <summary>
        /// 注册数据集
        /// </summary>
        private RegisterE registerE;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="registerE"></param>
        public IRegister(RegisterE registerE)
        {
            this.registerE = registerE;
        }

        #region 注册Iport用户
        public string RegisterIportUser()
        {
            string strJson = string.Empty;
            string strXmlParams =
                     string.Format("<params><loginname>{0}</loginname><password>{1}</password><username>{2}</username><companycode>{3}</companycode><departmentcode>{4}</departmentcode><usertypecode>{5}</usertypecode><workno>{6}</workno><mobile>{7}</mobile><phone>{8}</phone><qq>{9}</qq><email>{10}</email></params>",
                                     registerE.strLogogram, registerE.strPassword1, registerE.strUserName, registerE.strCompanyCode, registerE.strDepartmentCode, registerE.strUserTypeCode, registerE.strWorkNo, registerE.strMobile, registerE.strPhone, registerE.strQQ, registerE.strEmail);

            //校验手机号
            strJson = VerifyMobile(registerE.strMobile);
            if (!string.IsNullOrWhiteSpace(strJson))
            {
                return strJson;
            }

            //校验密码
            if (registerE.strPassword1 != registerE.strPassword2)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "密码不一致！").DicInfo());
                return strJson;
            }

            ServiceIportUser.WebServiceUserSoapClient service = new ServiceIportUser.WebServiceUserSoapClient();
            string strReturnMessage = service.Add(strXmlParams, 1);
            RetureMessage retureMessage = JsonConvert.DeserializeObject<RetureMessage>(strReturnMessage);

            if ((bool)retureMessage.Value == false)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, retureMessage.Message).DicInfo());
            }
            else
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(true, null, null).DicInfo());
            }

            return strJson;
        }
        #endregion

        #region 校验手机号
        /// <summary>
        /// 校验手机号
        /// </summary>
        /// <param name="strMobile"></param>
        /// <returns></returns>
        private string VerifyMobile(string strMobile)
        {
            string strJson = string.Empty;

            //判断是否已注册
            string sql =
                string.Format("select * from TB_SYS_USERINFO  where mobile='{0}'", strMobile);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathIport).ExecuteTable(sql);
            if (dt.Rows.Count != 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "手机号已注册！").DicInfo());
                return strJson;
            }

            ////手机号验证
            //string strMessage = TokenTool.VerifyMobile(strMobile);
            //if (strMessage != "ture")
            //{
            //    strJson = JsonConvert.SerializeObject(new DicPackage(false, null, strMessage).DicInfo());
            //    return strJson;
            //}

            return strJson;
        }
        #endregion
    }

    public struct RetureMessage
    {
        //是否成功
        public bool Success { get; set; }
        //消息
        public string Message { get; set; }
        //值
        public object Value { get; set; }
    }
}




