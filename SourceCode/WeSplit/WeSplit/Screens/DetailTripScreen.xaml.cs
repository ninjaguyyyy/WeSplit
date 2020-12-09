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
using WeSplit.DAO;
using WeSplit.DTO;
using LiveCharts;
using LiveCharts.Wpf;

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for DetailTripScreen.xaml
    /// </summary>
    public partial class DetailTripScreen : Window
    {
        private string idTrip;
        private Trip trip;

        public DetailTripScreen(string id)
        {
            InitializeComponent();
            idTrip = id;

            PointLabel = chartPoint =>
                string.Format("{0}", chartPoint.Y);

            
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (menuToggleButton.IsChecked == true)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_adding.Visibility = Visibility.Collapsed;
                tt_settings.Visibility = Visibility.Collapsed;
                tt_info.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt_home.Visibility = Visibility.Visible;
                tt_adding.Visibility = Visibility.Visible;
                tt_settings.Visibility = Visibility.Visible;
                tt_info.Visibility = Visibility.Visible;
            }
        }

        private void menuToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            mainPanel.Opacity = 1;
        }

        private void menuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            mainPanel.Opacity = 0.3;
        }

        private void mainPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuToggleButton.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            trip = TripDAO.GetById(idTrip);
            this.DataContext = new
            {
                Trip = new
                {
                    Name = trip.Name,
                    StartDate = trip.StartDate,
                    EndDate = trip.EndDate,
                    MainImage = trip.MainImage,
                    Transport = TransportDAO.GetById(trip.Transport),
                    Status = trip.Status == "finish" ? "False": "True",
                    NameButtonStatus = trip.Status == "finish" ? "Đã kết thúc" : "Kết thúc"
                },
                PointLabel
            };
        }

        private void finishButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void showExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            membersListView.Visibility = Visibility.Collapsed;
            expensesListView.Visibility = Visibility.Visible;
            donationsListView.Visibility = Visibility.Collapsed;
        }

        private void showDonationsButton_Click(object sender, RoutedEventArgs e)
        {
            membersListView.Visibility = Visibility.Collapsed;
            expensesListView.Visibility = Visibility.Collapsed;
            donationsListView.Visibility = Visibility.Visible;
        }

        private void showMembersButton_Click(object sender, RoutedEventArgs e)
        {
            membersListView.Visibility = Visibility.Visible;
            expensesListView.Visibility = Visibility.Collapsed;
            donationsListView.Visibility = Visibility.Collapsed;
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

    }
}
