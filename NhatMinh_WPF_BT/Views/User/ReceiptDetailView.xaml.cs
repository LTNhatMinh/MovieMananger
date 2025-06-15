using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NhatMinh_WPF_BT.Views.User
{
    /// <summary>
    /// Interaction logic for ReceiptDetailView.xaml
    /// </summary>
    public partial class ReceiptDetailView : Window
    {
        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        private MainWindow _mainWindow;
        private Account _account;
        private Order _order;
        private Cinema _cinema;
        private string _cinemaType;
        private string _customerName;
        private string _customerPhone;
        private List<string> _listSeat;
        public bool IsBought { get; private set; } = false;

        public ReceiptDetailView(MainWindow mainWindow, Account account,
            List<string> seatList, string cinemaType,
            string customerName, string customerPhone,
            Order order, Cinema cinema)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _account = account;
            _order = order;
            _cinema = cinema;
            _cinemaType = cinemaType;
            _customerName = customerName;
            _customerPhone = customerPhone;
            _listSeat = seatList;
            dtgReceiptDetail.ItemsSource = order.DetailOrders;
        }

        private void dtgReceiptDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DetailOrder detailOrder = dataGrid.SelectedItem as DetailOrder;
            if (detailOrder == null) return;

            int price = (_order.CinemaType.Contains("Vip")) ? (Parameter.CinemaVipPrice + Parameter.CinemaVipSurcharges) : Parameter.CinemaStandardPrice;
            var ageValue = detailOrder.GetType().GetProperty("Age")?.GetValue(detailOrder, null);
            int discount = (ageValue.ToString() == "Adult") ? 0 : (int)(price * (Parameter.CinemaStandardKidDiscount / 100.0));

            detailOrder.Age = ageValue.ToString();
            detailOrder.Discount = discount;
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            _order.Total = unitOfWork.GetOrderService().TotalOrderDetail(_order);
            TicketWindow ticketWindow = new TicketWindow(_listSeat, _order.CinemaType,
                                                       _customerName, _customerPhone, _order, _cinema);

            ticketWindow.ShowDialog();

            if (ticketWindow.IsBought)
            {
                IsBought = true;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsBought = false;
            this.Close();
        }
    }
}
