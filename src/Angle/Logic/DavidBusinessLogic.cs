using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Drawing;
using Angle.Models;

namespace Angle.Logic
{
    public class DavidBusinessLogic
    {
        public HighChartOptions GetHighchart(HighchartTypeEnum type)
        {
            HighChartOptions currentChart = new HighChartOptions();
            switch (type)
            {
                case HighchartTypeEnum.混合型:
                    {
                        #region 混合型

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv",
                                type = ChartTypeEnum.area.ToString()
                            },
                            title = new Title() { text = "网站流量图" },
                            xAxis = new List<XAxis>() {
                    new XAxis(){
                        categories = new List<string>() { "一月", "二月", "三月", "四月", "五月" },
                        reversed = false,
                        opposite = false
                    }
                },
                            yAxis = new YAxis() { title = new Title() { text = "独立访问数" } },
                            tooltip = new ToolTip() { crosshairs = new List<bool>() { true, false } },
                            series = new List<Series>() {
                    new Series(){
                        name = "网易",
                        data = new List<object>() {
                            new object[2] { "中国", 511 },
                            new object[2] { "美国", 114 },
                            new object[2] { "英国", 345 },
                            new { name = "韩国", y = 622, sliced = true, selected = true },
                            new object[2] { "日本", 411 }
                        },
                        type=ChartTypeEnum.pie.ToString(),
                        showInLegend=true,
                        size=100,
                        center=new int[2]{80,30},
                        allowPointSelect=true
                    },
                    new Series { name = "新浪", data = new List<object> { 11, 13, 5, 6, 4 }, type= ChartTypeEnum.column.ToString(),allowPointSelect=false, color="#CC6600" },
                    new Series { name = "腾讯", data = new List<object> { 12, 8, 9, 2, 6 }, type=ChartTypeEnum.spline.ToString(), color="#33CCFF" },
                    new Series { name = "网易", data = new List<object> { 8, 7, 3, 2, 3 }, type= ChartTypeEnum.spline.ToString(),allowPointSelect=false, color= "#0088A8" }
                }
                        };

                        #endregion
                    };
                    break;
                case HighchartTypeEnum.饼图型:
                    {
                        #region 饼图型

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv",
                                type = ChartTypeEnum.pie.ToString()
                            },
                            title = new Title() { text = "地域流量图" },
                            yAxis = new YAxis() { title = new Title() { text = "独立访问数" } },
                            tooltip = new ToolTip() { pointFormat = "{series.name}: <b>{point.percentage}%</b><br/>{series.name}:{point.y}", percentageDecimals = 2 },
                            series = new List<Series>() {
                    new Series(){
                        name="地域",
                        data = new List<object>() {
                            new object[2] { "中国", 511 },
                            new object[2] { "美国", 114 },
                            new object[2] { "英国", 345 },
                            new object[2] { "韩国", 622 },
                            new { name = "韩国", y = 622, sliced = true, selected = true },
                            new object[2] { "日本", 411 }
                        },
                        showInLegend=false,
                        size=270,
                        center=new int[2]{700,150},
                        allowPointSelect=true
                    }
                }
                        };

                        #endregion
                    };
                    break;
                case HighchartTypeEnum.柱状图:
                    {
                        #region 柱线图

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv"
                            },
                            title = new Title() { text = "网站流量图" },
                            xAxis = new List<XAxis>() {
                    new XAxis(){
                        categories = new List<string>() { "一月", "二月", "三月", "四月", "五月" },
                        reversed = false,
                        opposite = false
                    }
                },
                            yAxis = new YAxis() { title = new Title() { text = "独立访问数" } },
                            tooltip = new ToolTip() { crosshairs = new List<bool>() { true, false } },
                            series = new List<Series>() {
                    new Series { name = "新浪", data = new List<object> { 11, 13, 5, 6, 4 }, type= ChartTypeEnum.column.ToString(),allowPointSelect=false, color="#CC6600" },
                    new Series { name = "腾讯", data = new List<object> { 12, 8, 9, 2, 6 }, type=ChartTypeEnum.spline.ToString(), color="#33CCFF" },
                    new Series { name = "网易", data = new List<object> { 8, 7, 3, 2, 3 }, type= ChartTypeEnum.spline.ToString(),allowPointSelect=false, color= "#0088A8" }
                }
                        };

                        #endregion
                    };
                    break;
                case HighchartTypeEnum.多柱状图:
                    {
                        #region 多柱型图

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv"
                            },
                            title = new Title() { text = "网站流量图" },
                            xAxis = new List<XAxis>() {
                    new XAxis(){
                        categories = new List<string>() { "一月", "二月", "三月", "四月", "五月" },
                        reversed = false,
                        opposite = false
                    }
                },
                            yAxis = new YAxis() { title = new Title() { text = "独立访问数" } },
                            tooltip = new ToolTip() { crosshairs = new List<bool>() { true, false } },
                            series = new List<Series>() {
                    new Series { name = "新浪", data = new List<object> { 11, 13, 5, 6, 4 }, type= ChartTypeEnum.column.ToString(),allowPointSelect=false, color="#CC6600" },
                    new Series { name = "腾讯", data = new List<object> { 12, 8, 9, 2, 6 }, type=ChartTypeEnum.column.ToString(), color="#33CCFF" },
                    new Series { name = "网易", data = new List<object> { 8, 7, 3, 2, 3 }, type= ChartTypeEnum.column.ToString(),allowPointSelect=false, color= "#0088A8" }
                }
                        };

                        #endregion
                    };
                    break;
                case HighchartTypeEnum.多流线图:
                    {
                        #region 多流线型

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv"
                            },
                            title = new Title() { text = "网站流量图" },
                            xAxis = new List<XAxis>() {
                    new XAxis(){
                        categories = new List<string>() { "一月", "二月", "三月", "四月", "五月" },
                        reversed = false,
                        opposite = false
                    }
                },
                            yAxis = new YAxis() { title = new Title() { text = "独立访问数" } },
                            tooltip = new ToolTip() { crosshairs = new List<bool>() { true, false }, shared = true },
                            series = new List<Series>() {
                    new Series { name = "新浪", data = new List<object> { 11, 13, 5, 6, 4 }, type= ChartTypeEnum.spline.ToString(),allowPointSelect=false, color="#CC6600" },
                    new Series { name = "腾讯", data = new List<object> { 12, 8, 9, 2, 6 }, type=ChartTypeEnum.spline.ToString(), color="#33CCFF" },
                    new Series { name = "网易", data = new List<object> { 8, 7, 3, 2, 3 }, type= ChartTypeEnum.spline.ToString(),allowPointSelect=false, color= "#0088A8" }
                }
                        };

                        #endregion
                    };
                    break;
                case HighchartTypeEnum.多横柱图:
                    {
                        #region 多横柱型

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv",
                                type = ChartTypeEnum.bar.ToString()
                            },
                            title = new Title() { text = "网站流量图" },
                            xAxis = new List<XAxis>() {
                    new XAxis(){
                        categories = new List<string>() { "一月", "二月", "三月", "四月", "五月" },
                        reversed = false,
                        opposite = false
                    }
                },
                            yAxis = new YAxis() { title = new Title() { text = "独立访问数" } },
                            tooltip = new ToolTip() { crosshairs = new List<bool>() { true, false } },
                            series = new List<Series>() {
                    new Series { name = "新浪", data = new List<object> { 11, 13, 5, 6, 4 }, allowPointSelect=false, color="#CC6600" },
                    new Series { name = "腾讯", data = new List<object> { 12, 8, 9, 2, 6 }, color="#33CCFF" },
                    new Series { name = "网易", data = new List<object> { 8, 7, 3, 2, 3 }, allowPointSelect=false, color= "#0088A8" }
                }
                        };

                        #endregion
                    };
                    break;
                case HighchartTypeEnum.层叠图:
                    {
                        #region 层叠型

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv",
                                type = ChartTypeEnum.column.ToString(),
                                style = "width:100%; heigh:400px;"
                            },
                            title = new Title() { text = "网站流量图" },
                            xAxis = new List<XAxis>() {
                    new XAxis(){
                        categories = new List<string>() { "一月", "二月", "三月", "四月", "五月" },
                        reversed = false,
                        opposite = false
                    }
                },
                            yAxis = new YAxis() { title = new Title() { text = "独立访问数" } },
                            tooltip = new ToolTip() { crosshairs = new List<bool>() { true, false } },
                            plotOptions = new PlotOptions() { stacking = StackingTypeEnum.normal.ToString() },
                            series = new List<Series>() {
                    new Series { name = "新浪", data = new List<object> { 11, 13, 5, 6, 4 }, allowPointSelect=false, color="#CC6600" },
                    new Series { name = "腾讯", data = new List<object> { 12, 8, 9, 2, 6 }, color="#33CCFF" },
                    new Series { name = "网易", data = new List<object> { 8, 7, 3, 2, 3 }, allowPointSelect=false, color= "#0088A8" }
                }
                        };

                        #endregion
                    };
                    break;
                case HighchartTypeEnum.区域图:
                    {
                        #region 区域型

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv",
                                type = ChartTypeEnum.areaspline.ToString()
                            },
                            title = new Title() { text = "网站流量图" },
                            xAxis = new List<XAxis>() {
                    new XAxis(){
                        categories = new List<string>() { "一月", "二月", "三月", "四月", "五月" },
                        reversed = false,
                        opposite = false
                    }
                },
                            yAxis = new YAxis() { title = new Title() { text = "独立访问数" } },
                            tooltip = new ToolTip() { crosshairs = new List<bool>() { true, false } },
                            series = new List<Series>() {
                    new Series { name = "新浪", data = new List<object> { 11, 13, 5, 6, 4 }, allowPointSelect=false, color="#CC6600" },
                    new Series { name = "腾讯", data = new List<object> { 12, 8, 9, 2, 6 }, color="#33CCFF" },
                    new Series { name = "网易", data = new List<object> { 8, 7, 3, 2, 3 }, allowPointSelect=false, color= "#0088A8" }
                }
                        };

                        #endregion
                    };
                    break;
                case HighchartTypeEnum.温度计型:
                    {
                        #region 温度计型

                        currentChart = new HighChartOptions()
                        {
                            chart = new Chart()
                            {
                                renderTo = "highChartDiv",
                                type = ChartTypeEnum.spline.ToString(),
                                inverted = true
                            },
                            title = new Title() { text = "Atmosphere Temperature by Altitude" },
                            subtitle = new SubTitle() { text = "According to the Standard Atmosphere Model" },
                            xAxis = new List<XAxis>(){
                    new XAxis(){reversed=false, title=new Title(){text="Altitude"}, categories=null}
                },
                            yAxis = new YAxis() { title = new Title() { text = "Temperature" }, max = 20, min = -80 },
                            tooltip = new ToolTip() { headerFormat = "<b>{series.name}</b><br/>", pointFormat = "{point.x} km: {point.y}°C", percentageDecimals = 2 },
                            series = new List<Series>() {
                    new Series(){
                        name="Temperature",
                        size=null,
                        showInLegend=false,
                        data = new List<object>() {
                            new object[2] { 0, 15 },
                            new object[2] { 10, -50 },
                            new object[2] { 20, -56.5 },
                            new object[2] { 30, -46.5 },
                            new object[2] { 40, -22.1 },
                            new object[2] { 50, -2.5 },
                            new object[2] { 60, -27.7 },
                            new object[2] { 70, -55.7 },
                            new object[2] { 80, -76.5 }
                        }
                    }
                }
                        };

                        #endregion
                    };
                    break;
                default:
                    break;
            }

            return currentChart;
        }

     

    }
}