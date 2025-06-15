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

namespace NhatMinh_WPF_BT
{
    /// <summary>
    /// Interaction logic for CinemaView.xaml
    /// </summary>
    public partial class CinemaView : UserControl
    {
        TextBlock[] arrayTextBlocks;

        private static UnitOfWork unitOfWork = UnitOfWork.Instance;
        private Account _account;
        MainWindow _mainWindow;

        public CinemaView(MainWindow mainWindow, Account account)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _account = account;
            txtUserName.Text += _account.Name;
            OutputCinema();
        }

        private void OutputCinema()
        {
            List<Cinema> cinemas = unitOfWork.GetCinemaService().cinemas.ToList();

            arrayTextBlocks = new TextBlock[cinemas.Count];
            WrapPanel wrapPanel = new WrapPanel();
            for (int i = 0; i < arrayTextBlocks.Length; i++)
            {
                arrayTextBlocks[i] = new TextBlock
                {
                    Text = $"Name: {cinemas[i].Name}\n" +
                           $"Type: {cinemas[i].Type}\n" +
                           $"Price: {cinemas[i].PriceCenter}",
                    Margin = new Thickness(5),
                    Foreground = Brushes.DarkSlateBlue,
                    TextWrapping = TextWrapping.Wrap
                };

                Border border = new Border
                {
                    Child = arrayTextBlocks[i],
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.Black,
                    Padding = new Thickness(8),
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(5),
                    Background = Brushes.LightYellow
                };
                wrapPanel.Children.Add(border);
            }
            cinemaGrb.Content = wrapPanel;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Content = new AdminView(_mainWindow, _account);
        }
    }
}
