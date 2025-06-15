using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
    /// Interaction logic for
    /// .xaml
    /// </summary>
    public partial class ScheduleView : UserControl
    {
        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        private Account _account;
        MainWindow _mainWindow;
        public ScheduleView(MainWindow mainWindow, Account account)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _account = account;
            txtUserName.Text += _account.Name;
            LoadData();
        }

        private void LoadData()
        {
            List<Schedule> schedules = unitOfWork.GetScheduleService().schedules.ToList();
            List<ScheduleDto> scheduleDto = unitOfWork.GetScheduleService().
                MapToScheduleDtos(schedules,
                unitOfWork.GetMovieService().movies.ToList(),
                unitOfWork.GetCinemaService().cinemas.ToList());

            cbbMovie.ItemsSource = unitOfWork.GetMovieService().GetMovieByStatus();
            cbbCinema.ItemsSource = unitOfWork.GetCinemaService().cinemas;

            scheduleDtg.ItemsSource = scheduleDto;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (btnAdd.Content.ToString() == "Save")
            {
                btnAdd.Click += btnAdd_Click;
                btnAdd.Content = "Add";
                cbbCinema.IsEnabled = false;
                cbbMovie.IsEnabled = false;
                dtpStartDate.IsEnabled = false;

                txtSchedule.Text = string.Empty;
                cbbCinema.Text = string.Empty;
                cbbMovie.Text = string.Empty;
                dtpStartDate.SelectedDate = DateTime.UtcNow;

                btnAdd.Click -= Save;
            }
            else
                _mainWindow.Content = new AdminView(_mainWindow, _account);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnAdd.Click -= btnAdd_Click;
            btnAdd.Content = "Save";
            cbbCinema.IsEnabled = true;
            cbbMovie.IsEnabled = true;
            dtpStartDate.IsEnabled = true;

            cbbCinema.Text = string.Empty;
            cbbMovie.Text = string.Empty;
            dtpStartDate.SelectedDate = DateTime.UtcNow;
            int newId = int.Parse(unitOfWork.GetScheduleService().GetLast().Id) + 1;
            txtSchedule.Text = newId.ToString();
            btnAdd.Click += Save;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (cbbCinema.SelectedValue == null ||
            cbbMovie.SelectedValue == null ||
            txtSchedule.Text == string.Empty)
            {
                MessageBox.Show(
                $"Please choose full data",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
                return;

            }

            if (dtpStartDate.SelectedDate.Value.Date <= DateTime.UtcNow.Date)
            {
                MessageBox.Show(
                $"Please choose a bigger date or equal {DateTime.UtcNow.AddDays(1).ToString("dd/MM/yyyy")}",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
                );
                return;
            }
            int newId = int.Parse(unitOfWork.GetScheduleService().GetLast().Id) + 1;


            Movie movie = unitOfWork.GetMovieService().GetMovieByName((cbbMovie.SelectedItem as Movie).Name);
            string cinemaId = unitOfWork.GetCinemaService().GetCinemaIdByName(cbbCinema.SelectedValue.ToString());

            if (unitOfWork.GetScheduleService().isExistSchedule(cinemaId, movie.Id, dtpStartDate.SelectedDate.Value))
            {
                MessageBox.Show("This schedule is existed",
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
                return;
            }

            Schedule newSchedule = new Schedule(newId.ToString(), movie.Id, cinemaId, dtpStartDate.SelectedDate.Value, true);
            unitOfWork.GetScheduleService().Add(newSchedule);
            LoadData();
            MessageBox.Show("Add schedule successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            cbbCinema.Text = string.Empty;
            cbbMovie.Text = string.Empty;
            dtpStartDate.SelectedDate = DateTime.UtcNow;
            txtSchedule.Text = (newId + 1).ToString();
        }

        private void scheduleDtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;

            if (dataGrid.SelectedItem is ScheduleDto selectedSchedule)
            {
                cbbMovie.SelectedValue = selectedSchedule.MovieName;
                txtSchedule.Text = selectedSchedule.ScheduleId;
                cbbCinema.SelectedValue = selectedSchedule.CinemaName;
                dtpStartDate.SelectedDate = selectedSchedule.AirDate;
            }
        }

        private void statusText_Loaded(object sender, RoutedEventArgs e)
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
            if (movie == null) return;

            if (btnAdd.Content == "Add")
            {
                txtSchedule.Text = string.Empty;
                cbbCinema.Text = string.Empty;
                dtpStartDate.Text = string.Empty;
            }

            List<Schedule> schedules = unitOfWork.GetScheduleService().GetListingScheduleByMovieId(movie.Id);
            List<ScheduleDto> scheduleDtos = unitOfWork.GetScheduleService().
                MapToScheduleDtos(schedules,
                unitOfWork.GetMovieService().movies.ToList(),
                unitOfWork.GetCinemaService().cinemas.ToList());
            scheduleDtg.ItemsSource = scheduleDtos;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbbMovie.Text = string.Empty;
            txtSchedule.Text = string.Empty;
            cbbCinema.Text = string.Empty;
            dtpStartDate.Text = string.Empty;

            List<Schedule> schedules = unitOfWork.GetScheduleService().schedules.ToList();
            List<ScheduleDto> scheduleDto = unitOfWork.GetScheduleService().
                MapToScheduleDtos(schedules,
                unitOfWork.GetMovieService().movies.ToList(),
                unitOfWork.GetCinemaService().cinemas.ToList());
            scheduleDtg.ItemsSource = scheduleDto;
        }
    }
}
