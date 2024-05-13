using AutoShop.ClassesDB;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace AutoShop.Forms
{
    /// <summary>
    /// Interaction logic for ChangeData.xaml
    /// </summary>
    public partial class ChangeData : Window
    {
        AutoShopDB AutoShop;
        string _login;
        string _from;

        public ChangeData(AutoShopDB db, bool FromMain, string _l, string from)
        {
            InitializeComponent();
            _login = _l;
            AutoShop = db;
            _from = from;
            date.DisplayDateEnd = DateTime.Now.AddYears(-18);
            date.DisplayDateStart = DateTime.Now.AddYears(-100);

            if (FromMain)
            {
                m.Visibility = Visibility.Visible;
                isMainManager.Visibility = Visibility.Visible;
            }

            try
            {
                DataRow access = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == _login);
                DataRow manager = AutoShop._dataSet.Tables["Managers"].AsEnumerable().FirstOrDefault(m => m.Field<int>("Id") == access.Field<int>("ManagerId"));

                first.Text = manager.Field<string>("FirstName");
                last.Text = manager.Field<string>("LastName");
                middle.Text = manager.Field<string>("MiddleName");
                date.SelectedDate = manager.Field<DateTime>("Birthday");
                phoneNumber.Text = manager.Field<string>("PhoneNumber");
                isMainManager.IsChecked = access.Field<bool>("Level");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeTheme(object sender, MouseButtonEventArgs e)
        {
            ResourceDictionary otherThemeDictionary = new ResourceDictionary();
            otherThemeDictionary.Source = new Uri("Styles/" + (sender as TextBlock).Tag.ToString() + "Style.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(otherThemeDictionary);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
                if (!string.IsNullOrWhiteSpace(first.Text) && !string.IsNullOrWhiteSpace(last.Text) && !string.IsNullOrWhiteSpace(middle.Text) && !string.IsNullOrWhiteSpace(phoneNumber.Text) && date.SelectedDate != null)
                {
                    change.IsEnabled = true;
                }
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TextChanged(null, null);
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            int id = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == _login).Field<int>("ManagerId");
            DataRow row = AutoShop._dataSet.Tables["Managers"].AsEnumerable().FirstOrDefault(m => m.Field<int>("Id") == id);

            row["FirstName"] = first.Text;
            row["LastName"] = last.Text;
            row["MiddleName"] = middle.Text;
            row["PhoneNumber"] = phoneNumber.Text;
            row["Birthday"] = date.SelectedDate;

            bool tmp = isMainManager.IsChecked == true ? true : false;
            AutoShop.ChangeManagerData(row, tmp, _from);
            Close();
        }
    }
}
