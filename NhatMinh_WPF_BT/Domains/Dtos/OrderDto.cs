using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class OrderDto
    {
        public string IdOrder { get; set; }
        public string MovieName { get; set; }
        public string CinemaType { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }

        public OrderDto(string idOrder, string movieName, string cinemaType, string customerName, string phoneNumber, DateTime date, double total)
        {
            IdOrder = idOrder;
            MovieName = movieName;
            CinemaType = cinemaType;
            CustomerName = customerName;
            PhoneNumber = phoneNumber;
            Date = date;
            Total = total;
        }
    }
}
