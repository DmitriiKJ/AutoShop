using AutoShop.ClassesDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        AutoShopDB AutoShop;
        public AddClient(AutoShopDB db)
        {
            InitializeComponent();
            AutoShop = db;
        }

        private void ChangeTheme(object sender, MouseButtonEventArgs e)
        {
            ResourceDictionary otherThemeDictionary = new ResourceDictionary();
            otherThemeDictionary.Source = new Uri("Styles/" + (sender as TextBlock).Tag.ToString() + "Style.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(otherThemeDictionary);
        }

        private void addClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Add
                DataRow row = AutoShop._dataSet.Tables["Clients"].NewRow();
                row["FirstName"] = firstName.Text;
                row["MiddleName"] = middleName.Text;
                row["LastName"] = lastName.Text;
                row["PhoneNumber"] = phoneNumber.Text;
                row["Address"] = address.Text;

                AutoShop.AddClient(row);
                MessageBox.Show("Кліент був доданий!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(firstName.Text) &&
                !string.IsNullOrWhiteSpace(lastName.Text) &&
                !string.IsNullOrWhiteSpace(middleName.Text) &&
                !string.IsNullOrWhiteSpace(phoneNumber.Text) &&
                !string.IsNullOrWhiteSpace(address.Text))
            {
                addClient.IsEnabled = true;
            }
            else
            {
                addClient.IsEnabled = false;
            }
        }
    }
}
