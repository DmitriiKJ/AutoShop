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
    /// Interaction logic for FireWindow.xaml
    /// </summary>
    public partial class FireWindow : Window
    {
        public static FireWindow Window;
        AutoShopDB AutoShop;
        public FireWindow(AutoShopDB db)
        {
            InitializeComponent();
            Window = this;
            AutoShop = db;

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
                FireWindow.Window.DragMove();
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

        private void fire_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)login.SelectedItem;
                string login1 = selectedRow["Login"].ToString();

                DataRow row = AutoShop._dataSet.Tables["Managers"].AsEnumerable().FirstOrDefault(m => m.Field<int>("Id") == AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == login1).Field<int>("ManagerId"));

                AutoShop.FireManager(row);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }

        private void login_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(login.SelectedIndex != -1)
            {
                fire.IsEnabled = true;
            }
        }
    }
}
