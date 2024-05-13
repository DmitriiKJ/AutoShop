using AutoShop.AdditionalClasses;
using AutoShop.ClassesDB;
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

namespace AutoShop.Forms
{
    /// <summary>
    /// Interaction logic for ChangePasswordForMe.xaml
    /// </summary>
    public partial class ChangePasswordForMe : Window
    {
        private AutoShopDB AutoShop;
        private string _login;
        public ChangePasswordForMe(string login, AutoShopDB db)
        {
            InitializeComponent();
            _login = login;
            AutoShop = db;
        }

        private void ChangeTheme(object sender, MouseButtonEventArgs e)
        {
            ResourceDictionary otherThemeDictionary = new ResourceDictionary();
            otherThemeDictionary.Source = new Uri("Styles/" + (sender as TextBlock).Tag.ToString() + "Style.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(otherThemeDictionary);
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            if (Password.ValidatePassword(newP.Text))
            {
                AutoShop.ChangePassword(newP.Text, _login, _login);
                Close();
            }
            else
            {
                MessageBox.Show("Пароль має складатися не менше ніж з 8 символів та містити великі літери, малі, спецсимволи! Також він може мати цифри та не може мати пробілів.");
            }
        }
    }
}
