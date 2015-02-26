namespace NB.StockStudio.Foundation
{
    using System;

    public class Chinese
    {
        private static string[] Animals = new string[] { "鼠", "牛", "虎", "兔", "龙", "蛇", "马", "羊", "猴", "鸡", "狗", "猪" };
        private static string[] Gan = new string[] { "甲", "乙", "丙", "丁", "戊", "己", "庚", "辛", "壬", "癸" };
        private static int[] LunarInfo = new int[] { 
            0x4bd8, 0x4ae0, 0xa570, 0x54d5, 0xd260, 0xd950, 0x16554, 0x56a0, 0x9ad0, 0x55d2, 0x4ae0, 0xa5b6, 0xa4d0, 0xd250, 0x1d255, 0xb540, 
            0xd6a0, 0xada2, 0x95b0, 0x14977, 0x4970, 0xa4b0, 0xb4b5, 0x6a50, 0x6d40, 0x1ab54, 0x2b60, 0x9570, 0x52f2, 0x4970, 0x6566, 0xd4a0, 
            0xea50, 0x6e95, 0x5ad0, 0x2b60, 0x186e3, 0x92e0, 0x1c8d7, 0xc950, 0xd4a0, 0x1d8a6, 0xb550, 0x56a0, 0x1a5b4, 0x25d0, 0x92d0, 0xd2b2, 
            0xa950, 0xb557, 0x6ca0, 0xb550, 0x15355, 0x4da0, 0xa5d0, 0x14573, 0x52d0, 0xa9a8, 0xe950, 0x6aa0, 0xaea6, 0xab50, 0x4b60, 0xaae4, 
            0xa570, 0x5260, 0xf263, 0xd950, 0x5b57, 0x56a0, 0x96d0, 0x4dd5, 0x4ad0, 0xa4d0, 0xd4d4, 0xd250, 0xd558, 0xb540, 0xb5a0, 0x195a6, 
            0x95b0, 0x49b0, 0xa974, 0xa4b0, 0xb27a, 0x6a50, 0x6d40, 0xaf46, 0xab60, 0x9570, 0x4af5, 0x4970, 0x64b0, 0x74a3, 0xea50, 0x6b58, 
            0x55c0, 0xab60, 0x96d5, 0x92e0, 0xc960, 0xd954, 0xd4a0, 0xda50, 0x7552, 0x56a0, 0xabb7, 0x25d0, 0x92d0, 0xcab5, 0xa950, 0xb4a0, 
            0xbaa4, 0xad50, 0x55d9, 0x4ba0, 0xa5b0, 0x15176, 0x52b0, 0xa930, 0x7954, 0x6aa0, 0xad50, 0x5b52, 0x4b60, 0xa6e6, 0xa4e0, 0xd260, 
            0xea65, 0xd530, 0x5aa0, 0x76a3, 0x96d0, 0x4bd7, 0x4ad0, 0xa4d0, 0x1d0b6, 0xd250, 0xd520, 0xdd45, 0xb5a0, 0x56d0, 0x55b2, 0x49b0, 
            0xa577, 0xa4b0, 0xaa50, 0x1b255, 0x6d20, 0xada0
         };
        private static string[] MonthName = new string[] { "1 月", "2 月", "3 月", "4 月", "5 月", "6 月", "7 月", "8 月", "9 月", "10 月", "11 月", "12 月" };
        private static int[] SolarMonth = new int[] { 0x1f, 0x1c, 0x1f, 30, 0x1f, 30, 0x1f, 0x1f, 30, 0x1f, 30, 0x1f };
        private static string[] SolarTerm = new string[] { 
            "小寒", "大寒", "立春", "雨水", "惊蛰", "春分", "清明", "谷雨", "立夏", "小满", "芒种", "夏至", "小暑", "大暑", "立秋", "处暑", 
            "白露", "秋分", "寒露", "霜降", "立冬", "小雪", "大雪", "冬至"
         };
        private static string[] Str1 = new string[] { "日", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
        private static string[] Str2 = new string[] { "初", "十", "廿", "卅", "　" };
        private static int[] TermInfo = new int[] { 
            0, 0x52d8, 0xa5e3, 0xf95c, 0x14d59, 0x1a206, 0x1f763, 0x24d89, 0x2a45d, 0x2fbdf, 0x353d8, 0x3ac35, 0x404af, 0x45d25, 0x4b553, 0x50d19, 
            0x56446, 0x5bac6, 0x61087, 0x6658a, 0x6b9db, 0x70d90, 0x760cc, 0x7b3b6
         };
        private static string[] Zhi = new string[] { "子", "丑", "寅", "卯", "辰", "巳", "午", "未", "申", "酉", "戌", "亥" };

        private static int LeapDays(int y)
        {
            if (LeapMonth(y) != 0)
            {
                return (((LunarInfo[y - 0x76c] & 0x10000) != 0) ? 30 : 0x1d);
            }
            return 0;
        }

        private static int LeapMonth(int y)
        {
            return (LunarInfo[y - 0x76c] & 15);
        }

        public static LunarInfo Lunar(DateTime d)
        {
            LunarInfo info = new LunarInfo();
            DateTime time = new DateTime(0x76c, 1, 1);
            TimeSpan span = (TimeSpan) (d - time);
            int totalDays = (int) span.TotalDays;
            info.DayCyl = totalDays + 40;
            info.MonthCyl = 14;
            int num2 = 0;
            int y = 0x76c;
            while ((y < 0x802) && (totalDays > 0))
            {
                num2 = lYearDays(y);
                totalDays -= num2;
                info.MonthCyl += 12;
                y++;
            }
            if (totalDays < 0)
            {
                totalDays += num2;
                y--;
                info.MonthCyl -= 12;
            }
            info.Year = y;
            info.YearCyl = y - 0x748;
            int num4 = LeapMonth(y);
            info.IsLeap = false;
            y = 1;
            while ((y < 13) && (totalDays > 0))
            {
                if (((num4 > 0) && (y == (num4 + 1))) && !info.IsLeap)
                {
                    y--;
                    info.IsLeap = true;
                    num2 = LeapDays(info.Year);
                }
                else
                {
                    num2 = MonthDays(info.Year, y);
                }
                if (info.IsLeap && (y == (num4 + 1)))
                {
                    info.IsLeap = false;
                }
                totalDays -= num2;
                if (!info.IsLeap)
                {
                    info.MonthCyl++;
                }
                y++;
            }
            if (((totalDays == 0) && (num4 > 0)) && (y == (num4 + 1)))
            {
                if (info.IsLeap)
                {
                    info.IsLeap = false;
                }
                else
                {
                    info.IsLeap = true;
                    y--;
                    info.MonthCyl--;
                }
            }
            if (totalDays < 0)
            {
                totalDays += num2;
                y--;
                info.MonthCyl--;
            }
            info.Month = y - 1;
            info.Day = totalDays;
            return info;
        }

        private static int lYearDays(int y)
        {
            int num = 0x15c;
            for (int i = 0x8000; i > 8; i = i >> 1)
            {
                num += ((LunarInfo[y - 0x76c] & i) != 0) ? 1 : 0;
            }
            return (num + LeapDays(y));
        }

        private static int MonthDays(int y, int m)
        {
            return (((LunarInfo[y - 0x76c] & (((int) 0x10000) >> m)) != 0) ? 30 : 0x1d);
        }
    }
}

