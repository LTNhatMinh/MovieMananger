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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NhatMinh_WPF_BT.Views.Admin
{
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : UserControl
    {
        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        Account _account;
        MainWindow _mainWindow;

        public OrderView(MainWindow mainWindow, Account account)
        {
            InitializeComponent();
            _account = account;
            _mainWindow = mainWindow;
            LoadData();
        }

        private void LoadData()
        {
            cbbMovie.ItemsSource = unitOfWork.GetMovieService().GetMovieByStatus();
        }

        private void dtgOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            OrderDto orderDto = dtgOrder.SelectedItem as OrderDto;
            if (orderDto == null) return;

            Order order = unitOfWork.GetOrderService().orders.FirstOrDefault(ord => ord.Id.Contains(orderDto.IdOrder));


            dtgOrderDetail.ItemsSource = order.DetailOrders;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<OrderDto> orderDtos = unitOfWork.GetOrderService().GetOrderDtos(
                unitOfWork.GetOrderService().orders.ToList(),
                unitOfWork.GetScheduleShowTimeService().scheduleShowTimes.ToList(),
                unitOfWork.GetScheduleService().schedules.ToList(),
                unitOfWork.GetMovieService().movies.ToList());

            Movie movie = cbbMovie.SelectedItem as Movie;
            DateTime date = DateTime.MinValue;
            if (movie == null && dtpDate.SelectedDate == null)
                return;

            if (dtpDate.SelectedDate != null)
                date = dtpDate.SelectedDate.Value;

            if (movie != null && dtpDate.SelectedDate == null)
                dtgOrder.ItemsSource = unitOfWork.GetOrderService().GetListingOrderByMovieName(movie.Name, orderDtos);

            else if (movie == null && dtpDate.SelectedDate != null)
            {
                List<Order> ordersByDate = unitOfWork.GetOrderService().GetListingOrderByDate(date);
                if (ordersByDate == null || orderDtos == null) return;

                orderDtos = unitOfWork.GetOrderService().GetOrderDtos(ordersByDate,
                            unitOfWork.GetScheduleShowTimeService().scheduleShowTimes.ToList(),
                            unitOfWork.GetScheduleService().schedules.ToList(),
                            unitOfWork.GetMovieService().movies.ToList());

                dtgOrder.ItemsSource = orderDtos;
            }
            else
            {
                List<OrderDto> searchByNameAndDate = unitOfWork.GetOrderService().GetListingOrderByMovieNameAndDate(movie.Name, date, orderDtos);
                dtgOrder.ItemsSource = searchByNameAndDate;
            }
            dtgOrderDetail.ItemsSource = null;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbbMovie.Text = string.Empty;
            dtpDate.Text = string.Empty;

            dtgOrder.ItemsSource = null;
            dtgOrderDetail.ItemsSource = null;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new AdminView(_mainWindow, _account);
        }
    }
}
