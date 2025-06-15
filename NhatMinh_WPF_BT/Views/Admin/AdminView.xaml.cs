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
using NhatMinh_WPF_BT.Views;
using NhatMinh_WPF_BT.Views.Admin;

namespace NhatMinh_WPF_BT
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {
        private MainWindow _mainWindow;
        private Account _account;

        public AdminView(MainWindow mainWindow, Account account)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _account = account;
        }

        private void btnMovieManager_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new MovieManangerView(_mainWindow, _account);
        }

        private void btnListCinema_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new CinemaView(_mainWindow, _account);
        }

        private void btnScheduleManager_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new ScheduleView(_mainWindow, _account);
        }

        private void btnScheduleShowtime_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new ScheduleShowTimeView(_mainWindow, _account);
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new OrderView(_mainWindow, _account);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new LoginView(_mainWindow);
        }
    }
}
