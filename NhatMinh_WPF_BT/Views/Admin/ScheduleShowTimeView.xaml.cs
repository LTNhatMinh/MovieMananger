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
    /// Interaction logic for ScheduleShowTimeView.xaml
    /// </summary>
    public partial class ScheduleShowTimeView : UserControl
    {
        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        private Account _account;
        MainWindow _mainWindow;

        public ScheduleShowTimeView(MainWindow mainWindow, Account account)
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

        private void statusText_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var movie = (textBlock?.DataContext) as ScheduleShowTimeDto;

            if (movie != null)
            {
                textBlock.Text = movie.Status ? "Available" : "Unavailable";
            }
        }
        private void statusTextSchedule_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var movie = (textBlock?.DataContext) as ScheduleDto;

            if (movie != null)
            {
                textBlock.Text = movie.Status ? "Available" : "Unavailable";
            }
        }

        private void cbbMovie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            Movie movie = comboBox.SelectedItem as Movie;
            List<Schedule> schedules = unitOfWork.GetScheduleService().GetListingScheduleByMovieId(movie.Id);
            List<ScheduleDto> scheduleDto = unitOfWork.GetScheduleService().
                                             MapToScheduleDtos(schedules,
                                             unitOfWork.GetMovieService().movies.ToList(),
                                              unitOfWork.GetCinemaService().cinemas.ToList());

            dtgSchedule.ItemsSource = scheduleDto;
        }

        List<ScheduleShowTimeDto> scheduleShowTimeDtos;
        ScheduleDto schedule;
        private void dtgSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            schedule = dataGrid.SelectedItem as ScheduleDto;
            if (schedule == null)
                return;
            scheduleShowTimeDtos = unitOfWork.GetScheduleShowTimeService().GetScheduleShowTimeDtosWithScheduleId(
                                                             unitOfWork.GetCinemaService().cinemas.ToList(),
                                                             schedule.ScheduleId);

            dtgScheduleShowtime.ItemsSource = scheduleShowTimeDtos;
            if (scheduleShowTimeDtos.Count != 0)
            {
                string showtime = scheduleShowTimeDtos[scheduleShowTimeDtos.Count - 1].AirDate.
                            AddMinutes(scheduleShowTimeDtos[scheduleShowTimeDtos.Count - 1].Duration + Parameter.PreparationTime).ToString("HH:mm tt");

                string[] parts = showtime.Split(':');
                if (parts.Length == 2)
                {
                    string hourStr = parts[0];
                    string minuteStr = parts[1].Remove(2);


                    int hourIndex = cbbHour.Items.IndexOf(hourStr);
                    int minuteIndex = cbbMinute.Items.IndexOf(minuteStr);

                    if (hourIndex >= 0)
                        cbbHour.SelectedIndex = hourIndex;

                    if (minuteIndex >= 0)
                        cbbMinute.SelectedIndex = minuteIndex;
                }
            }
            else
            {
                cbbHour.SelectedIndex = 0;
                cbbMinute.SelectedIndex = 0;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ScheduleShowTimeDto checkSchedule = unitOfWork.GetScheduleShowTimeService().GetLastTimeScheduleShowTime(scheduleShowTimeDtos);

            if (cbbMovie.SelectedValue == null ||
                dtgSchedule.SelectedValue == null)
            {
                MessageBox.Show("Please choose film and schedule",
                                "Warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            DateTime airDate;
            int hour = int.Parse(cbbHour.Text);
            int minute = int.Parse(cbbMinute.Text);
            DateTime timeStart;

            if (checkSchedule != null)
            {
                airDate = checkSchedule.AirDate.Date;

                timeStart = new DateTime(airDate.Year, airDate.Month, airDate.Day, hour, minute, 0);

                if (unitOfWork.GetScheduleShowTimeService().CheckOutTheMovieShow(scheduleShowTimeDtos[scheduleShowTimeDtos.Count - 1]))
                {
                    MessageBox.Show("The projection rate is over the schedule time",
                                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!unitOfWork.GetScheduleShowTimeService().CheckTimeDuration(timeStart, checkSchedule))
                {
                    MessageBox.Show($"The next projection time must be greater than or equal " +
                                    $"{checkSchedule.AirDate.AddMinutes(checkSchedule.Duration + Parameter.PreparationTime).ToString("HH:mm tt")}",
                                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                airDate = schedule.AirDate.Date;

                timeStart = new DateTime(airDate.Year, airDate.Month, airDate.Day, hour, minute, 0);
            }

            int newId = int.Parse(unitOfWork.GetScheduleShowTimeService().GetLast().Id) + 1;
            ScheduleShowTime newScheduleShowTime = new ScheduleShowTime(newId.ToString(), schedule.ScheduleId, timeStart, true);
            unitOfWork.GetScheduleShowTimeService().Add(newScheduleShowTime);

            scheduleShowTimeDtos = unitOfWork.GetScheduleShowTimeService().GetScheduleShowTimeDtosWithScheduleId(
                                                         unitOfWork.GetCinemaService().cinemas.ToList(),
                                                         schedule.ScheduleId);

            dtgScheduleShowtime.ItemsSource = scheduleShowTimeDtos;
            MessageBox.Show("Add schedule successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int hour = 1; hour <= 24; hour++)
            {
                cbbHour.Items.Add(hour.ToString("D2"));
            }

            for (int minute = 0; minute < 60; minute++)
            {
                cbbMinute.Items.Add(minute.ToString("D2"));
            }

            cbbHour.SelectedIndex = 0;
            cbbMinute.SelectedIndex = 0;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new AdminView(_mainWindow, _account);
        }
    }
}
