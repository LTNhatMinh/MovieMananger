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
using NhatMinh_WPF_BT.Views.User;

namespace NhatMinh_WPF_BT.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private CheckTimeMovieAndSchedule CheckTimeMovieAndSchedule = new CheckTimeMovieAndSchedule();
        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        MainWindow _mainWindow;

        public LoginView(MainWindow mainWindow)
        {
            InitializeComponent();
            CheckTimeMovieAndSchedule.CheckTheDeadline(unitOfWork.GetMovieService().movies.ToList(), unitOfWork.GetScheduleService().schedules.ToList(),
                                                        unitOfWork.GetScheduleShowTimeService().scheduleShowTimes.ToList());

            _mainWindow = mainWindow;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Password;
            Account account = unitOfWork.GetAccountService().isExistAccountByUsernamePassword(userName, password);
            if (account != null)
            {

                if (account.Role == "Admin")
                    _mainWindow.Content = new AdminView(_mainWindow, account);
                else
                    _mainWindow.Content = new UserView(_mainWindow, account);
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }

            txtUserName.Text = string.Empty;
            txtPassword.Password = string.Empty;
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnLogin_Click(sender, e);
        }
    }
}
