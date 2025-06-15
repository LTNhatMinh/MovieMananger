using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class CinemaService
    {
        private static UnitOfWork _unitOfWork = UnitOfWork.Instance;
        public ObservableCollection<Cinema> cinemas;

        public CinemaService()
        {
            cinemas = new ObservableCollection<Cinema>(_unitOfWork.CinemaRepository.Gets());
        }

        public Cinema GetCinemaById(string id)
        {
            return cinemas.FirstOrDefault(c => c.IdCinema.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public int GetIndexSeatBySeatName(string seatName, List<Seat> seats)
        {
            for (int i = 0; i < seats.Count; i++)
            {
                if (seats[i].Name.Contains(seatName.ToUpper()))
                    return i;
            }
            return -1;
        }

        public string GetIndexSeatBySeatId(int seatId, List<Seat> seats)
        {

            for (int i = 0; i < seats.Count; i++)
            {
                if (i == seatId)
                    return seats[i].Name;
            }

            return string.Empty;
        }

        public bool isExistSeat(string seatName, List<Seat> seats, List<BoughtSeat> boughtSeats)
        {
            int indexSeat = GetIndexSeatBySeatName(seatName, seats);

            if (indexSeat == -1)
                return true;
            if (seatName.Length != 2)
                return true;

            if (indexSeat >= 0)
            {
                for (int i = 0; i < boughtSeats.Count; i++)
                {
                    if ((int.Parse(boughtSeats[i].SeatNo) - 1).ToString()
                        .Equals(indexSeat.ToString()))
                        return true;
                }
            }
            return false;
        }

        public string GetCinemaTypeById(string idCemina)
        {
            return cinemas.FirstOrDefault(c => c.IdCinema.Contains(idCemina)).Type;
        }

        public string GetCinemaIdByName(string name)
        {
            return cinemas.FirstOrDefault(c => c.Name.Contains(name)).IdCinema;
        }
    }
}
