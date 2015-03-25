using System;
using System.Collections;
using System.Globalization;

namespace stonefw.Utility
{
    public static class ChinaDate
    {
        private static readonly ChineseLunisolarCalendar China = new ChineseLunisolarCalendar();
        private static readonly Hashtable GHoliday = new Hashtable();
        private static readonly Hashtable NHoliday = new Hashtable();
        private static readonly string[] Jq = { "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至" };
        private static readonly int[] JqData = { 0, 21208, 43467, 63836, 85337, 107014, 128867, 150921, 173149, 195551, 218072, 240693, 263343, 285989, 308563, 331033, 353350, 375494, 397447, 419210, 440795, 462224, 483532, 504758 };

        static ChinaDate()
        {
            //公历节日
            GHoliday.Add("0101", "元旦");
            GHoliday.Add("0214", "情人节");
            GHoliday.Add("0305", "雷锋日");
            GHoliday.Add("0308", "妇女节");
            GHoliday.Add("0312", "植树节");
            GHoliday.Add("0315", "消费者权益日");
            GHoliday.Add("0401", "愚人节");
            GHoliday.Add("0501", "劳动节");
            GHoliday.Add("0504", "青年节");
            GHoliday.Add("0601", "儿童节");
            GHoliday.Add("0701", "建党节");
            GHoliday.Add("0801", "建军节");
            GHoliday.Add("0910", "教师节");
            GHoliday.Add("1001", "国庆节");
            GHoliday.Add("1224", "平安夜");
            GHoliday.Add("1225", "圣诞节");

            //农历节日
            NHoliday.Add("0101", "春节");
            NHoliday.Add("0115", "元宵节");
            NHoliday.Add("0505", "端午节");
            NHoliday.Add("0815", "中秋节");
            NHoliday.Add("0909", "重阳节");
            NHoliday.Add("1208", "腊八节");
        }

        /// <summary>
        /// 获取农历
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChinaDate(DateTime dt)
        {
            if (dt > China.MaxSupportedDateTime || dt < China.MinSupportedDateTime)
            {
                //日期范围：1901 年 2 月 19 日 - 2101 年 1 月 28 日
                throw new Exception(string.Format("日期超出范围！必须在{0}到{1}之间！", China.MinSupportedDateTime.ToString("yyyy-MM-dd"), China.MaxSupportedDateTime.ToString("yyyy-MM-dd")));
            }
            //string str = string.Format("{0} {1}{2}", GetYear(dt), GetMonth(dt), GetDay(dt));
            string str = string.Format("{0}{1}", GetMonth(dt), GetDay(dt));
            string strJq = GetSolarTerm(dt);
            if (strJq != "")
            {
                str += " (" + strJq + ")";
            }
            string strHoliday = GetHoliday(dt);
            if (strHoliday != "")
            {
                str += " " + strHoliday;
            }
            string strChinaHoliday = GetChinaHoliday(dt);
            if (strChinaHoliday != "")
            {
                str += " " + strChinaHoliday;
            }

            return str;
        }

        /// <summary>
        /// 获取农历年份
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetYear(DateTime dt)
        {
            int yearIndex = China.GetSexagenaryYear(dt);
            const string yearTg = " 甲乙丙丁戊己庚辛壬癸";
            const string yearDz = " 子丑寅卯辰巳午未申酉戌亥";
            const string yearSx = " 鼠牛虎兔龙蛇马羊猴鸡狗猪";
            int year = China.GetYear(dt);
            int yTg = China.GetCelestialStem(yearIndex);
            int yDz = China.GetTerrestrialBranch(yearIndex);

            string str = string.Format("[{1}]{2}{3}{0}", year, yearSx[yDz], yearTg[yTg], yearDz[yDz]);
            return str;
        }

        /// <summary>
        /// 获取农历月份
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetMonth(DateTime dt)
        {
            int year = China.GetYear(dt);
            int iMonth = China.GetMonth(dt);
            int leapMonth = China.GetLeapMonth(year);
            bool isLeapMonth = iMonth == leapMonth;
            if (leapMonth != 0 && iMonth >= leapMonth)
            {
                iMonth--;
            }

            string szText = "正二三四五六七八九十";
            string strMonth = isLeapMonth ? "闰" : "";
            if (iMonth <= 10)
            {
                strMonth += szText.Substring(iMonth - 1, 1);
            }
            else if (iMonth == 11)
            {
                strMonth += "十一";
            }
            else
            {
                strMonth += "腊";
            }
            return strMonth + "月";
        }

        /// <summary>
        /// 获取农历日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDay(DateTime dt)
        {
            int iDay = China.GetDayOfMonth(dt);
            string szText1 = "初十廿三";
            string szText2 = "一二三四五六七八九十";
            string strDay;
            if (iDay == 20)
            {
                strDay = "二十";
            }
            else if (iDay == 30)
            {
                strDay = "三十";
            }
            else
            {
                strDay = szText1.Substring((iDay - 1) / 10, 1);
                strDay = strDay + szText2.Substring((iDay - 1) % 10, 1);
            }
            return strDay;
        }

        /// <summary>
        /// 获取节气
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetSolarTerm(DateTime dt)
        {
            DateTime dtBase = new DateTime(1900, 1, 6, 2, 5, 0);
            DateTime dtNew;
            double num;
            int y;
            string strReturn = "";

            y = dt.Year;
            for (int i = 1; i <= 24; i++)
            {
                num = 525948.76 * (y - 1900) + JqData[i - 1];
                dtNew = dtBase.AddMinutes(num);
                if (dtNew.DayOfYear == dt.DayOfYear)
                {
                    strReturn = Jq[i - 1];
                }
            }

            return strReturn;
        }

        /// <summary>
        /// 获取公历节日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetHoliday(DateTime dt)
        {
            string strReturn = "";
            object g = GHoliday[dt.Month.ToString("00") + dt.Day.ToString("00")];
            if (g != null)
            {
                strReturn = g.ToString();
            }

            return strReturn;
        }

        /// <summary>
        /// 获取农历节日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChinaHoliday(DateTime dt)
        {
            string strReturn = "";
            int year = China.GetYear(dt);
            int iMonth = China.GetMonth(dt);
            int leapMonth = China.GetLeapMonth(year);
            int iDay = China.GetDayOfMonth(dt);
            if (China.GetDayOfYear(dt) == China.GetDaysInYear(year))
            {
                strReturn = "除夕";
            }
            else if (leapMonth != iMonth)
            {
                if (leapMonth != 0 && iMonth >= leapMonth)
                {
                    iMonth--;
                }
                object n = NHoliday[iMonth.ToString("00") + iDay.ToString("00")];
                if (n != null)
                {
                    if (strReturn == "")
                    {
                        strReturn = n.ToString();
                    }
                    else
                    {
                        strReturn += " " + n;
                    }
                }
            }

            return strReturn;
        }

        /// <summary>
        /// 获取星期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetWeek(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Saturday:
                    return "星期六";
                case DayOfWeek.Sunday:
                    return "星期日";
            }
            return "";
        }
    }
}