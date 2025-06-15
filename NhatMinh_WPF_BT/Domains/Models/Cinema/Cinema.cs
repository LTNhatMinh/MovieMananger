using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class Cinema
    {
        public Cinema(string idCinema, string name)
        {
            IdCinema = idCinema;
            Name = name;
            Seats = new List<Seat>();
        }

        public string IdCinema { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int PriceCenter { get; set; }

        public List<Seat> Seats { get; set; }
    }
}
