using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Bunk_Manager.ViewModels;
using System.Windows.Media;

namespace Bunk_Manager
{
    public partial class AddSubject : PhoneApplicationPage
    {
        public AddSubject()
        {
            InitializeComponent();
        }
        public string ogname;
        public string[] colors = new string[10] { "CornflowerBlue", "DarkGoldenrod", "DarkOrange", "IndianRed", "LightCoral", "LightGreen", "SteelBlue", "Peru", "MediumSpringGreen", "LightSeaGreen" };
        
        public bool flag = false; 
        public string subjectname;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    //int index = int.Parse(selectedIndex);
                    //DataContext = App.ViewModel.Items[index];
                    subjectname = NavigationContext.QueryString["selectedItem"];
                    //Blah.Text = assignmentName;
                    //count++;
                    IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                    if (settings.Contains(subjectname))
                    {
                        var tempObj = settings[subjectname] as ItemViewModel;
                        SubName.Text = subjectname;
                        ogname = subjectname;
                        Total.Text = tempObj.Total.ToString();
                        Bunked.Text = tempObj.Bunked.ToString();
                        Minimum.Text = tempObj.Minimum.ToString();
                        flag = true;
                        DeleteButton.Visibility = Visibility.Visible;
                    }

                    
                }

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int attendance;
            String name = SubName.Text;
            int total = Convert.ToInt32(Total.Text);
            int bunked = Convert.ToInt32(Bunked.Text);
            int minimum = Convert.ToInt32(Minimum.Text);
            
            
            double tempMin = Convert.ToDouble(minimum);
            double tempTotal = Convert.ToDouble(total);
            //float.Parse((minimum* total)/100);
            //double safeBunk = Convert.ToDouble((total - bunked)-((tempMin*tempTotal)/100 ));
            //safeBunk = Math.Round(safeBunk);
            //if (safeBunk < 0)
             //   safeBunk = 0;
           // string backcolor = ((ListPickerItem)BackColorChoose.SelectedItem).Content.ToString();
            //Convert.ToDouble(attendance);
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if ((settings.Contains(name) && flag==false)|| name == "" )
            {
                MessageBox.Show("A subject with this name already exists");
            }

            else if (total <= 0 || bunked < 0 || minimum < 0 || minimum>100)
            {
                MessageBox.Show("Please check your input!");
            }
            
            else {
               // var color = new Color() { R = 0x34, G = 0x98, B = 0xdb };
               // var brush = new SolidColorBrush(color);
               // var hexcolor = brush.Color.ToString();
               // Color color = Color.FromArgb(1,52, 152, 219);
                //string back = color.ToString();
                // rgb(52, 152, 219) #3498db rgba(52, 152, 219,1.0)
                attendance = ((total - bunked) * 100 / (total));
                //int temptot = total;
                //int tempatt = attendance;
                //int att = total - bunked;
                //double safeBunk = -1;
                //while (tempatt > minimum) {
                //    safeBunk++;
                //    total++;
                //    tempatt = (att * 100 / (total));
                //}
                //if (safeBunk < 0)
                //    safeBunk = 0;
                double safeBunk = -1;
                int attended = total - bunked;
                safeBunk = calc(attendance,minimum,attended,total,bunked,safeBunk);
                if (safeBunk < 0)
                    safeBunk = 0;
                Random rnd = new Random();
                int colorindex = rnd.Next(0, 10);
                if(flag==false)
                settings.Add(name, new ItemViewModel() { ID = "1", SubName = name, Total = total, Bunked = bunked, BackColor=colors[colorindex],Attendance=attendance,Minimum=minimum, SafeBunk = safeBunk });
                else if (!settings.Contains(name)) {
                    settings.Remove(ogname);
                    settings.Add(name, new ItemViewModel() { ID = "1", SubName = name, Total = total, Bunked = bunked, BackColor = colors[colorindex], Attendance = attendance, Minimum = minimum, SafeBunk = safeBunk });

                }

                else
                {
                    var tempObj = settings[name] as ItemViewModel;
                    settings[name] = new ItemViewModel() { ID = "1", SubName = name, Total = total, Bunked = bunked, BackColor = tempObj.BackColor, Attendance = attendance, Minimum = minimum, SafeBunk = safeBunk };

                }
                    settings.Save();
                    NavigationService.GoBack();

            }
        }

        public double calc(int attendance, int minimum,int attended,int total,int bunked,double safeBunk){
            if (attendance >= minimum)
            {
                ++total;
                ++safeBunk;
                return calc(((attended * 100) / total), minimum, attended, total, bunked, safeBunk);
            }
            else  
                return safeBunk;
        }
        private void TotalMinus_Click(object sender, RoutedEventArgs e)
        {
            
            int totalTemp = Convert.ToInt32(Total.Text);
            totalTemp--;
            Total.Text = totalTemp.ToString();
        }

        private void TotalPlus_Click(object sender, RoutedEventArgs e)
        {
            
            int totalTemp = Convert.ToInt32(Total.Text);
            totalTemp++;
            Total.Text = totalTemp.ToString();
        }

        private void BunkedMinus_Click(object sender, RoutedEventArgs e)
        {
            int bunkedTemp = Convert.ToInt32(Bunked.Text);
            bunkedTemp--;
            Bunked.Text = bunkedTemp.ToString();
        }

        private void BunkPlus_Click(object sender, RoutedEventArgs e)
        {
            int bunkedTemp = Convert.ToInt32(Bunked.Text);
            bunkedTemp++;
            Bunked.Text = bunkedTemp.ToString();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string reference = SubName.Text;
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(reference))
            {
                settings.Remove(reference);
                settings.Save();

                NavigationService.GoBack();
            }
        }
    }
}