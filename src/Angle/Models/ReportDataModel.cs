using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Angle.Models
{
    public class ReportDataModel
    {
        public int ReportId { get; set; }
        public int Clicks { get; set; }
        public int Impressions { get; set; }
        public DateTime ReportDate { get; set; }
        public  string ReportName { get; set; }
        public int UV { get; set; }

        public object ReportWeek { get; set; }
        public object ReportYear { get; set; }

    }
}