using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    internal interface ISetBoughtSeatsRepository
    {
        void SetBoughtSeats(string idSchedule, List<BoughtSeat> boughtSeats);
    }
}
