using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl
    {
        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        private MainWindow _mainWindow;
        private Account _account;
        private List<string> listSeat = new List<string>();
        private List<string> paymentList = new List<string>();
        private ScheduleShowTime scheduleShowTimeById;
        private Order order;
        private List<BoughtSeat> boughtSeatList;
        private Schedule schedule;
        private ScheduleShowTime scheduleShowTime;
        private ScheduleShowTimeDto scheduleShowTimeDto;
        private Cinema cinema;
        List<ScheduleShowTimeDto> scheduleShowTimeDtosByMovieName;
        List<ScheduleShowTimeDto> scheduleShowTimeDtos;
        Button[] arrayTextBlocks;

        public UserView(MainWindow mainWindow, Account account)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _account = account;
            txtUserName.Text += _account.Name;
            LoadData();
        }

        private void LoadData()
        {
            cbbMovie.ItemsSource = cbbMovie.ItemsSource = unitOfWork.GetMovieService().GetMovieByStatus(); ;
        }

        private void status_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var movie = (textBlock?.DataContext) as ScheduleShowTimeDto;

            if (movie != null)
            {
                textBlock.Text = movie.Status ? "Available" : "Unavailable";
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Movie movie = cbbMovie.SelectedItem as Movie;
            List<ScheduleShowTimeDto> scheduleShowTimeDtosByMovieName = null;
            scheduleShowTimeDtos = unitOfWork.GetScheduleShowTimeService()
                         .GetScheduleShowTimeDtos(unitOfWork.GetCinemaService().cinemas.ToList());

            if (string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
            string.IsNullOrWhiteSpace(txtCustomerPhone.Text))
            {
                MessageBox.Show("Please enter Customer Name and Customer Phone!", "Warning",
                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(txtCustomerPhone.Text, @"^\d{1}$"))
            {
                MessageBox.Show("Enter the correct number format in Customer Phone", "Error",
                MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }


            if (movie == null && dtpDate.SelectedDate != null)
            {
                DateTime date = dtpDate.SelectedDate.Value;
                scheduleShowTimeDtosByMovieName = unitOfWork.GetScheduleShowTimeService().GetListingScheduleShowTimeByDate(date, scheduleShowTimeDtos);
            }
            else if (movie != null && dtpDate.SelectedDate == null)
            {
                scheduleShowTimeDtosByMovieName = unitOfWork.GetScheduleShowTimeService().GetListingScheduleShowTimeByMovieName(movie.Name, scheduleShowTimeDtos);
            }
            else
            {
                dtgScheduleShowtime.ItemsSource = scheduleShowTimeDtos;
                return;
            }

            dtgScheduleShowtime.ItemsSource = scheduleShowTimeDtosByMovieName;
        }

        private void SetOutputBtn(string Name, int i)
        {
            bool isBought = unitOfWork.GetOrderService().isExistBoughtSeat(boughtSeatList, i);

            var button = new Button
            {
                Content = $"{Name}",
                Foreground = isBought ? Brushes.Red : Brushes.DarkSlateBlue,
                IsEnabled = !isBought,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Tag = i
            };

            button.Click += ButtonClick;

            arrayTextBlocks[i] = button;

            Border border = new Border
            {
                Width = 70,
                Height = 40,
                Child = arrayTextBlocks[i],
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Black,
                Padding = new Thickness(0),
                Margin = new Thickness(5),
                CornerRadius = new CornerRadius(5),
                Background = Brushes.LightYellow
            };
            wrpCinema.Children.Add(border);
        }

        private void dtgScheduleShowtime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            wrpCinema.Children.Clear();
            DataGrid dataGrid = sender as DataGrid;

            scheduleShowTimeDto = dataGrid.SelectedItem as ScheduleShowTimeDto;
            if (scheduleShowTimeDto == null) return;

            List<Cinema> cinemas = unitOfWork.GetCinemaService().cinemas.ToList();
            List<ScheduleShowTimeDto> scheduleShowTimeDtos = unitOfWork.GetScheduleShowTimeService()
                        .GetScheduleShowTimeDtos(unitOfWork.GetCinemaService().cinemas.ToList());

            scheduleShowTime = unitOfWork.GetScheduleShowTimeService().GetScheduleShowTimeById(scheduleShowTimeDto.Id);
            unitOfWork.GetScheduleShowTimeService().GetScheduleShowTimeByScheduleId(scheduleShowTimeDto.ScheduleId,
            unitOfWork.GetScheduleShowTimeService().scheduleShowTimes.ToList());


            schedule = unitOfWork.GetScheduleService().GetScheduleById(scheduleShowTimeDto.ScheduleId);
            scheduleShowTimeById = unitOfWork.GetScheduleShowTimeService()
                                    .GetScheduleShowTimeByScheduleId(scheduleShowTimeDto.ScheduleId,
                                    unitOfWork.GetScheduleShowTimeService().scheduleShowTimes.ToList());
            if (scheduleShowTimeById == null) return;

            Cinema cinema = unitOfWork.GetCinemaService().GetCinemaById(scheduleShowTimeDto.CinemaId);
            boughtSeatList = new List<BoughtSeat>(scheduleShowTimeById.BoughtSeats);
            arrayTextBlocks = new Button[cinema.Seats.Count];

            for (int i = 0; i < arrayTextBlocks.Length; i++)
            {
                SetOutputBtn(cinema.Seats[i].Name, i);
            }

            string scheduleId = scheduleShowTimeDto.ScheduleId;
            string cinemaType = unitOfWork.GetCinemaService()
                                .GetCinemaTypeById(scheduleShowTimeDto.CinemaId);

            order = new Order((unitOfWork.GetOrderService().orders.Count + 1).ToString(),
            scheduleId, cinemaType, txtCustomerName.Text, txtCustomerPhone.Text, DateTime.Now, 0);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            int seatNo = (int)button.Tag;
            cinema = unitOfWork.GetCinemaService().GetCinemaById(scheduleShowTimeDto.CinemaId);
            string seatName = cinema.Seats[seatNo].Name;

            var existingDetail = order.DetailOrders.FirstOrDefault(d => d.SeatNo == seatNo);

            if (existingDetail != null)
            {
                order.DetailOrders.Remove(existingDetail);
                boughtSeatList.RemoveAll(b => b.SeatNo == seatNo.ToString());

                listSeat.Remove(seatName.ToUpper());
                paymentList.Remove(existingDetail.Price.ToString());

                button.Foreground = Brushes.DarkSlateBlue;
                button.Background = Brushes.Transparent;
            }
            else
            {
                int price = (order.CinemaType.Contains("Vip")) ? (Parameter.CinemaVipPrice + Parameter.CinemaVipSurcharges) : Parameter.CinemaStandardPrice;
                string ageStr = "Adult";
                int discount = (ageStr == "Adult" || order.CinemaType.Contains("Vip")) ? 0 : (int)(price * (Parameter.CinemaStandardKidDiscount / 100.0));

                DetailOrder detailOrder = new DetailOrder("Adult", seatNo, price, discount);
                order.DetailOrders.Add(detailOrder);
                boughtSeatList.Add(new BoughtSeat((seatNo).ToString()));

                listSeat.Add(seatName.ToUpper());
                paymentList.Add(detailOrder.Price.ToString());

                button.Foreground = Brushes.Black;
                button.Background = Brushes.Red;
            }
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {

            if (order == null || order.DetailOrders.Count == 0) return;

            scheduleShowTime.BoughtSeats = boughtSeatList;
            ReceiptDetailView receiptDetailView = new ReceiptDetailView(_mainWindow,
                              _account, listSeat, order.CinemaType,
                              txtCustomerName.Text,
                              txtCustomerPhone.Text, order, cinema);

            receiptDetailView.ShowDialog();

            if (receiptDetailView.IsBought)
            {
                MessageBox.Show("Buy successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                unitOfWork.GetScheduleShowTimeService()
                            .SetBoughtSeats(scheduleShowTimeById.Id, scheduleShowTime.BoughtSeats);

                listSeat = new List<string>();
                paymentList = new List<string>();
                wrpCinema.Children.Clear();

                dtgScheduleShowtime.ItemsSource = null;
                if (scheduleShowTimeDtosByMovieName == null)
                    dtgScheduleShowtime.ItemsSource = scheduleShowTimeDtos;
                else
                    dtgScheduleShowtime.ItemsSource = scheduleShowTimeDtosByMovieName;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbbMovie.Text = string.Empty;
            dtpDate.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtCustomerPhone.Text = string.Empty;
            dtgScheduleShowtime.ItemsSource = null;
            wrpCinema.Children.Clear();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new LoginView(_mainWindow);
        }
    }
}
