using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatMinh_WPF_BT
{
    public class Order
    {
        public string Id { get; set; }
        public string IdScheduleShowTime { get; set; }
        public string CinemaType { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }

        public ObservableCollection<DetailOrder> DetailOrders { get; set; } = new ObservableCollection<DetailOrder>();

        public Order(string id, string idScheduleShowTime, string cinemaType, string customerName, string phoneNumber, DateTime date, double total)
        {
            Id = id;
            IdScheduleShowTime = idScheduleShowTime;
            CinemaType = cinemaType;
            CustomerName = customerName;
            PhoneNumber = phoneNumber;
            Date = date;
            Total = total;
        }
    }

}
