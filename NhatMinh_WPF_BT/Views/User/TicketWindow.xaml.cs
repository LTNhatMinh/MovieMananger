using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NhatMinh_WPF_BT.Views.User
{
    /// <summary>
    /// Interaction logic for TicketWindow.xaml
    /// </summary>
    public partial class TicketWindow : Window
    {
        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        private Order _order;
        private Cinema _cinema;

        public bool IsBought { get; private set; } = false;

        public TicketWindow(List<string> seatList, string cinemaType,
                            string customerName, string customerPhone,
                            Order order, Cinema cinema)
        {
            _order = order;
            _cinema = cinema;
            InitializeComponent();

            int totalSeats = order.DetailOrders.Count;
            string totalFormatted = order.Total.ToString("N0");

            var childSeats = order.DetailOrders
               .Where(d => d.Age == "Child")
               .Select(d => unitOfWork.GetCinemaService().GetIndexSeatBySeatId(d.SeatNo, _cinema.Seats))
               .ToList();

            var adultSeats = order.DetailOrders
                .Where(d => d.Age == "Adult")
                .Select(d => unitOfWork.GetCinemaService().GetIndexSeatBySeatId(d.SeatNo, _cinema.Seats))
                .ToList();


            string seatDisplay = $"Child: {string.Join(", ", childSeats)}\nAdult: {string.Join(", ", adultSeats)}";

            List<string> paymentParts = new List<string>();
            foreach (var detail in order.DetailOrders)
            {
                decimal finalPrice = detail.Price - detail.Discount;
                paymentParts.Add(finalPrice.ToString("N0"));
            }

            string joinedPayments = string.Join(" + ", paymentParts);

            txtTotalSeat.Text = totalSeats.ToString();
            txtCustomerName.Text = customerName;
            txtPhone.Text = customerPhone;
            txtSeats.Text = seatDisplay;
            txtTotalPayment.Text = totalFormatted;
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            unitOfWork.GetOrderService().Add(_order);
            IsBought = true;
            this.Close();
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsBought = false;
            this.Close();
        }
    }
}
