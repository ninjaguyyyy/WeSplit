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

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private List<SplashData> splashData = new List<SplashData>();
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            splashData = SplashDataDAO.GetAll();

            Random _rng = new Random();
            int indexRandom = _rng.Next(splashData.Count);
            nameText.Text = splashData[indexRandom].Name;
            desText.Text = splashData[indexRandom].Description;


            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var pathAbsolute = $"{folder}\\Assets\\Images\\Systems\\{splashData[indexRandom].BackgroundNameFile}";

            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(
                new Uri(
                   pathAbsolute, UriKind.Absolute));
            myBrush.ImageSource = image.Source;

            container.Background = myBrush;
            

        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveShowCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
