//
//文件名：    GetTodayDealList.aspx.cs
//功能描述：  获取今日待办列表
//创建时间：  2015/09/18
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

namespace M_Platform.OA
{
    public partial class GetTodayDealList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //速记码（用户名）
            string strLogogram = Request.Params["Logogram"];

            try
            {
                if (strLogogram == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，获取今日待办列表失败！").DicInfo());
                    return;

                }

                DateTime curTime = DateTime.Now;
                string strStartTime = curTime.ToShortDateString() + " 00:00:00";

                string strSql =
                        string.Format(@"select title 
                                        from awsprod.wf_task_log 
                                        where owner='{0}' and endtime >= to_date('{1}', 'yyyy-MM-dd hh24:mi:ss')",
                                        strLogogram, strStartTime);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无今日已办！").DicInfo());
                    return;
                }

                string[] strArray = new string[dt.Rows.Count];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow] = Convert.ToString(dt.Rows[iRow]["title"]);
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取今日待办列表数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}