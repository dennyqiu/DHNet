using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EChartsDemo.Models;

namespace EChartsDemo.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页--柱状图（使用静态数据）
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 饼图（jq获取动态数据）
        /// </summary>
        /// <returns></returns>
        public ActionResult PieMap()
        {
            return View();
        }


        /// <summary>
        /// 饼图（jq获取动态数据）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PieMap(string id)
        {
            var pie = new PieMapViewModel()
            {
                LegendData = new List<string>()
                {
                    "直接访问",
                    "邮件营销",
                    "联盟广告",
                    "视频广告",
                    "搜索引擎"
                },
                SeriesData = new List<VisitSource>()
                {
                    new VisitSource() {name = "直接访问", value = "335"},
                    new VisitSource() {name = "邮件营销", value = "310"},
                    new VisitSource() {name = "联盟广告", value = "234"},
                    new VisitSource() {name = "视频广告", value = "135"},
                    new VisitSource() {name = "搜索引擎", value = "1548"}
                }
            };

            return Json(new { status = 1, result = pie });
        }

        /// <summary>
        /// 饼图（angularjs）
        /// </summary>
        /// <returns></returns>
        public ActionResult PieMapS()
        {
            return View();
        }

    }
}