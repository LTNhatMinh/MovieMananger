using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class Movie : IHasDateAndStatus
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public DateTime StartAirDate { get; set; }
        public DateTime EndAirDate { get; set; }
        public bool Status { get; set; }

        public DateTime GetDateToCheck() => EndAirDate;

        public Movie(string id, string name, string description, int duration, DateTime startAirDate, DateTime endAirDate, bool status)
        {
            Id = id;
            Name = name;
            Description = description;
            Duration = duration;
            StartAirDate = startAirDate;
            EndAirDate = endAirDate;
            Status = status;
        }
    }
}
