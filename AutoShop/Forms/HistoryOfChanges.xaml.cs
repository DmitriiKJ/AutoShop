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
    /// Interaction logic for HistoryOfChanges.xaml
    /// </summary>
    public partial class HistoryOfChanges : Window
    {
        public static HistoryOfChanges Window;
        AutoShopDB AutoShop;
        public HistoryOfChanges(AutoShopDB db, string login)
        {
            InitializeComponent();
            Window = this;
            AutoShop = db;

            int id = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == login).Field<int>("ManagerId");

            var changes = AutoShop._dataSet.Tables["HistoryOfChanges"].AsEnumerable().Where(h => h.Field<int>("ManagerWhoChangedId") == id || h.Field<int>("ManagerWhoWasChanged") == id).ToList();

            List<HistoryOfChange> changesList = new List<HistoryOfChange>();
            foreach (var change in changes)
            {
                var tmp1 = AutoShop._dataSet.Tables["Managers"].AsEnumerable().FirstOrDefault(m => m.Field<int>("Id") == change.Field<int>("ManagerWhoChangedId"));

                var tmp2 = AutoShop._dataSet.Tables["Managers"].AsEnumerable().FirstOrDefault(m => m.Field<int>("Id") == change.Field<int>("ManagerWhoWasChanged"));

                changesList.Add(new HistoryOfChange
                {
                    ManagerWhoChanged = tmp1.Field<string>("LastName") + ' ' + tmp1.Field<string>("FirstName"),
                    ManagerWhoWasChanged = tmp2.Field<string>("LastName") + ' ' + tmp2.Field<string>("FirstName"),
                    DateChange = change.Field<DateTime>("DateChange"),
                    Info = change.Field<string>("Info")
                });
            }

            history.ItemsSource = changesList;
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

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                HistoryOfChanges.Window.DragMove();
            }
        }
    }
}
