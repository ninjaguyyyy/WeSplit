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
using LiveCharts.Helpers;

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


            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");

            
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

            DisplayDetail();
            DisplayDonationChart();
            DisplayCostChart();

        }

        private void DisplayDetail()
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
                    Status = trip.Status == "finish" ? "False" : "True",
                    NameButtonStatus = trip.Status == "finish" ? "Đã kết thúc" : "Kết thúc"
                },
                PointLabel,
                SeriesCollection
            };

            membersListView.ItemsSource = trip.Members;
            expensesListView.ItemsSource = trip.Expenses;
            donationsListView.ItemsSource = trip.Members;
            placesListView.ItemsSource = trip.Places;
        }

        private void DisplayDonationChart()
        {
            SeriesCollection series = new SeriesCollection();
            foreach (var member in trip.Members)
            {
                int donation = int.Parse(member.Donation);
                PieSeries pieSeries = new PieSeries
                {
                    Title = member.Name,
                    Values = new ChartValues<int> { donation },
                    DataLabels = true,
                    LabelPoint = PointLabel
                };
                series.Add(pieSeries);
            }
            donationPie.Series = series;
        }

        private void DisplayCostChart()
        {
            SeriesCollection series = new SeriesCollection();
            foreach (var expense in trip.Expenses)
            {
                int cost = int.Parse(expense.Cost);
                PieSeries pieSeries = new PieSeries
                {
                    Title = expense.Name,
                    Values = new ChartValues<int> { cost },
                    DataLabels = true,
                    LabelPoint = PointLabel
                };
                series.Add(pieSeries);
            }
            costPie.Series = series;
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
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void addPlaceButton_Click(object sender, RoutedEventArgs e)
        {
            var addPlaceDialog = new AddPlaceDialog();

            if (addPlaceDialog.ShowDialog() == true)
            {
                var newplace = addPlaceDialog.NewPlace;
                if (trip.Places == null)
                {
                    trip.Places = new List<Place>();
                }
                trip.Places.Add(newplace);

                placesListView.ItemsSource = null;
                placesListView.ItemsSource = trip.Places;

                TripDAO.InsertPlaces(idTrip, newplace);
            }


        }

        private void deleteMemberButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var idMember = ((Image)sender).Uid;
            TripDAO.RemoveMember(idTrip, idMember);
            DisplayDetail();
            MessageBox.Show("Đã xóa thành viên thành công.", "Thông báo");

        }

        private void editMemberButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var idMember = ((Image)sender).Uid;
            var memberCurrent = TripDAO.GetMemberById(idTrip, idMember);

            var addMemberDialog = new AddMemberDialog(memberCurrent);

            if (addMemberDialog.ShowDialog() == true)
            {
                var newMember = addMemberDialog.NewMember;

                TripDAO.RemoveMember(idTrip, idMember);
                TripDAO.InsertMember(idTrip, newMember);
                DisplayDetail();
                DisplayDonationChart();
                MessageBox.Show("Đã cập nhật thành công!");
            }
        }

        private void addMemberButton_Click(object sender, RoutedEventArgs e)
        {
            var addMemberDialog = new AddMemberDialog();

            if (addMemberDialog.ShowDialog() == true)
            {
                var newMember = addMemberDialog.NewMember;
                
                trip.Members.Add(newMember);
                TripDAO.InsertMember(idTrip, newMember);
                DisplayDetail();
                DisplayDonationChart();
                
            }
        }

        private void removePlaceBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var idPlace = ((TextBlock)sender).Uid;
            TripDAO.RemovePlace(idTrip, idPlace);
            DisplayDetail();
            MessageBox.Show("Đã xóa điểm dừng thành công.", "Thông báo");
        }

        private void editPlaceBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var idPlace = ((TextBlock)sender).Uid;
            var placeCurrent = TripDAO.GetPlaceById(idTrip, idPlace);

            var addPlaceDialog = new AddPlaceDialog();
            addPlaceDialog.NewPlace = placeCurrent;

            if (addPlaceDialog.ShowDialog() == true)
            {
                var newPlace = addPlaceDialog.NewPlace;

                TripDAO.RemovePlace(idTrip, idPlace);
                TripDAO.InsertPlaces(idTrip, newPlace);
                DisplayDetail();
                MessageBox.Show("Đã cập nhật thành công!");
            }
        }

        private void addExpenseBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var addExpenseDialog = new AddExpenseDialog();

            if (addExpenseDialog.ShowDialog() == true)
            {
                var newExpense = addExpenseDialog.NewExpense;
                trip.Expenses.Add(newExpense);

                TripDAO.InsertExpense(idTrip, newExpense);
                DisplayDetail();
                DisplayCostChart();
            }
        }

        private void editExpenseBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void removeExpenseBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var idExpense = ((Image)sender).Uid;
            TripDAO.RemoveExpense(idTrip, idExpense);
            DisplayDetail();
            DisplayCostChart();
            MessageBox.Show("Đã xóa chi phí thành công.", "Thông báo");
        }
    }
}
