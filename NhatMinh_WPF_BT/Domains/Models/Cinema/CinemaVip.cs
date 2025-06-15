using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class CinemaVip : Cinema
    {
        private int Surcharges = Parameter.CinemaVipSurcharges;

        public CinemaVip(string idCinema, string name) : base(idCinema, name)
        {
            PriceCenter = Parameter.CinemaVipPrice;
            Type = Parameter.CinemaType[1];
        }
    }
}
