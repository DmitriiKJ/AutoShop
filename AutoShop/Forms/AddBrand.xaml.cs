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
    /// Interaction logic for AddBrand.xaml
    /// </summary>
    public partial class AddBrand : Window
    {
        AutoShopDB AutoShop;
        public AddBrand(AutoShopDB db)
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

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(brandName.Text))
            {
                addBrand.IsEnabled = true;
            }
            else addBrand.IsEnabled = false;
        }

        private void addBrand_Click(object sender, RoutedEventArgs e)
        {
            DataRow row = AutoShop._dataSet.Tables["Brands"].NewRow();
            row["BrandName"] = brandName.Text;
            row["Info"] = info.Text;
            AutoShop.AddBrand(row);
            Close();
        }
    }
}
