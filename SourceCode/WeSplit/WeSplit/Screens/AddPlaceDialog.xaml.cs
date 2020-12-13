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
using WeSplit.DTO;

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for AddPlaceDialog.xaml
    /// </summary>
    public partial class AddPlaceDialog : Window
    {
        private string avatar_name = "food-store.png";
        private Place newPlace = new Place();

        internal Place NewPlace { get => newPlace; set => newPlace = value; }

        public AddPlaceDialog()
        {
            InitializeComponent();
            this.DataContext = new
            {
                Avatar_Name = avatar_name
            };
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }


        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string name = namePlaceTextbox.Text;
            string address = addressPlaceTextbox.Text;
            string description = actionTextbox.Text;
            var startDate = startPlacePicker.SelectedDate;
            var endDate = endPlacePicker.SelectedDate;

            if (name == "" || address == "")
            {
                MessageBox.Show("Chưa điền đầy đủ thông tin", "Lỗi");
                return;
            }


            NewPlace.Start = (startDate == null) ? "Chưa rõ" : ((DateTime)startDate).ToString("dd/MM");
            NewPlace.End = (endDate == null) ? "Chưa rõ" : ((DateTime)endDate).ToString("dd/MM");
            NewPlace.Name = name;
            NewPlace.Address = address;
            NewPlace.Description = description;
            NewPlace.Id = Guid.NewGuid().ToString();
            NewPlace.Avatar = avatar_name;

            DialogResult = true;

        }

        private void avatarPlaceChosen(object sender, MouseButtonEventArgs e)
        {
            var imageObject = sender as Image;
            avatar_name = imageObject.Uid;
            this.DataContext = new
            {
                Avatar_Name = avatar_name
            };
        }
    }
}
