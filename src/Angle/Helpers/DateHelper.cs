//====================================================================
//** Copyright © 请保留此注释 **
//====================================================================
// 文件名称：DateHelper.cs
// 项目名称：常用方法实用工具集
// 创建时间：2016年7月25日10时59分
// 创建人员：DH
// 负 责 人：DH
// 参与人员：DH
// ===================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Angle.Helpers
{

        /// <summary>
        /// 日期操作辅助类
        /// </summary>
        public static class DateHelper
        {

            #region 当前时间

            /// <summary>
            /// 返回当前时间的标准日期格式
            /// </summary>
            /// <returns>yyyy-MM-dd</returns>
            public static string GetDate()
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
            /// <summary>
            /// 返回当前时间的标准时间格式string
            /// </summary>
            /// <returns>HH:mm:ss</returns>
            public static string GetTime()
            {
                return DateTime.Now.ToString("HH:mm:ss");
            }
            /// <summary>
            /// 返回当前时间的标准时间格式string
            /// </summary>
            /// <returns>yyyy-MM-dd HH:mm:ss</returns>
            public static string GetDateTime()
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            /// <summary>
            /// 返回相对于当前时间的相对天数
            /// </summary>
            /// <param name="relativeday">增加的天数</param>
            /// <returns>相对天数</returns>
            public static string GetDateTimeOfDay(int relativeday)
            {
                return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
            }
            /// <summary>
            /// 返回相对于当前时间的相对分钟数
            /// </summary>
            /// <param name="relativeday">增加的分钟数</param>
            /// <returns>相对分钟数</returns>
            public static string GetDateTimeOfMinutes(int relativeday)
            {
                return DateTime.Now.AddMinutes(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
            }
            /// <summary>
            /// 返回当前时间的标准时间格式
            /// </summary>
            /// <returns>yyyy-MM-dd HH:mm:ss:fffffff</returns>
            public static string GetDateTimeF()
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
            }

            #endregion

            #region 时间转换

            /// <summary>
            /// 返回指定日期格式
            /// </summary>
            /// <param name="datetimestr">需要转换的时间</param>
            /// <param name="replacestr">指定格式</param>
            /// <returns>转换后的时间</returns>
            public static string GetDate(string datetimestr, string replacestr)
            {
                if (datetimestr == null) return replacestr;
                if (datetimestr.Equals("")) return replacestr;
                try
                {
                    datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
                }
                catch
                {
                    return replacestr;
                }
                return datetimestr;
            }
            /// <summary>
            /// 转换时间为unix时间戳
            /// </summary>
            /// <param name="date">需要传递UTC时间,避免时区误差,例:DataTime.UTCNow</param>
            /// <returns></returns>
            public static double ConvertToUnixTimestamp(DateTime date)
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                TimeSpan diff = date - origin;
                return Math.Floor(diff.TotalSeconds);
            }
            /// <summary>
            /// 将8位日期型整型数据转换为日期字符串数据
            /// 默认为英文格式
            /// </summary>
            /// <param name="date">整型日期</param>
            /// <returns></returns>
            public static string FormatDate(int date)
            {
                return FormatDate(date, false);
            }
            /// <summary>
            /// 将8位日期型整型数据转换为日期字符串数据
            /// </summary>
            /// <param name="date">整型日期</param>
            /// <param name="chnType">是否以中文年月日输出</param>
            /// <returns></returns>
            public static string FormatDate(int date, bool chnType)
            {
                string dateStr = date.ToString();
                if (date <= 0 || dateStr.Length != 8) return dateStr;
                if (chnType) return dateStr.Substring(0, 4) + "年" + dateStr.Substring(4, 2) + "月" + dateStr.Substring(6) + "日";
                return dateStr.Substring(0, 4) + "-" + dateStr.Substring(4, 2) + "-" + dateStr.Substring(6);
            }

            #endregion

            #region 标准时间

            /// <summary>
            /// 返回标准时间
            /// </summary>
            /// <param name="fDateTime">转换时间</param>
            /// <param name="formatStr">转换格式</param>
            /// <returns>转换后的时间</returns>
            public static string GetStandardDateTime(string fDateTime, string formatStr)
            {
                if (fDateTime == "0000-0-0 0:00:00") return fDateTime;
                DateTime time = new DateTime(1900, 1, 1, 0, 0, 0, 0);
                if (DateTime.TryParse(fDateTime, out time))
                {
                    return time.ToString(formatStr);
                }
                else
                {
                    return "N/A";
                }
            }
            /// <summary>
            /// 返回标准时间 yyyy-MM-dd HH:mm:ss
            /// </summary>
            /// <param name="fDateTime">转换时间</param>
            /// <returns>yyyy-MM-dd HH:mm:ss</returns>
            public static string GetStandardDateTime(string fDateTime)
            {
                return GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
            }
            /// <summary>
            /// 返回标准时间 yyyy-MM-dd
            /// </summary>
            /// <param name="fDate">转换时间</param>
            /// <returns>yyyy-MM-dd</returns>
            public static string GetStandardDate(string fDate)
            {
                return GetStandardDateTime(fDate, "yyyy-MM-dd");
            }

        #endregion

        #region 返回相差 秒数、分钟数、小时数

        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="time">时间字符串</param>
        /// <param name="Sec">秒数</param>
        /// <returns>相差的秒数</returns>
        public static int DateOfDiffSeconds(string time, int Sec)
            {
                if (string.IsNullOrEmpty(time)) return 1;
                DateTime dateTime = DateTime.Parse("1900-01-01");
                if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01") return 1;
                TimeSpan ts = DateTime.Now - dateTime.AddSeconds(Sec);
                if (ts.TotalSeconds > int.MaxValue)
                {
                    return int.MaxValue;
                }
                else if (ts.TotalSeconds < int.MinValue)
                {
                    return int.MinValue;
                }
                return (int)ts.TotalSeconds;
            }
            /// <summary>
            /// 返回相差的分钟数
            /// </summary>
            /// <param name="time">时间字符串</param>
            /// <param name="minutes">分钟数</param>
            /// <returns>相差的分钟数</returns>
            public static int DateOfDiffMinutes(string time, int minutes)
            {
                if (string.IsNullOrEmpty(time)) return 1;
            // DateTime dateTime = TypeHelper.StrToDateTime(time, DateTime.Parse("1900-01-01"));
            DateTime dateTime = DateTime.Parse("1900-01-01");
            if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01") return 1;
                TimeSpan ts = DateTime.Now - dateTime.AddMinutes(minutes);
                if (ts.TotalMinutes > int.MaxValue)
                {
                    return int.MaxValue;
                }
                else if (ts.TotalMinutes < int.MinValue)
                {
                    return int.MinValue;
                }
                return (int)ts.TotalMinutes;
            }
            /// <summary>
            /// 返回相差的小时数
            /// </summary>
            /// <param name="time">时间字符串</param>
            /// <param name="hours">小时数</param>
            /// <returns>相差的小时数</returns>
            public static int DateOfDiffHours(string time, int hours)
            {
                if (string.IsNullOrEmpty(time)) return 1;
                //DateTime dateTime = TypeHelper.StrToDateTime(time, DateTime.Parse("1900-01-01"));
                  DateTime dateTime = DateTime.Parse("1900-01-01");
                if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01") return 1;
                TimeSpan ts = DateTime.Now - dateTime.AddHours(hours);
                if (ts.TotalHours > int.MaxValue)
                {
                    return int.MaxValue;
                }
                else if (ts.TotalHours < int.MinValue)
                {
                    return int.MinValue;
                }
                return (int)ts.TotalHours;
            }

            #endregion

            #region 时间格式检测

            /// <summary>
            /// 是否常规时间
            /// </summary>
            public static bool IsTime(string timeval)
            {
                return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
            }
            /// <summary>
            /// 判断字符串是否是yy-mm-dd字符串
            /// </summary>
            /// <param name="str">待判断字符串</param>
            /// <returns>判断结果</returns>
            public static bool IsDateString(string str)
            {
                return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
            }

            #endregion

            #region 年

            /// <summary>
            /// 判断某年是否为闰年
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <returns>是否为闰年</returns>
            public static bool YearIsLeap(int year)
            {
                if (year <= 0001 || year >= 9999) return false;
                return DateTime.IsLeapYear(year);
            }
            /// <summary>
            /// 计算某年共有多少天
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <returns>共有多少天</returns>
            public static int YearOfTotalDay(int year)
            {
                if (year <= 0001 || year >= 9999) return -1;
                return YearOfLastDay(year).DayOfYear;
            }
            /// <summary>
            /// 获得某年第一天的日期
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <returns>第一天日期</returns>
            public static DateTime YearOfFirstDay(int year)
            {
                if (year <= 0001 || year >= 9999) return DateTime.MaxValue;
                DateTime result = DateTime.MinValue;
                DateTime.TryParse(string.Format("{0}-01-01", year), out result);
                return result;
            }
            /// <summary>
            /// 获得某年最后一天的日期
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <returns>最后一天日期</returns>
            public static DateTime YearOfLastDay(int year)
            {
                if (year <= 0001 || year >= 9999) return DateTime.MaxValue;
                DateTime result = DateTime.MaxValue;
                DateTime.TryParse(string.Format("{0}-12-31", year), out result);
                return result;
            }

            #endregion

            #region 月

            /// <summary>
            /// 获得当前年当前月的总天数
            /// </summary>
            /// <returns>共有多少天</returns>
            public static int MonthOfTotalDay()
            {
                DateTime _now = DateTime.Now;
                return DateTime.DaysInMonth(_now.Year, _now.Month);
            }
            /// <summary>
            /// 获得某年某月的总天数
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <param name="month">需要计算的月份</param>
            /// <returns>共有多少天</returns>
            public static int MonthOfTotalDay(int year, int month)
            {
                if (year <= 0001 || year >= 9999) return -1;
                if (month < 1 || month > 12) return -1;
                return DateTime.DaysInMonth(year, month);
            }
            /// <summary>
            /// 获得当前年当前月第一天的日期
            /// </summary>
            /// <returns>第一天日期</returns>
            public static DateTime MonthOfFirstDay()
            {
                DateTime _now = DateTime.Now;
                return MonthOfFirstDay(_now.Year, _now.Month);
            }
            /// <summary>
            /// 获得某年某月第一天的日期
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <param name="month">需要计算的月份</param>
            /// <returns>第一天日期</returns>
            public static DateTime MonthOfFirstDay(int year, int month)
            {
                if (year <= 0001 || year >= 9999) return DateTime.MaxValue;
                if (month < 1 || month > 12) return DateTime.MaxValue;
                DateTime result = DateTime.MinValue;
                DateTime.TryParse(string.Format("{0}-{1}-01", year, month), out result);
                return result;
            }
            /// <summary>
            /// 获得当前年当前月最后一天的日期
            /// </summary>
            /// <returns>最后一天日期</returns>
            public static DateTime MonthOfLastDay()
            {
                DateTime _now = DateTime.Now;
                return MonthOfLastDay(_now.Year, _now.Month);
            }
            /// <summary>
            /// 获得某年某月最后一天的日期
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <param name="month">需要计算的月份</param>
            /// <returns>最后一天日期</returns>
            public static DateTime MonthOfLastDay(int year, int month)
            {
                if (year <= 0001 || year >= 9999) return DateTime.MaxValue;
                if (month < 1 || month > 12) return DateTime.MaxValue;
                DateTime result = DateTime.MaxValue;
                string tmpString = string.Format("{0}-{1}-{2}", year, month, DateTime.DaysInMonth(year, month));
                DateTime.TryParse(tmpString, out result);
                return result;
            }

            #endregion

            #region 周

            /// <summary>
            /// 周（星期）信息实体类
            /// </summary>
            [Serializable]
            public class WeekInfo
            {
                /// <summary>
                /// 周（星期）信息实体类
                /// </summary>
                public WeekInfo()
                {
                    Number = 0;
                    BeginDate = DateTime.MinValue;
                    EndDate = DateTime.MaxValue;
                }
                /// <summary>
                /// 周数
                /// </summary>
                public int Number { get; set; }
                /// <summary>
                /// 开始时间
                /// </summary>
                public DateTime BeginDate { get; set; }
                /// <summary>
                /// 结束时间
                /// </summary>
                public DateTime EndDate { get; set; }
                /// <summary>
                /// 输出第几周和日期间隔
                /// <para>默认格式：</para>
                /// <para>第几周，从2012年12月21日至2112年12月21日。</para>
                /// </summary>
                /// <returns>第几周和日期间隔</returns>
                public override string ToString()
                {
                    return string.Format("第{0}周，从{1}至{2}。", Number, BeginDate.ToString("yyyy年MM月dd日"), EndDate.ToString("yyyy年MM月dd日"));
                }
                /// <summary>
                /// 输出第几周
                /// </summary>
                /// <param name="isFill">少于2位时是否补零</param>
                /// <returns>第几周</returns>
                public string GetWeekString(bool isFill)
                {
                    string _format = "第{0}周";
                    if (isFill && Number < 10) _format = "第0{0}周";
                    return string.Format(_format, Number);
                }
                /// <summary>
                /// 输出日期间隔
                /// </summary>
                /// <param name="inputString">输出格式化字符串</param>
                /// <param name="dateString">日期格式化字符串</param>
                /// <returns>日期间隔</returns>
                public string GetDateString(string inputString, string dateString)
                {
                    if (string.IsNullOrWhiteSpace(inputString) || string.IsNullOrWhiteSpace(dateString)) return null;
                    try
                    {
                        return string.Format(inputString, BeginDate.ToString(dateString), EndDate.ToString(dateString));
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            /// <summary>
            /// 获得年度第一个周一的日期
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <param name="jumpYear">年度第一周是否跳过跨年的周数</param>
            /// <param name="offset">年度第一个周一偏移量</param>
            /// <returns>年度第一个周一的日期</returns>
            public static DateTime WeekOfFirstDay(int year, bool jumpYear, out int offset)
            {
                DateTime firstDate = YearOfFirstDay(year); //该年的第一天
                int firstWeek = (int)firstDate.DayOfWeek; //该年的第一天是周几
                offset = 0; //周一偏移量
                switch (firstWeek)
                {
                    case 1: //星期一
                        offset = jumpYear ? 0 : 0;
                        break;
                    case 2: //星期二
                        offset = jumpYear ? 6 : -1;
                        break;
                    case 3: //星期三
                        offset = jumpYear ? 5 : -2;
                        break;
                    case 4: //星期四
                        offset = jumpYear ? 4 : -3;
                        break;
                    case 5: //星期五
                        offset = jumpYear ? 3 : -4;
                        break;
                    case 6: //星期六
                        offset = jumpYear ? 2 : -5;
                        break;
                    case 0: //星期日
                        offset = jumpYear ? 1 : -6;
                        break;
                }
                firstDate = firstDate.AddDays(offset);
                return firstDate;
            }
            /// <summary>
            /// 获得年度周（星期）信息实体集合列表
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <param name="jumpYear">年度第一周是否跳过跨年的周数</param>
            /// <returns>周（星期）信息实体集合列表对象</returns>
            public static IList<WeekInfo> WeekOfList(int year, bool jumpYear)
            {
                IList<WeekInfo> weekList = new List<WeekInfo>();
                if (year <= 0001 || year >= 9999) return weekList;
                int offset = 0;
                DateTime firstDate = WeekOfFirstDay(year, jumpYear, out offset); //年度周一的日期
                int index = 1;
                WeekInfo weekInfo;
                while (true)
                {
                    if (index > 54) break;
                    weekInfo = new WeekInfo();
                    weekInfo.Number = index;
                    weekInfo.BeginDate = firstDate; //周一
                    weekInfo.EndDate = firstDate.AddDays(6); //周日
                    weekList.Add(weekInfo);
                    firstDate = firstDate.AddDays(7); //下周
                    if (jumpYear)
                    {
                        if (firstDate.Year != year) break;
                    }
                    else
                    {
                        if (firstDate.AddDays(6).Year != year) break;
                    }
                    index++;
                }
                return weekList;
            }
            /// <summary>
            /// 获得某年第某周的开始日期和结束日期
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <param name="weekNumber">需要计算的周数</param>
            /// <param name="jumpYear">年度第一周是否跳过跨年的周数</param>
            /// <param name="weekBeginDate">开始日期</param>
            /// <param name="weekEndDate">结束日期</param>
            public static void WeekOfDate(int year, int weekNumber, bool jumpYear, out DateTime weekBeginDate, out DateTime weekEndDate)
            {
                weekBeginDate = DateTime.MinValue;
                weekEndDate = DateTime.MaxValue;
                if (year <= 0001 || year >= 9999 || weekNumber < 1 || weekNumber > 54) return;
                int offset = 0;
                DateTime firstDate = WeekOfFirstDay(year, jumpYear, out offset); //年度周一的日期
                firstDate = firstDate.AddDays((weekNumber - 1) * 7);
                weekBeginDate = firstDate;
                weekEndDate = firstDate.AddDays(6);
            }
            /// <summary>
            /// 获得某个日期属于某年的第几周
            /// </summary>
            /// <param name="currentDate">需要计算的日期</param>
            /// <param name="jumpYear">年度第一周是否跳过跨年的周数</param>
            /// <returns>某年的第几周</returns>
            public static int WeekOfCurrent(DateTime currentDate, bool jumpYear)
            {
                if (DateTime.MinValue == currentDate || DateTime.MaxValue == currentDate) return 0;
                int result = 0, offset = 0;
                DateTime firstDate = WeekOfFirstDay(currentDate.Year, jumpYear, out offset); //年度周一的日期
                int firstWeek = (int)firstDate.DayOfWeek; //该年的第一天是周几
                int currentDay = currentDate.DayOfYear;
                if (offset > 0)
                {
                    currentDay += offset;
                }
                else
                {
                    currentDay -= offset;
                }
                int remainderDay = currentDay - (7 - firstWeek);
                if (remainderDay < 1)
                {
                    result = 1;
                }
                else
                {
                    result = remainderDay / 7;
                    if (remainderDay % 7 != 0) result++;
                    result++;
                }
                return result;
            }
            /// <summary>
            /// 统计一段时间内有多少个星期几
            /// </summary>
            /// <param name="beginDate">开始日期</param>
            /// <param name="endDate">结束日期</param>
            /// <param name="weekNumber">星期几</param>
            /// <returns>多少个星期几</returns>
            public static int WeekOfTotalWeeks(DateTime beginDate, DateTime endDate, DayOfWeek weekNumber)
            {
                TimeSpan _dayTotal = new TimeSpan(endDate.Ticks - beginDate.Ticks);
                int result = (int)_dayTotal.TotalDays / 7;
                double iLen = _dayTotal.TotalDays % 7;
                for (int i = 0; i <= iLen; i++)
                {
                    if (beginDate.AddDays(i).DayOfWeek == weekNumber)
                    {
                        result++;
                        break;
                    }
                }
                return result;
            }

            #endregion

            #region 季度

            /// <summary>
            /// 计算当前月属于第几季度
            /// </summary>
            /// <returns>当前年第几季度</returns>
            public static int QuarterOfCurrent()
            {
                return QuarterOfCurrent(DateTime.Now.Month);
            }
            /// <summary>
            /// 计算某个月属于第几季度
            /// </summary>
            /// <param name="month">需要计算的月份</param>
            /// <returns>某年第几季度</returns>
            public static int QuarterOfCurrent(int month)
            {
                if (month < 1 || month > 12) return -1;
                return (month - 1) / 3 + 1;
            }
            /// <summary>
            /// 获得当前年当前季度的开始日期和结束日期
            /// </summary>
            /// <param name="quarterBeginDate">开始日期</param>
            /// <param name="quarterEndDate">结束日期</param>
            public static void QuarterOfDate(out DateTime quarterBeginDate, out DateTime quarterEndDate)
            {
                int quarter = QuarterOfCurrent(DateTime.Now.Month);
                QuarterOfDate(DateTime.Now.Year, quarter, out quarterBeginDate, out quarterEndDate);
            }
            /// <summary>
            /// 获得指定日期所在季度的开始日期和结束日期
            /// </summary>
            /// <param name="fromDate">需要计算的日期</param>
            /// <param name="quarterBeginDate">开始日期</param>
            /// <param name="quarterEndDate">结束日期</param>
            public static void QuarterOfDate(DateTime fromDate, out DateTime quarterBeginDate, out DateTime quarterEndDate)
            {
                int quarter = QuarterOfCurrent(fromDate.Month);
                QuarterOfDate(fromDate.Year, quarter, out quarterBeginDate, out quarterEndDate);
            }
            /// <summary>
            /// 获得某年第某季度的开始日期和结束日期
            /// </summary>
            /// <param name="year">需要计算的年份</param>
            /// <param name="quarter">需要计算的季度</param>
            /// <param name="quarterBeginDate">开始日期</param>
            /// <param name="quarterEndDate">结束日期</param>
            public static void QuarterOfDate(int year, int quarter, out DateTime quarterBeginDate, out DateTime quarterEndDate)
            {
                quarterBeginDate = DateTime.MinValue;
                quarterEndDate = DateTime.MaxValue;
                if (year <= 0001 || year >= 9999 || quarter < 1 || quarter > 4) return;
                int month = (quarter - 1) * 3 + 1;
                quarterBeginDate = new DateTime(year, month, 1);
                quarterEndDate = quarterBeginDate.AddMonths(3).AddMilliseconds(-1);
            }

        #endregion
        /// <summary>

        /// 获取指定年份的最大周数

        /// </summary>

        /// <param name="year">年份</param>

        /// <returns>周数</returns>

        public static int GetMaxWeekOfYear(int year)

        {

            DateTime tempDate = new DateTime(year, 12, 31);

            int tempDayOfWeek = (int)tempDate.DayOfWeek;

            if (tempDayOfWeek != 0)

            {

                tempDate = tempDate.Date.AddDays(-tempDayOfWeek);

            }

            return GetWeekIndex(tempDate);

        }





        /// <summary>

        /// 获取当前日期是一年中的第几周，如果12月31号与下一年的1月1好在同一个星期则算下一年的第一周

        /// </summary>

        /// <param name="dt"></param>

        /// <returns></returns>

        public static int GetWeekIndex(DateTime dt)

        {

            //确定此时间在一年中的位置

            int dayOfYear = dt.DayOfYear;



            //当年第一天

            DateTime tempDate = new DateTime(dt.Year, 1, 1);



            //确定当年第一天

            int tempDayOfWeek = (int)tempDate.DayOfWeek;

            tempDayOfWeek = tempDayOfWeek == 0 ? 7 : tempDayOfWeek;

            //确定星期几

            int index = (int)dt.DayOfWeek;



            index = index == 0 ? 7 : index;



            //当前周的范围

            DateTime retStartDay = dt.AddDays(-(index - 1));

            DateTime retEndDay = dt.AddDays(7 - index);



            //确定当前是第几周

            int weekIndex = (int)Math.Ceiling(((double)dayOfYear + tempDayOfWeek - 1) / 7);



            if (retStartDay.Year < retEndDay.Year)

            {

                weekIndex = 1;

            }



            return weekIndex;

        }





        /// <summary>

        /// 获取指定年份第几周的日期范围

        /// </summary>

        /// <param name="year">年份</param>

        /// <param name="weekIndex">第n周</param>

        /// <returns></returns>

        public static bool GetWeekRange(int year, int weekIndex, out DateTime weekRangeStart, out DateTime weekRangeEnd)

        {



            if (weekIndex < 1)

            {

                throw new Exception("请输入大于0的整数");

            }



            int allDays = (weekIndex - 1) * 7;

            //确定当年第一天

            DateTime firstDate = new DateTime(year, 1, 1);

            int firstDayOfWeek = (int)firstDate.DayOfWeek;



            firstDayOfWeek = firstDayOfWeek == 0 ? 7 : firstDayOfWeek;



            //周开始日

            int startAddDays = allDays + (1 - firstDayOfWeek);

            weekRangeStart = firstDate.AddDays(startAddDays);

            //周结束日

            int endAddDays = allDays + (7 - firstDayOfWeek);

            weekRangeEnd = firstDate.AddDays(endAddDays);



            if (weekRangeStart.Year > year || (weekRangeStart.Year == year && weekRangeEnd.Year > year))

            {

                //throw new Exception("今年没有第" + weekIndex + "周。");

                return false;

            }

            else

            {

                return true;

            }



        }



        /// <summary>

        /// 获取本周的周日周六的日期值

        /// </summary>

        /// <param name="date">当前日期</param>

        /// <param name="dtSun">周日</param>

        /// <param name="dtSat">周六</param>

        public static void GetWeekRange(DateTime date, out DateTime dtSun, out DateTime dtSat)

        {

            dtSun = System.DateTime.Now;

            dtSat = System.DateTime.Now;

            switch (date.DayOfWeek)

            {

                case System.DayOfWeek.Sunday:

                    dtSun = date;

                    dtSat = date.AddDays(6);

                    break;

                case System.DayOfWeek.Monday:

                    dtSun = date.AddDays(-1);

                    dtSat = date.AddDays(5);

                    break;



                case System.DayOfWeek.Tuesday:

                    dtSun = date.AddDays(-2);

                    dtSat = date.AddDays(4);

                    break;



                case System.DayOfWeek.Wednesday:

                    dtSun = date.AddDays(-3);

                    dtSat = date.AddDays(3);

                    break;



                case System.DayOfWeek.Thursday:

                    dtSun = date.AddDays(-4);

                    dtSat = date.AddDays(2);

                    break;



                case System.DayOfWeek.Friday:

                    dtSun = date.AddDays(-5);

                    dtSat = date.AddDays(1);

                    break;



                case System.DayOfWeek.Saturday:

                    dtSun = date.AddDays(-6);

                    dtSat = date;

                    break;

            }

        }





        /// <summary>

        /// 转换成星期中文汉字

        /// </summary>

        /// <param name="index"></param>

        /// <returns></returns>

        public static string GetWeekDayName(int index)

        {

            string result = string.Empty;

            switch (index)

            {

                case 0:

                    {

                        result = "星期日";

                        break;

                    }

                case 1:

                    {

                        result = "星期一";

                        break;

                    }

                case 2:

                    {

                        result = "星期二";

                        break;

                    }

                case 3:

                    {

                        result = "星期三";

                        break;

                    }

                case 4:

                    {

                        result = "星期四";

                        break;

                    }

                case 5:

                    {

                        result = "星期五";

                        break;

                    }

                case 6:

                    {

                        result = "星期六";

                        break;

                    }

            }



            return result;

          }

        #region 获得两个日期的间隔
        /// <summary>
        /// 获得两个日期的间隔
        /// </summary>
        /// <param name="DateTime1">日期一。</param>
        /// <param name="DateTime2">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            return ts;
        }
        #endregion

        #region 格式化日期时间
        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime1.ToString("yyyy-MM-dd");
                case "1":
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");
                case "2":
                    return dateTime1.ToString("yyyy/MM/dd");
                case "3":
                    return dateTime1.ToString("yyyy年MM月dd日");
                case "4":
                    return dateTime1.ToString("MM-dd");
                case "5":
                    return dateTime1.ToString("MM/dd");
                case "6":
                    return dateTime1.ToString("MM月dd日");
                case "7":
                    return dateTime1.ToString("yyyy-MM");
                case "8":
                    return dateTime1.ToString("yyyy/MM");
                case "9":
                    return dateTime1.ToString("yyyy年MM月");
                default:
                    return dateTime1.ToString();
            }
        }
        #endregion

        #region 得到随机日期
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            Random random = new Random();
            DateTime minTime = new DateTime();
            DateTime maxTime = new DateTime();

            System.TimeSpan ts = new System.TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            double dTotalSecontds = ts.TotalSeconds;
            int iTotalSecontds = 0;

            if (dTotalSecontds > System.Int32.MaxValue)
            {
                iTotalSecontds = System.Int32.MaxValue;
            }
            else if (dTotalSecontds < System.Int32.MinValue)
            {
                iTotalSecontds = System.Int32.MinValue;
            }
            else
            {
                iTotalSecontds = (int)dTotalSecontds;
            }


            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            int maxValue = iTotalSecontds;

            if (iTotalSecontds <= System.Int32.MinValue)
                maxValue = System.Int32.MinValue + 1;

            int i = random.Next(System.Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        public static string GetRandomTimeSpan()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion
        public static string GetIsoWeek(DateTime date)
        {
            var calendar = CultureInfo.InvariantCulture.Calendar;

            if (date.DayOfWeek >= DayOfWeek.Monday && date.DayOfWeek <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            var week = calendar.GetWeekOfYear(
                date,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);

            var year = date.Year;

            if (date.Month == 1 && week >= 52)
            {
                year -= 1;
            }

            return string.Format("{0}-W{1:00}", year, week);
        }
    }
}