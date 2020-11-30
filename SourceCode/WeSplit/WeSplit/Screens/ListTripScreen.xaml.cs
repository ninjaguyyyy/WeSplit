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
        private Paging tripsPaging;
        private const int ROW_PER_PAGE = 6;

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
            HandlePagingInfoForTrips();
            DisplayProducts();
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

        private void prevLVItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(tripsPaging.CurrentPage > 1)
            {
                tripsPaging.CurrentPage--;
                foreach (dynamic item in pagingListView.Items)
                {
                    if (item.Content.ToString() == (tripsPaging.CurrentPage + 1).ToString())
                    {
                        item.IsSelected = false;
                    }
                    if (item.Content.ToString() == tripsPaging.CurrentPage.ToString())
                    {
                        item.IsSelected = true;
                    }
                }
            }
            DisplayProducts();
            
            
        }

        private void nextLVItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (tripsPaging.CurrentPage < tripsPaging.TotalPages)
            {
                tripsPaging.CurrentPage++;
                foreach (dynamic item in pagingListView.Items)
                {
                    if (item.Content.ToString() == (tripsPaging.CurrentPage - 1).ToString())
                    {
                        item.IsSelected = false;
                    }
                    if (item.Content.ToString() == tripsPaging.CurrentPage.ToString())
                    {
                        item.IsSelected = true;
                    }
                }
            }
            DisplayProducts();
        }

        class PageInfo
        {
            public int Page { get; set; }
            public int TotalPages { get; set; }
        }

        class Paging
        {
            private int _totalPages;
            public int CurrentPage { get; set; }

            public int RowsPerPage { get; set; }

            public int TotalPages
            {
                get => _totalPages; set
                {
                    _totalPages = value;
                    Pages = new List<PageInfo>();
                    for (int i = 1; i <= _totalPages; i++)
                    {
                        Pages.Add(new PageInfo()
                        {
                            Page = i,
                            TotalPages = _totalPages
                        });
                    }
                }
            }

            public List<PageInfo> Pages { get; set; }

        }

        private void HandlePageClicked(object sender, RoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            tripsPaging.CurrentPage = int.Parse(item.Content.ToString());

            DisplayProducts();
        }

        void HandlePagingInfoForTrips()
        {
            var count = TripDAO.CountTrips();

            tripsPaging = new Paging
            {
                CurrentPage = 1,
                RowsPerPage = ROW_PER_PAGE,
                TotalPages = count / ROW_PER_PAGE +
                    (((count % ROW_PER_PAGE) == 0) ? 0 : 1)
            };

            
            foreach(var page in tripsPaging.Pages)
            {
                
                var pageListViewItem = new ListViewItem();
                pageListViewItem.Foreground = Brushes.White;
                Thickness padding = pageListViewItem.Padding;
                padding.Left = 13;
                padding.Right = 13;
                padding.Top = 8;
                padding.Bottom = 8;
                pageListViewItem.Padding = padding;

                pageListViewItem.Content = page.Page;
                pageListViewItem.PreviewMouseLeftButtonUp += HandlePageClicked;

                if (page.Page == 1)
                {
                    pageListViewItem.IsSelected = true;
                }

                pagingListView.Items.Add(pageListViewItem);
            }

            var nextListViewItem = new ListViewItem();
            nextListViewItem.Content = "next  ⏩";
            Thickness paddingForNext = nextListViewItem.Padding;
            paddingForNext.Left = 13;
            paddingForNext.Right = 20;
            paddingForNext.Top = 8;
            paddingForNext.Bottom = 8;
            nextListViewItem.Padding = paddingForNext;
            nextListViewItem.Focusable = false;
            nextListViewItem.Foreground = Brushes.White;
            nextListViewItem.PreviewMouseLeftButtonUp += nextLVItem_PreviewMouseLeftButtonUp;
            pagingListView.Items.Add(nextListViewItem);


        }

        void DisplayProducts()
        {
            var fetchedTrips = TripDAO.GetTrips(tripsPaging.RowsPerPage, tripsPaging.CurrentPage);
            tripsListView.ItemsSource = fetchedTrips;
        }

    }
}
