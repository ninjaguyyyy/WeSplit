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
    /// Interaction logic for AddExpenseDialog.xaml
    /// </summary>
    public partial class AddExpenseDialog : Window
    {
        private Expense newExpense = new Expense();
        internal Expense NewExpense { get => newExpense; set => newExpense = value; }

        public AddExpenseDialog()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            string cost = costTextbox.Text;

            if (name == "" || cost == "")
            {
                MessageBox.Show("Dữ liệu không được để trống!", "Lỗi");
                return;
            }

            if (NewExpense.Id == null)
            {
                NewExpense.Id = Guid.NewGuid().ToString();
            }

            NewExpense.Name = name;
            NewExpense.Cost = cost;
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = NewExpense;
        }
    }
}
