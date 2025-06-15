using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class Schedule : IHasDateAndStatus
    {
        public string Id { get; set; }
        public string IdMovie { get; set; }
        public string IdCinema { get; set; }
        public DateTime AirDate { get; set; }
        public bool Status { get; set; }

        public DateTime GetDateToCheck() => AirDate;

        public Schedule(string id, string idMovie, string idCinema, DateTime airDate, bool status)
        {
            Id = id;
            IdMovie = idMovie;
            IdCinema = idCinema;
            AirDate = airDate;
            Status = status;
        }
    }
}
