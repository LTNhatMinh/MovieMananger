using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class CinemaStandard : Cinema, IDiscount
    {
        public int Disount => Parameter.CinemaStandardKidDiscount;

        public CinemaStandard(string idCinema, string name) : base(idCinema, name)
        {
            PriceCenter = Parameter.CinemaStandardPrice;
            Type = Parameter.CinemaType[0];
        }
    }
}
