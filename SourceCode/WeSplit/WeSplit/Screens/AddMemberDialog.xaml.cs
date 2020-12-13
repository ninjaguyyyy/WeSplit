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
    /// Interaction logic for AddMemberDialog.xaml
    /// </summary>
    public partial class AddMemberDialog : Window
    {
        public Member NewMember = new Member();
        private string avatarEntered = "man.png";

        public AddMemberDialog()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            string donate = donateTextbox.Text;

            if(name == "" || donate == "")
            {
                MessageBox.Show("Dữ liệu không được để trống!", "Lỗi");
                return;
            }

            NewMember.Id = Guid.NewGuid().ToString();
            NewMember.Name = name;
            NewMember.Donation = donate;
            NewMember.Avatar = avatarEntered;
            DialogResult = true;
        }

        private void manAvatarImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            displayNameAvatar.Text = "avatar_man.png";
            avatarEntered = "man.png";
        }

        private void womanAvatarImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            displayNameAvatar.Text = "avatar_woman.png";
            avatarEntered = "woman.png";
        }
    }
}
