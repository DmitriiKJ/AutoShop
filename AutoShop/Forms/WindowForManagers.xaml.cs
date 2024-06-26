﻿using AutoShop.AdditionalClasses;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoShop.Forms
{
    /// <summary>
    /// Interaction logic for WindowForManagers.xaml
    /// </summary>
    public partial class WindowForManagers : Window
    {
        public static WindowForManagers Window;
        private AutoShopDB AutoShop;

        public WindowForManagers(string login, AutoShopDB db, bool isManager)
        {
            InitializeComponent();
            Window = this;
            AutoShop = db;
            loginCurrent.Text = login;

            ShowSellCars();

            if (isManager)
            {
                fire.IsEnabled = true;
                hire.IsEnabled = true;
                changePasswordForEmployee.IsEnabled = true;
                changeDataForEmployee.IsEnabled = true;
                showEmployeeSchedule.IsEnabled = true;
            }

            DataRow tmp = AutoShop._dataSet.Tables["WorkHistory"].AsEnumerable().FirstOrDefault(h => h.Field<DateTime?>("EndTime") == null && h.Field<int>("ManagerId") == AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == login).Field<int>("ManagerId"));

            if (tmp != null)
            {
                isWorking.IsChecked = true;
            }
            else
            {
                isWorking.IsChecked = false;
            }

            isWorking.Checked += IsWorking_Checked;
            isWorking.Unchecked += IsWorking_Unchecked;

            DataRow manager = AutoShop._dataSet.Tables["Managers"].AsEnumerable().FirstOrDefault(m => m.Field<int>("Id") == AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == login).Field<int>("ManagerId"));

            name.Text = manager.Field<string>("LastName") + ' ' + manager.Field<string>("FirstName");
            date.Text = manager.Field<DateTime>("Birthday").ToShortDateString();
            phone.Text = manager.Field<string>("PhoneNumber");
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                WindowForManagers.Window.DragMove();
            }
        }

        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            if (stack.Width == 0)
            {
                var slideIn = FindResource("SlideIn") as Storyboard;
                slideIn?.Begin(stack);
                var slideOut = FindResource("SlideOut") as Storyboard;
                slideOut?.Begin(table);
                info.Content = "Інформація";
            }
            else
            {
                var slideIn = FindResource("SlideIn") as Storyboard;
                slideIn?.Begin(table);
                var slideOut = FindResource("SlideOut") as Storyboard;
                slideOut?.Begin(stack);
                info.Content = "Можливості";
            }
        }
        private void ShowSellCars()
        {
            List<SellCar> list = new List<SellCar>();

            var buys = AutoShop._dataSet.Tables["Buys"].AsEnumerable().Where(b => b.Field<int>("ManagerId") == AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == loginCurrent.Text).Field<int>("ManagerId"));

            var carsId = buys.Select(b => b.Field<int>("CarId"));

            foreach (int id in carsId) 
            {
                list.Add(new SellCar
                {
                    Model = AutoShop._dataSet.Tables["Cars"].AsEnumerable().FirstOrDefault(c => c.Field<int>("Id") == id).Field<string>("Model"),

                    Brand = AutoShop._dataSet.Tables["Models"].AsEnumerable().FirstOrDefault(m => AutoShop._dataSet.Tables["Cars"].AsEnumerable().FirstOrDefault(c => c.Field<int>("Id") == id).Field<string>("Model") == m.Field<string>("Model")).Field<string>("Brand"),

                    dateSell = buys.FirstOrDefault(b => b.Field<int>("CarId") == id).Field<DateTime>("DateBuy")
                });
            }

            listSells.ItemsSource = list;
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

        private void changePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordForMe cpfm = new ChangePasswordForMe(loginCurrent.Text, AutoShop);
            cpfm.ShowDialog();
        }

        private void sellCar_Click(object sender, RoutedEventArgs e)
        {
            SellForm sellForm = new SellForm(AutoShop, loginCurrent.Text);
            sellForm.ShowDialog();
            ShowSellCars();
        }

        private void hire_Click(object sender, RoutedEventArgs e)
        {
            HireWindow hire = new HireWindow(AutoShop);
            hire.ShowDialog();
        }

        private void fire_Click(object sender, RoutedEventArgs e)
        {
            FireWindow fire = new FireWindow(AutoShop);
            fire.ShowDialog();
        }

        private void changePasswordForEmployee_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordForEmployee password = new ChangePasswordForEmployee(AutoShop, loginCurrent.Text);
            password.ShowDialog();
        }

        private void IsWorking_Checked(object sender, RoutedEventArgs e)
        {
            DataRow row = AutoShop._dataSet.Tables["WorkHistory"].NewRow();
            row["ManagerId"] = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == loginCurrent.Text).Field<int>("ManagerId");
            row["StartTime"] = DateTime.Now;
            row["EndTime"] = DBNull.Value;
            AutoShop.AddWork(row);
        }

        private void IsWorking_Unchecked(object sender, RoutedEventArgs e)
        {
            int id = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == loginCurrent.Text).Field<int>("ManagerId");

            DataRow row = AutoShop._dataSet.Tables["WorkHistory"].AsEnumerable().FirstOrDefault(h => h.Field<DateTime?>("EndTime") == null && h.Field<int>("ManagerId") == id);

            if (row != null)
            {
                AutoShop.UpdateWork(row);
            }
            else
            {
                MessageBox.Show("Цей працівник не працює!");
            }
        }

        private void showSchedule_Click(object sender, RoutedEventArgs e)
        {
            Schedule schedule = new Schedule(AutoShop, loginCurrent.Text);
            schedule.ShowDialog();
        }

        private void changeData_Click(object sender, RoutedEventArgs e)
        {
            ChangeData change = new ChangeData(AutoShop, false, loginCurrent.Text, loginCurrent.Text);
            change.ShowDialog();
            WindowForManagers window = new WindowForManagers(loginCurrent.Text, AutoShop, AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == loginCurrent.Text).Field<bool>("Level"));
            Close();
            window.ShowDialog();
        }

        private void ChangeDataForEmployee_Click(object sender, RoutedEventArgs e)
        {
            SelectEmployee select = new SelectEmployee(AutoShop, loginCurrent.Text, false);
            select.ShowDialog();
        }

        private void ShowEmployeeSchedule_Click(object sender, RoutedEventArgs e)
        {
            SelectEmployee select = new SelectEmployee(AutoShop, loginCurrent.Text, true);
            select.ShowDialog();
        }

        private void start_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (start.SelectedDate > end.SelectedDate) start.SelectedDate = end.SelectedDate;
            end.DisplayDateStart = start.SelectedDate;
        }

        private void end_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (end.SelectedDate < start.SelectedDate) end.SelectedDate = start.SelectedDate;
            start.DisplayDateEnd = end.SelectedDate;
        }

        private void find_Click(object sender, RoutedEventArgs e)
        {
            List<SellCar> list = new List<SellCar>();

            var buys = AutoShop._dataSet.Tables["Buys"].AsEnumerable().Where(b => b.Field<int>("ManagerId") == AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == loginCurrent.Text).Field<int>("ManagerId") && b.Field<DateTime>("DateBuy") >= start.SelectedDate && b.Field<DateTime>("DateBuy") <= end.SelectedDate);

            var carsId = buys.Select(b => b.Field<int>("CarId"));

            foreach (int id in carsId)
            {
                list.Add(new SellCar
                {
                    Model = AutoShop._dataSet.Tables["Cars"].AsEnumerable().FirstOrDefault(c => c.Field<int>("Id") == id).Field<string>("Model"),

                    Brand = AutoShop._dataSet.Tables["Models"].AsEnumerable().FirstOrDefault(m => AutoShop._dataSet.Tables["Cars"].AsEnumerable().FirstOrDefault(c => c.Field<int>("Id") == id).Field<string>("Model") == m.Field<string>("Model")).Field<string>("Brand"),

                    dateSell = buys.FirstOrDefault(b => b.Field<int>("CarId") == id).Field<DateTime>("DateBuy")
                });
            }

            listSells.ItemsSource = list;
        }

        private void all_Click(object sender, RoutedEventArgs e)
        {
            ShowSellCars();
        }

        private void historyOfChanges_Click(object sender, RoutedEventArgs e)
        {
            HistoryOfChanges history = new HistoryOfChanges(AutoShop, loginCurrent.Text);
            history.ShowDialog();
        }
    }
}
