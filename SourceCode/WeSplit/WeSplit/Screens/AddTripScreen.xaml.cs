using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
using System.Windows.Controls.Primitives;

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for AddTripScreen.xaml
    /// </summary>
    public partial class AddTripScreen : Window
    {

        private List<TypeTrip> typeTrips = new List<TypeTrip>();
        private List<Transport> transports = new List<Transport>();
        private TypeTrip typeChosen;
        private Transport transportChosen;
        private string nameImage = "default_trip.jpg";
        private string fileUploadPath;
        private string status = "progress";
        private List<Member> members = new List<Member>();
        private List<Expense> expenses = new List<Expense>();
        private List<Place> places = new List<Place>();
        private List<ImageTrip> images = new List<ImageTrip>();

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
            Application.Current.Shutdown();
        }


        private void LV_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var listTripScreen = new ListTripScreen();
            listTripScreen.Show();
            this.Close();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            var startDate = startDatePicker.SelectedDate;
            var endDate = endDatePicker.SelectedDate;
            if (name == "")
            {
                MessageBox.Show("Chưa điền đầy đủ thông tin", "Lỗi");
                return;
            }

            var tripEntered = new Trip()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Status = status,
                StartDate = (startDate == null)? "Chưa rõ": ((DateTime) startDate).ToString("dd/MM/yyyy"),
                EndDate = (endDate == null) ? "Chưa rõ" : ((DateTime) endDate).ToString("dd/MM/yyyy"),
                MainImage = nameImage,
                Type = typeChosen,
                Transport = transportChosen.Id,
                Members = members,
                Expenses = expenses,
                Places = places,
                Images = images
            };

            var resultAdded = TripDAO.InsertTrip(tripEntered);

            MessageBox.Show("Đã thêm thành công", "Thông báo");

        }

        private void workTypeButton_Click(object sender, RoutedEventArgs e)
        {
            typeChosen = typeTrips[1];
            this.DataContext = new { type = typeChosen, transport = transportChosen };
        }

        private void travelTypeButton_Click(object sender, RoutedEventArgs e)
        {
            typeChosen = typeTrips[0];
            this.DataContext = new { type = typeChosen, transport = transportChosen };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            typeTrips = TypeTripDAO.Get();
            transports = TransportDAO.Get();
            typeChosen = typeTrips[0];
            transportChosen = transports[0];

            statusChosen.Text = "Đang đi";

            this.DataContext = new { type = typeChosen, transport = transportChosen };
        }

        private void ChooseTransport_Button_Click(object sender, RoutedEventArgs e)
        {
            string idTransport = ((Button)sender).Tag.ToString();
            transportChosen = TransportDAO.GetById(idTransport);

            this.DataContext = new { type = typeChosen, transport = transportChosen };
        }

        private void imageUploadImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn ảnh bìa chuyến đi";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            op.Multiselect = false;
            var o = op.ShowDialog();
            if (o == true)
            {
                fileUploadPath = op.FileName;
                imageUploadImage.Source = new BitmapImage(new Uri(op.FileName));
                imageUploadImage.Tag = op.SafeFileName;

                nameFileTextblock.Text = op.SafeFileName;
            }
        }

        private void removeImage_Click(object sender, RoutedEventArgs e)
        {
            imageUploadImage.Source = new BitmapImage(new Uri("../Assets/Images/bg_upload.png", UriKind.Relative));
            nameFileTextblock.Text = "mac_dinh.png";
        }

        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            
            if(imageUploadImage.Tag.ToString() != "default_trip.jpg")
            {
                nameImage = imageUploadImage.Tag.ToString();

                // Copy file
                var folder = AppDomain.CurrentDomain.BaseDirectory;

                string newNameFile = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(nameImage);
                var targetPath = $"{folder}\\Assets\\Images\\Uploads\\";
                var destFile = System.IO.Path.Combine(targetPath, newNameFile);


                System.IO.File.Copy(fileUploadPath, destFile, true);

                // --
                nameImage = newNameFile;
                MessageBox.Show("Đã upload thành công", "Thông báo");
            }
        }

        private void addMemberButton_Click(object sender, RoutedEventArgs e)
        {
            
            var addMemberDialog = new AddMemberDialog();

            if (addMemberDialog.ShowDialog() == true)
            {
                var newMember = addMemberDialog.NewMember;
                members.Add(newMember);

                memberListView.ItemsSource = null;
                memberListView.ItemsSource = members;
            }
        }

        private void removeMemberButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chưa phát triển", "Thông báo");
        }

        private void editMemberButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chưa phát triển", "Thông báo");
        }

        private void addExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            var content = expenseContentTextbox.Text;
            var cost = expenseCostTextbox.Text;

            if(content == "" || cost == "")
            {
                MessageBox.Show("Dữ liệu không được để trống", "Lỗi");
                return;
            }

            Expense expenseEntered = new Expense { Cost = cost, Name = content, Id = Guid.NewGuid().ToString() };

            expenses.Add(expenseEntered);

            expensesListView.ItemsSource = null;
            expensesListView.ItemsSource = expenses;

        }

        private void cancelExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            expenseContentTextbox.Text = "";
            expenseCostTextbox.Text = "";
        }

        private void editExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chưa phát triển", "Thông báo");
        }

        private void removeExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chưa phát triển", "Thông báo");
        }

        private void progressStatusButton_Click(object sender, RoutedEventArgs e)
        {
            status = "progress";
            statusChosen.Text = "Đang đi";
        }

        private void finishStatusButton_Click(object sender, RoutedEventArgs e)
        {
            status = "finish";
            statusChosen.Text = "Đã kết thúc";
        }

        
    }
}
