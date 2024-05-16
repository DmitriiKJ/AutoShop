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
    /// Interaction logic for SelectEmployee.xaml
    /// </summary>
    public partial class SelectEmployee : Window
    {
        public static SelectEmployee Window;
        AutoShopDB AutoShop;
        string from;
        public SelectEmployee(AutoShopDB db, string withoutLogin, bool isSchedule)
        {
            InitializeComponent();
            Window = this;
            AutoShop = db;
            from = withoutLogin;

            List<int> workers = AutoShop._dataSet.Tables["Managers"]
                   .AsEnumerable()
                   .Where(m => m.Field<bool?>("isFired") != true).Select(m => m.Field<int>("Id")).ToList();

            var filteredRows = AutoShop._dataSet.Tables["Access"]
            .AsEnumerable()
            .Where(a => workers.Contains(a.Field<int>("ManagerId")) && a.Field<string>("Login") != withoutLogin)
            .CopyToDataTable();

            login.DisplayMemberPath = "Login";
            login.ItemsSource = filteredRows.DefaultView;

            if (isSchedule) btnSelect.Click += btnLogin_ClickSchedule;
            else btnSelect.Click += btnLogin_ClickData;
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                SelectEmployee.Window.DragMove();
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

        private void btnLogin_ClickSchedule(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)login.SelectedItem;
            string login1 = selectedRow["Login"].ToString();
            Schedule schedule = new Schedule(AutoShop, login1);
            Close();
            schedule.ShowDialog();
        }

        private void Login_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSelect.IsEnabled = (login.SelectedIndex != -1);
        }

        private void btnLogin_ClickData(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)login.SelectedItem;
            string login1 = selectedRow["Login"].ToString();
            ChangeData changeData = new ChangeData(AutoShop, true, login1, from);
            Close();
            changeData.ShowDialog();
        }
    }
}
