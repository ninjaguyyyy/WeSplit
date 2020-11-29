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

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for ListTripScreen.xaml
    /// </summary>
    public partial class ListTripScreen : Window
    {
        public ListTripScreen()
        {
            InitializeComponent();
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
            var test = TripDAO.GetTrips();
            tripsListView.ItemsSource = test;
        }


        private void addTripListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var addTripScreen = new AddTripScreen();
            addTripScreen.Show();

            this.Close();
        }

        private void tripListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var that = sender as StackPanel;
            var id = that.Tag;
            MessageBox.Show(id.ToString());
            var detailTripScreen = new DetailTripScreen();
            detailTripScreen.Show();
        }
    }
}
