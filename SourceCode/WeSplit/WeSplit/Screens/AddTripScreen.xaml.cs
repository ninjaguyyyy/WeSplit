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

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for AddTripScreen.xaml
    /// </summary>
    public partial class AddTripScreen : Window
    {
        public AddTripScreen()
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

        private static Color GetBlack(Color black)
        {
            return black;
        }

        private void menuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            
            mainPanel.Opacity = 0.8;
        }

        private void mainPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            menuToggleButton.IsChecked = false;
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void startDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentObject = sender as DatePicker;
            MessageBox.Show(currentObject.SelectedDate.ToString());
        }

        private void LV_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listTripScreen = new ListTripScreen();
            listTripScreen.Show();
            this.Close();
        }
    }
}
