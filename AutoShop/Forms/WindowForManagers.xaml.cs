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
    /// Interaction logic for WindowForManagers.xaml
    /// </summary>
    public partial class WindowForManagers : Window
    {
        private AutoShopDB AutoShop;

        public WindowForManagers(string login, AutoShopDB db, bool isManager)
        {
            InitializeComponent();
            AutoShop = db;
            loginCurrent.Text = login;

            ShowSellCars();

            if (isManager)
            {
                fire.IsEnabled = true;
                hire.IsEnabled = true;
                changePasswordForEmployee.IsEnabled = true;
            }

            //IsWorking.Checked -= IsWorking_Checked;
            //IsWorking.Unchecked -= IsWorking_Unchecked;

            //DataRow tmp = AutoShop._dataSet.Tables["WorkHistory"].AsEnumerable().FirstOrDefault(h => h.Field<DateTime?>("EndTime") == null && h.Field<int>("ManagerId") == AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == login).Field<int>("ManagerId"));

            //if(tmp != null)
            //{
            //    IsWorking.IsChecked = true;
            //}
            //else
            //{
            //    IsWorking.IsChecked = false;
            //}

            //IsWorking.Checked += IsWorking_Checked;
            //IsWorking.Unchecked += IsWorking_Unchecked;
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
            ChangePasswordForEmployee password = new ChangePasswordForEmployee(AutoShop);
            password.ShowDialog();

        }

        //private void IsWorking_Checked(object sender, RoutedEventArgs e)
        //{
        //    DataRow row = AutoShop._dataSet.Tables["WorkHistory"].NewRow();
        //    row["ManagerId"] = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == LoginCurrent.Text).Field<int>("ManagerId");
        //    row["StartTime"] = DateTime.Now;
        //    row["EndTime"] = DBNull.Value;
        //    AutoShop.AddWork(row);
        //}

        //private void IsWorking_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    int id = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == LoginCurrent.Text).Field<int>("ManagerId");

        //    DataRow row = AutoShop._dataSet.Tables["WorkHistory"].AsEnumerable().FirstOrDefault(h => h.Field<DateTime?>("EndTime") == null && h.Field<int>("ManagerId") == id);

        //    if(row != null)
        //    {
        //        AutoShop.UpdateWork(row);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Цей працівник не працює!");
        //    }
        //}
    }
}
