using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class ScheduleShowTimeDto
    {
        public string Id { get; set; }
        public string MovieName { get; set; }
        public string CinemaName { get; set; }
        public string CinemaId { get; set; }
        public string ScheduleId { get; set; }
        public DateTime AirDate { get; set; }
        public string ShowTime { get; set; }
        public int Duration { get; set; }
        public bool Status { get; set; }

        public ScheduleShowTimeDto(string id, string movieName, string cinemaName, string cinemaId, string scheduleId, DateTime airDate, string showTime, int duration, bool status)
        {
            Id = id;
            MovieName = movieName;
            CinemaName = cinemaName;
            CinemaId = cinemaId;
            ScheduleId = scheduleId;
            AirDate = airDate;
            ShowTime = showTime;
            Duration = duration;
            Status = status;
        }
    }
}
