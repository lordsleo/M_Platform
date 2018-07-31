//
//文件名：    RegisterE.aspx.cs
//功能描述：  注册数据集
//创建时间：  2015/10/24
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace M_Platform.Entrance
{
    public class RegisterE
    {
        //手机号
        public string strMobile { get;  set; }
        //速记码（Iport登陆账户）
        public string strLogogram { get; set; }
        //密码1
        public string strPassword1 { get; set; }
        //密码2
        public string strPassword2 { get; set; }
        //用户名称
        public string strUserName { get; set; }
        //公司编码
        public string strCompanyCode { get; set; }
        //部门编码
        public string strDepartmentCode { get; set; }
        //用户类型编码
        public string strUserTypeCode { get; set; }
        //员工工号
        public string strWorkNo { get; set; }
        //电话
        public string strPhone { get; set; }
        //QQ
        public string strQQ { get; set; }
        //邮箱
        public string strEmail { get; set; }


        #region 公共方法
        #region 初始化注册数据
        /// <summary>
        /// 初始化注册数据
        /// </summary>
        public RegisterE()
        {
            strMobile = null;
            strLogogram = null;
            strPassword1 = null;
            strPassword2 = null;
            strUserName = null;
            strCompanyCode = "2D1E299478C00352E053A86401690352";  //司机，虚拟公司组织
            strDepartmentCode = "2D1E1C26F470B138E053A8640169B138"; //司机， 虚拟部门组织
            strUserTypeCode = "2";   //默认值
            strWorkNo = null;
            strPhone = null;
            strQQ = null;
            strEmail = null;
        }
        #endregion     
        #endregion
    }
}

