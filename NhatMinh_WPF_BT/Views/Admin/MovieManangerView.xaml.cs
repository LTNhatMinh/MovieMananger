using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace NhatMinh_WPF_BT
{
    /// <summary>
    /// Interaction logic for MovieManangerView.xaml
    /// </summary>
    public partial class MovieManangerView : UserControl
    {
        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        MainWindow _mainWindow;
        private Account _account;

        public MovieManangerView(MainWindow mainWindow, Account account)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _account = account;
            txtUserName.Text += _account.Name;
            LoadData();
        }

        private void LoadData()
        {
            List<Movie> movies = unitOfWork.GetMovieService().movies.ToList();

            movieDtg.ItemsSource = movies;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new AdminView(_mainWindow, _account);
        }

        private void statusText_Loaded(object sender, RoutedEventArgs e)
        {
            var textBlock = sender as TextBlock;
            var movie = (textBlock?.DataContext) as Movie;

            if (movie != null)
            {
                textBlock.Text = movie.Status ? "Available" : "Unavailable";
            }
        }

        private void LockButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var movie = button?.Tag as Movie;

            if (movie != null)
            {
                movie.Status = false;
                movieDtg.Items.Refresh();
            }
        }
    }
}
