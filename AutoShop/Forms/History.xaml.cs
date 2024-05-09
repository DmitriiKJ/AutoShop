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
    /// Interaction logic for History.xaml
    /// </summary>
    public partial class History : Window
    {
        AutoShopDB AutoShop;
        public History(AutoShopDB db)
        {
            InitializeComponent();
            AutoShop = db;

            List<HistoryElement> history = new List<HistoryElement>();
            foreach(DataRow row in AutoShop._dataSet.Tables["Buys"].AsEnumerable())
            {
                DataRow manager = AutoShop._dataSet.Tables["Managers"].AsEnumerable().FirstOrDefault(m => m.Field<int>("Id") == row.Field<int>("ManagerId"));

                DataRow client = AutoShop._dataSet.Tables["Clients"].AsEnumerable().FirstOrDefault(c => c.Field<int>("Id") == row.Field<int>("ClientId"));

                DataRow car = AutoShop._dataSet.Tables["Cars"].AsEnumerable().FirstOrDefault(c => c.Field<int>("Id") == row.Field<int>("CarId"));

                history.Add(new HistoryElement
                {
                    ManagerFullName = manager.Field<string>("LastName") + ' ' + manager.Field<string>("FirstName") + ' ' + manager.Field<string>("MiddleName"),

                    ClientFullName = client.Field<string>("LastName") + ' ' + client.Field<string>("FirstName") + ' ' + client.Field<string>("MiddleName"),

                    CarInfo = car.Field<string>("Model") + ' ' + '(' + car.Field<int>("Year") + ')',

                    DateSell = row.Field<DateTime>("DateBuy")
                });
            }

            historyGrid.ItemsSource = history;
        }

        private void ChangeTheme(object sender, MouseButtonEventArgs e)
        {
            ResourceDictionary otherThemeDictionary = new ResourceDictionary();
            otherThemeDictionary.Source = new Uri("Styles/" + (sender as TextBlock).Tag.ToString() + "Style.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(otherThemeDictionary);
        }
    }
}
