using AutoShop.AdditionalClasses;
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
    /// Interaction logic for ChangePasswordForEmployee.xaml
    /// </summary>
    public partial class ChangePasswordForEmployee : Window
    {
        public static ChangePasswordForEmployee Window;
        AutoShopDB AutoShop;
        string log;
        public ChangePasswordForEmployee(AutoShopDB db, string loginManager)
        {
            InitializeComponent();
            Window = this;
            AutoShop = db;
            log = loginManager;

            try
            {
                List<int> workers = AutoShop._dataSet.Tables["Managers"]
                .AsEnumerable()
                .Where(m => m.Field<bool?>("isFired") != true).Select(m => m.Field<int>("Id")).ToList();

                var filteredRows = AutoShop._dataSet.Tables["Access"]
                .AsEnumerable()
                .Where(a => a.Field<bool>("Level") == false && workers.Contains(a.Field<int>("ManagerId")))
                .CopyToDataTable();

                login.DisplayMemberPath = "Login";
                login.ItemsSource = filteredRows.DefaultView;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ChangePasswordForEmployee.Window.DragMove();
            }
        }

        private void ChangeTheme(object sender, MouseButtonEventArgs e)
        {
            ResourceDictionary otherThemeDictionary = new ResourceDictionary();
            otherThemeDictionary.Source = new Uri("Styles/" + (sender as TextBlock).Tag.ToString() + "Style.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(otherThemeDictionary);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            if (Password.ValidatePassword(newP.Text))
            {
                DataRowView selectedRow = (DataRowView)login.SelectedItem;
                string login1 = selectedRow["Login"].ToString();

                AutoShop.ChangePassword(newP.Text, login1, log);
                Close();
            }
            else
            {
                MessageBox.Show("Пароль має складатися не менше ніж з 8 символів та містити великі літери, малі, спецсимволи! Також він може мати цифри та не може мати пробілів.");
            }
        }

        private void login_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(login.SelectedIndex != -1)
            {
                change.IsEnabled = true;
            }
        }
    }
}
