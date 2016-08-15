using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EChartsDemo.Models
{
    public class PieMapViewModel
    {
        /// <summary>
        /// 图例数据
        /// </summary>
        public List<string> LegendData { get; set; }

        /// <summary>
        /// 图表数据
        /// </summary>
        public List<VisitSource> SeriesData { get; set; }
    }
}