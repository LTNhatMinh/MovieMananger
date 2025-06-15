using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public static class Parameter
    {
        public static int CinemaStandardPrice = 60000;
        public static int CinemaStandardKidDiscount = 50;

        public static int CinemaVipPrice = 100000;
        public static int CinemaVipSurcharges = 40000;

        public static int PreparationTime = 60;

        public static List<string> CinemaType = new List<string> { "Standard", "Vip" };
        public static List<string> VipTheaterSupport = new List<string> { "Popcorn", "Soft Drink" };
    }
    public static class FilePath
    {
        public static string Account = "Data/Account.xml";
        public static string Cinema = "Data/Cinema.xml";
        public static string Movie = "Data/Movie.xml";
        public static string Orders = "Data/Orders.xml";
        public static string Schedule = "Data/Schedule.xml";
        public static string ScheduleShowTimes = "Data/ScheduleShowTimes.xml";
    }
}
