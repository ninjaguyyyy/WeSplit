﻿using System;
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
using static WeSplit.Helpers.IEnum;

namespace WeSplit.Screens
{
    /// <summary>
    /// Interaction logic for ListTripScreen.xaml
    /// </summary>
    public partial class ListTripScreen : Window
    {
        private Paging tripsPaging;
        private const int ROW_PER_PAGE = 12;
        private StatusFilter statusFilter = StatusFilter.ALL;
        private string searchKey = "";
        private string modeSearch = "name_trip";

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
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            modeSearch = "name_trip";
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
            var detailTripScreen = new DetailTripScreen(id.ToString());
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
            var count = TripDAO.CountTrips(statusFilter, searchKey);

            tripsPaging = new Paging
            {
                CurrentPage = 1,
                RowsPerPage = ROW_PER_PAGE,
                TotalPages = count / ROW_PER_PAGE +
                    (((count % ROW_PER_PAGE) == 0) ? 0 : 1)
            };

            pagingListView.Items.Clear();

            var prevListViewItem = new ListViewItem();
            prevListViewItem.Content = "⏪  prev";
            Thickness paddingForNext = prevListViewItem.Padding;
            paddingForNext.Left = 20;
            paddingForNext.Right = 13;
            paddingForNext.Top = 8;
            paddingForNext.Bottom = 8;
            prevListViewItem.Padding = paddingForNext;
            prevListViewItem.Focusable = false;
            prevListViewItem.Foreground = Brushes.White;
            prevListViewItem.PreviewMouseLeftButtonUp += prevLVItem_PreviewMouseLeftButtonUp;
            pagingListView.Items.Add(prevListViewItem);

            foreach (var page in tripsPaging.Pages)
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
            Thickness paddingForPrev = nextListViewItem.Padding;
            paddingForPrev.Left = 13;
            paddingForPrev.Right = 20;
            paddingForPrev.Top = 8;
            paddingForPrev.Bottom = 8;
            nextListViewItem.Padding = paddingForPrev;
            nextListViewItem.Focusable = false;
            nextListViewItem.Foreground = Brushes.White;
            nextListViewItem.PreviewMouseLeftButtonUp += nextLVItem_PreviewMouseLeftButtonUp;
            pagingListView.Items.Add(nextListViewItem);


        }

        void DisplayProducts()
        {
            var fetchedTrips = TripDAO.GetTrips(tripsPaging.RowsPerPage, tripsPaging.CurrentPage, statusFilter, searchKey, modeSearch);
            tripsListView.ItemsSource = fetchedTrips;
        }

        private void allFilterButton_Click(object sender, RoutedEventArgs e)
        {
            statusFilter = StatusFilter.ALL;

            HandlePagingInfoForTrips();
            DisplayProducts();
        }

        private void progressFilterButton_Click(object sender, RoutedEventArgs e)
        {
            statusFilter = StatusFilter.PROGRESS;

            HandlePagingInfoForTrips();
            DisplayProducts();
        }

        private void finishFilterButton_Click(object sender, RoutedEventArgs e)
        {
            statusFilter = StatusFilter.FINISH;

            HandlePagingInfoForTrips();
            DisplayProducts();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchKey = searchTextBox.Text;

            HandlePagingInfoForTrips();
            DisplayProducts();
        }

        private void modeSearchComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            modeSearch = ((ComboBoxItem)modeSearchComboBox.SelectedItem).Tag?.ToString();
        }

        private void settingScreen_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Chưa phát triển", "Thông báo");
        }

        private void infoScreen_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Chưa phát triển", "Thông báo");
        }
    }
}
