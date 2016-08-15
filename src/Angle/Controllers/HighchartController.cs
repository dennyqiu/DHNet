using Angle.Logic;
using Angle.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Angle.Helpers;

namespace Angle.Controllers
{
    public class HighchartController : Controller
    {
        private DavidBusinessLogic logic = new DavidBusinessLogic();

        // GET: Highchart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()

        {
            return View();
        }
        public ActionResult Index3()

        {
            return View();
        }
        public JsonResult GetHighChartOptions()
        {
            //int chartType = Request.Form["type"] == null ? (int)HighchartTypeEnum.混合型 : Convert.ToInt32(Request.Form["type"].ToString());
            string chartType = Request.Form["type"].ToString();
            HighchartTypeEnum type = (HighchartTypeEnum)Enum.Parse(typeof(HighchartTypeEnum), chartType.ToString());
            HighChartOptions chart = logic.GetHighchart(type);
            return Json(new { value = chart, label = type.ToString() }, JsonRequestBehavior.AllowGet);
        }
       

        /// <summary>
        /// 获取数据图表的数据源-直接是个List<TMode>的集合队列 后台测试数据源
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataChartSource()
        {
            int queryId = Request.Form["filterValue"] == null ? 0 : Convert.ToInt32(Request.Form["filterValue"]);
            int dimensionType = Request.Form["dimensionType"] == null ? 0 : Convert.ToInt32(Request.Form["dimensionType"]);
            //int dimensionType = Convert.ToInt32(Request.Form["dimensionType"]);
            TimeTypeEnum type = (TimeTypeEnum)Enum.Parse(typeof(TimeTypeEnum), dimensionType.ToString());

            IList<ReportDataModel> reportDataLs = new List<ReportDataModel>() {
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,1,1), Impressions=10000, Clicks=5513, UV=45241},
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,2,11), Impressions=23121, Clicks=5111, UV=53532},
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,3,12), Impressions=76511, Clicks=4522, UV=34234},
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,4,13), Impressions=96511, Clicks=7362, UV=41133},
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,4,14), Impressions=42231, Clicks=4224, UV=42612},
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,4,15), Impressions=34244, Clicks=6242, UV=51311},
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,5,16), Impressions=86511, Clicks=3424, UV=84322},
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,10,17), Impressions=31311, Clicks=6234, UV=77342},
                new ReportDataModel(){ ReportId=1, ReportName="David测试订单", ReportDate=new DateTime(2013,12,18), Impressions=23131, Clicks=7242, UV=61111},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,4,19), Impressions=41311, Clicks=3244, UV=72421},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,4,10), Impressions=10000, Clicks=5513, UV=45241},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,4,11), Impressions=23121, Clicks=5111, UV=53532},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,4,12), Impressions=34232, Clicks=4522, UV=34234},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,4,13), Impressions=96511, Clicks=7362, UV=41133},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,4,14), Impressions=96511, Clicks=4224, UV=42612},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,4,15), Impressions=34244, Clicks=6242, UV=51311},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,4,16), Impressions=96511, Clicks=3424, UV=84322},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,5,17), Impressions=31311, Clicks=6234, UV=77342},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,10,18), Impressions=96511, Clicks=7242, UV=61111},
                new ReportDataModel(){ ReportId=2, ReportName="David测试订单2", ReportDate=new DateTime(2013,12,19), Impressions=41311, Clicks=3244, UV=72421},
            };

            if (queryId > 0)
                reportDataLs = reportDataLs.Where(n => n.ReportId == queryId).ToList<ReportDataModel>();

            switch (type)
            {
                case TimeTypeEnum.Day:
                    {
                        var result = from item in reportDataLs
                                     group item by DateHelper.FormatDate(item.ReportDate,"2")into reportDataDay
                                     select new
                                     {
                                         ReportDayStr = reportDataDay.Key,
                                         Impressions = reportDataDay.Sum(n => n.Impressions),
                                         Clicks = reportDataDay.Sum(n => n.Clicks),
                                         CTR = reportDataDay.Sum(n => n.Clicks) == 0 ? 0 : Math.Round((double)reportDataDay.Sum(n => n.Clicks) / reportDataDay.Sum(n => n.Impressions), 6),
                                         UV = reportDataDay.Max(n => n.UV)
                                     };
                        return Json(new { chartSource = result }, JsonRequestBehavior.AllowGet);
                    };
                case TimeTypeEnum.Week:
                    {
                        var result = from item in reportDataLs
                                         //group item by DateHelper.WeekOfCurrent(item.ReportDate, false) into reportDataWeek
                                     group item by DateHelper.GetIsoWeek(item.ReportDate) into reportDataWeek
                                     select new
                                     {
                                         ReportWeekStr = reportDataWeek.Key,
                                         Impressions = reportDataWeek.Sum(n => n.Impressions),
                                         Clicks = reportDataWeek.Sum(n => n.Clicks),
                                         CTR = reportDataWeek.Sum(n => n.Clicks) == 0 ? 0 : Math.Round((double)reportDataWeek.Sum(n => n.Clicks) / reportDataWeek.Sum(n => n.Impressions), 6),
                                         UV = reportDataWeek.Max(n => n.UV)
                                     };
                        return Json(new { chartSource = result }, JsonRequestBehavior.AllowGet);
                    };
                case TimeTypeEnum.Month:
                    {
                        var result = from item in reportDataLs
                                     group item by item.ReportDate.Month into reportDataYear
                                     select new
                                     {
                                         ReportMonthStr = reportDataYear.Key,
                                         Impressions = reportDataYear.Sum(n => n.Impressions),
                                         Clicks = reportDataYear.Sum(n => n.Clicks),
                                         CTR = reportDataYear.Sum(n => n.Clicks) == 0 ? 0 : Math.Round((double)reportDataYear.Sum(n => n.Clicks) / reportDataYear.Sum(n => n.Impressions), 6),
                                         UV = reportDataYear.Max(n => n.UV)
                                     };
                        return Json(new { chartSource = result }, JsonRequestBehavior.AllowGet);
                    };
                default:
                    {
                        var result = reportDataLs;
                        return Json(new { chartSource = reportDataLs }, JsonRequestBehavior.AllowGet);
                    }
            }
        }

        public ActionResult GetData(string hours, string nums)
        {
            int Nums = Convert.ToInt16(nums);
            string strConn = @"Provider = Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source =" + "d:\\sample.xls";//当然你可以通过传值来设置数据源
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            System.Data.DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            conn.Close();
            List<string> tblName = new List<string>();
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                tblName.Add(dt.Rows[i]["TABLE_NAME"].ToString());
            }
            string strCom;
            DataSet ds = new DataSet();
            OleDbCommand cmd = new OleDbCommand();
            for (int i = 0; i < Nums; i++)
            {
                strCom = " SELECT top " + hours + " * FROM [" + tblName[i] + "];";
                cmd.Connection = conn;
                cmd.CommandText = strCom;
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                conn.Open();
                adapter.Fill(ds, "table");
                conn.Close();
            }
            string data = ToJson(ds.Tables[0]);
            return Json(data);
        }
        public string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    if (strKey != "value")
                    {
                        jsonString.Append("\"" + strKey + "\":\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            jsonString.Append(strValue + "\",");
                        }
                        else
                        {
                            jsonString.Append(strValue + "\"");
                        }
                    }
                    else
                    {
                        jsonString.Append("\"" + strKey + "\":");
                        if (j < dt.Columns.Count - 1)
                        {
                            jsonString.Append(strValue + ",");
                        }
                        else
                        {
                            jsonString.Append(strValue + "");
                        }
                    }

                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        public string DateConverter(string Country, DateTime DT)
        {
            //Phone = Phone.Replace(" ", "").Replace(")", ")-");
            switch (Country)
            {
                case "W":
                    return "WW" + DT;
                case "M":
                    return "mm" + DT;
                default:
                    return "d"+DT;
            }
        }
    }
}