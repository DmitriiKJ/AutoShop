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
    /// Interaction logic for SellForm.xaml
    /// </summary>
    public partial class SellForm : Window
    {
        public static SellForm Window;
        AutoShopDB AutoShop;
        private string CurrentManagerLogin;
        List<Client> _clients;
        List<Car> _cars;
        public SellForm(AutoShopDB db, string login)
        {
            InitializeComponent();
            Window = this;
            AutoShop = db;
            CurrentManagerLogin = login;
            currentManager.Text = CurrentManagerLogin;

            AutoShop.UpdateAllDataSet();

            _clients = AutoShop._dataSet.Tables["Clients"].AsEnumerable().Select(c => new Client
            {
                Id = c.Field<int>("Id"),
                FullName = c.Field<string>("FirstName") + ' ' + c.Field<string>("LastName"),
            }).ToList();

            clients.ItemsSource = _clients;
            clients.DisplayMemberPath = nameof(Client.FullName);
            clients.SelectedValuePath = nameof(Client.Id);

            _cars = AutoShop._dataSet.Tables["Cars"].AsEnumerable().Where(c => c.Field<bool?>("isSold") == null || c.Field<bool?>("isSold") == false).Select(c => new Car
            {
                Id = c.Field<int>("Id"),
                Info = c.Field<string>("Model") + ' ' + AutoShop._dataSet.Tables["Models"].AsEnumerable().FirstOrDefault(m => c.Field<string>("Model") == m.Field<string>("Model")).Field<string>("Brand") + $"({c.Field<int>("Year")})"
            }).ToList();

            cars.ItemsSource = _cars;
            cars.DisplayMemberPath = nameof(Car.Info);
            cars.SelectedValuePath = nameof(Car.Id);
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                SellForm.Window.DragMove();
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

        private void addClient_Click(object sender, RoutedEventArgs e)
        {
            AddClient addClient = new AddClient(AutoShop);
            addClient.ShowDialog();
            _clients = AutoShop._dataSet.Tables["Clients"].AsEnumerable().Select(c => new Client
            {
                Id = c.Field<int>("Id"),
                FullName = c.Field<string>("FirstName") + ' ' + c.Field<string>("LastName"),
            }).ToList();
            clients.ItemsSource = _clients;
        }

        private void addCar_Click(object sender, RoutedEventArgs e)
        {
            AddCar addCar = new AddCar(AutoShop);
            addCar.ShowDialog();

            _cars = AutoShop._dataSet.Tables["Cars"].AsEnumerable().Where(c => c.Field<bool?>("isSold") == null || c.Field<bool?>("isSold") == false).Select(c => new Car
            {
                Id = c.Field<int>("Id"),
                Info = c.Field<string>("Model") + ' ' + AutoShop._dataSet.Tables["Models"].AsEnumerable().FirstOrDefault(m => c.Field<string>("Model") == m.Field<string>("Model")).Field<string>("Brand") + $"({c.Field<int>("Year")})"
            }).ToList();

            cars.ItemsSource = _cars;
        }

        private void sell_Click(object sender, RoutedEventArgs e)
        {
            if(clients.SelectedIndex != -1 && cars.SelectedIndex != -1)
            {
                try
                {
                    DataRow row = AutoShop._dataSet.Tables["Buys"].NewRow();
                    row["ClientId"] = clients.SelectedValue;
                    row["CarId"] = cars.SelectedValue;
                    row["ManagerId"] = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == CurrentManagerLogin).Field<int>("ManagerId");
                    row["DateBuy"] = DateTime.Now;

                    AutoShop.Buy(row);

                    MessageBox.Show("Автомобіль успішно продано!");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Заповніть всі поля!");
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clients.SelectedIndex != -1 && cars.SelectedIndex != -1)
            {
                sell.IsEnabled = true;
            }
            else
            {
                sell.IsEnabled = false;
            }
        }
    }
}
