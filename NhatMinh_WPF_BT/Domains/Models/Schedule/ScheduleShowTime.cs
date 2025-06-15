using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class ScheduleShowTime : IHasDateAndStatus
    {
        public string Id { get; set; }
        public string IdSchedule { get; set; }
        public DateTime AirDate { get; set; }
        public bool Status { get; set; }

        public List<BoughtSeat> BoughtSeats { get; set; } = new List<BoughtSeat>();

        public DateTime GetDateToCheck() => AirDate;

        public ScheduleShowTime(string id, string idSchedule, DateTime airDate, bool status)
        {
            Id = id;
            IdSchedule = idSchedule;
            AirDate = airDate;
            Status = status;
        }
    }
}

