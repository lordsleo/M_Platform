//
//文件名：    AddDevice.aspx.cs
//功能描述：  添加移动设备
//创建时间：  2015/10/19
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

namespace M_Platform.Device
{
    public partial class AddDevice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //设备ID
            string strDeviceToken = Request.Params["DeviceToken"];
            //设备类型
            string strDeviceType = Request.Params["DeviceType"];
            //应用名称
            string strAppName = Request.Params["AppName"];
            //用户编码
            string strCodeUser = Request.Params["CodeUser"];
            //移动电话
            string strMobile = Request.Params["Mobile"];

            try
            {
                if (strDeviceToken == null || strDeviceType == null || strAppName == null || strCodeUser == null || strMobile == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，添加移动设备失败！").DicInfo());
                    return;
                }

                if (string.IsNullOrWhiteSpace(strDeviceToken) || string.IsNullOrWhiteSpace(strDeviceType) || string.IsNullOrWhiteSpace(strAppName))
                {
                    //string warning = "参数DeviceToken，DeviceType，AppName不能为空！";
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，添加移动设备失败！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format("select devicetoken,devicetype,appname from tb_app_device where devicetoken='{0}' and devicetype='{1}' and appname='{2}'", strDeviceToken, strDeviceType, strAppName);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {//插入
                    strSql =
                        string.Format(@"insert into tb_app_device (mobile,devicetoken,devicetype,appname,code_user)                
                                        values ('{0}','{1}','{2}','{3}','{4}')",
                                        strMobile, strDeviceToken, strDeviceType, strAppName, strCodeUser);
                }
                else
                {//更新       
                    strSql =
                        string.Format(@"update tb_app_device set code_user='{0}',mobile='{1}',modifytime=to_date('{2}','YYYY-MM-DD HH24:MI:SS') 
                                        where devicetoken='{3}' and devicetype='{4}' and appname='{5}'",
                                        strCodeUser, strMobile, DateTime.Now, strDeviceToken, strDeviceType, strAppName);
                }

                new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteNonQuery(strSql);
                Json = JsonConvert.SerializeObject(new DicPackage(true, null, "添加设备成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：添加移动设备数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}