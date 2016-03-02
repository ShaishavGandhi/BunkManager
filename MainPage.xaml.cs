using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Bunk_Manager.Resources;
using Bunk_Manager.ViewModels;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using System.Windows.Data;
using Microsoft.Phone.Tasks;
using GoogleAds;

namespace Bunk_Manager
{
    
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        private InterstitialAd interstitialAd;
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;
            IsolatedStorageSettings settings1 = IsolatedStorageSettings.ApplicationSettings;
            //interstitialAd = new InterstitialAd("ca-app-pub-7421031935026273/2401558142");
            //AdRequest adRequest = new AdRequest();
           // adRequest.ForceTesting = true;
            //interstitialAd.ReceivedAd += OnAdReceived;
            //try { 
            //if(Convert.ToInt32(settings1["ratecount"])%2==0)
            //interstitialAd.LoadAd(adRequest);
            //}
            //catch{}

            if (settings1.Contains("ratecount") && !settings1.Contains("reviewed"))
            {
                int count = Convert.ToInt32(settings1["ratecount"]);
                if (count != -1)
                    count++;
                if (count % 5 == 0 && count != -1)
                {
                    //Add Dialog Code Here
                    // MessageBoxButton btn = new MessageBoxButton();

                    MessageBoxResult result = MessageBox.Show("Please take a moment to review this application. It means a lot to us :)", "Would you like to rate this application?", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

                        marketplaceReviewTask.Show();
                        settings1.Add("reviewed", true);
                        settings1.Save();
                    }


                }
                if (count == 5)
                    count = 0;

                settings1["ratecount"] = count;
                settings1.Save();
            }
            else
            {
                if (!settings1.Contains("ratecount"))
                    settings1.Add("ratecount", 0);
                settings1.Save();
            }
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items

        //private void OnAdReceived(object sender, AdEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("Ad received successfully");
        //    interstitialAd.ShowAd();
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            else
            {
                App.ViewModel.Items.Clear();
                App.ViewModel.LoadData();
                
            }
            if (App.ViewModel.Items.Count > 5)
            {
                ApplicationBar.Opacity = 1;
            }

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Count == 2)
            {
                MessageBox.Show("Wecome To Attendance Planner!\n\n Add a subject with the '+' button in the application bar.\n Enter details of your subject and bunk away!\n Once you do that, long tap on any subject to register an attended lecture, or press the bunk button to register a bunk lecture!\n\n Happy Bunking :)");

            }
        }

        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ItemViewModel).ID, UriKind.Relative));

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

      
     
        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var control = (sender as StackPanel);
            var children = control.Children;
            var text = children.ElementAt(0) as TextBlock;
            string name = text.Text;
            NavigationService.Navigate(new Uri("/AddSubject.xaml?selectedItem=" + name, UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddSubject.xaml", UriKind.Relative));
        }

        private void BunkButton_Click(object sender, RoutedEventArgs e)
        {
            var bunkButton = (sender as Button);
            var Parent = bunkButton.Parent;
            StackPanel sp = VisualTreeHelper.GetChild(Parent, 1) as StackPanel;
            TextBlock tp = VisualTreeHelper.GetChild(sp, 0) as TextBlock;
            String subname = tp.Text;

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(subname))
            {
                var tempObj = settings[subname] as ItemViewModel;
                int total = tempObj.Total+1;
                int bunked = tempObj.Bunked+1;
                int minimum = tempObj.Minimum;
                int attendance = ((total - bunked)*100 / (total));
                double safeBunk = -1;
                int attended = total-bunked;
                safeBunk = calc(attendance,minimum,attended,total,bunked,safeBunk,false);
                if (safeBunk < 0)
                    safeBunk = 0;
                settings[subname] = new ItemViewModel() { ID = tempObj.ID, SubName = tempObj.SubName, Total = tempObj.Total+1, Bunked = tempObj.Bunked+1, BackColor=tempObj.BackColor,Attendance=attendance,Minimum=tempObj.Minimum, SafeBunk = safeBunk };
                settings.Save();
               // MessageBox.Show("You bunked a lecture!\n Let's not make that a habit ;)");
                App.ViewModel.Items.Clear();
                App.ViewModel.LoadData();
            }
        }

        public double calc(int attendance, int minimum, int attended, int total, int bunked, double safeBunk,bool flag)
        {
            if (attendance >= minimum)
            {
                ++total;
                ++safeBunk;
                if(flag==true)
                return calc(((attended * 100) / total), minimum, attended, total, bunked, safeBunk,true);
                else
                    return calc(((attended * 100) / total), minimum, attended, total, ++bunked, safeBunk,false);
            }
            else
                return safeBunk;
        }
        private void StackPanel_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var control = (sender as StackPanel);
            var children = control.Children;
            var text = children.ElementAt(0) as TextBlock;
            string name = text.Text;

            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(name))
            {
                var tempObj = settings[name] as ItemViewModel;
                int total = tempObj.Total + 1;
                int bunked = tempObj.Bunked ;
                int attendance = ((total - bunked) * 100 / (total));
                int minimum = tempObj.Minimum;
                double safeBunk = -1;
                int attended = total - bunked;
                safeBunk = calc(attendance, minimum, attended, total, bunked, safeBunk,true);
                if (safeBunk < 0)
                    safeBunk = 0;
                settings[name] = new ItemViewModel() { ID = tempObj.ID, SubName = tempObj.SubName, Total = tempObj.Total + 1, Bunked = tempObj.Bunked , BackColor = tempObj.BackColor, Attendance = attendance, Minimum = tempObj.Minimum, SafeBunk = safeBunk };
                settings.Save();
                MessageBox.Show("You attended a lecture!");
                App.ViewModel.Items.Clear();
                App.ViewModel.LoadData();
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Support1.xaml", UriKind.Relative));
        }

       

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

       
    }

   
}