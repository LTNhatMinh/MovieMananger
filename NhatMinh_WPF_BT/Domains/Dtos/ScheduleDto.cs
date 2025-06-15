using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class ScheduleDto
    {
        public string ScheduleId { get; set; }
        public string MovieName { get; set; }
        public string IdCinema { get; set; }
        public string CinemaName { get; set; }
        public DateTime AirDate { get; set; }
        public bool Status { get; set; }

        public ScheduleDto(string scheduleId, string movieName, string idCinema, string cinemaName, DateTime airDate, bool status)
        {
            ScheduleId = scheduleId;
            MovieName = movieName;
            IdCinema = idCinema;
            CinemaName = cinemaName;
            AirDate = airDate;
            Status = status;
        }
    }
}
