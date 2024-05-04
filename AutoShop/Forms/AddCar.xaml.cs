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
    /// Interaction logic for AddCar.xaml
    /// </summary>
    public partial class AddCar : Window
    {
        AutoShopDB AutoShop;
        private int _year = 2000;
        List<Model> _models;

        public AddCar(AutoShopDB dB)
        {
            InitializeComponent();
            AutoShop = dB;
            AutoShop.UpdateAllDataSet();
            numeric.Text = _year.ToString();

            _models = AutoShop._dataSet.Tables["Models"].AsEnumerable().Select(m => new Model
            {
                ModelName = m.Field<string>("Model")
            }).ToList();

            model.ItemsSource = _models;
            model.DisplayMemberPath = nameof(Model.ModelName);
            model.SelectedValuePath = nameof(Model.ModelName);
        }

        private void ChangeTheme(object sender, MouseButtonEventArgs e)
        {
            ResourceDictionary otherThemeDictionary = new ResourceDictionary();
            otherThemeDictionary.Source = new Uri("Styles/" + (sender as TextBlock).Tag.ToString() + "Style.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(otherThemeDictionary);
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            if(_year > DateTime.Now.Year - 30)
            {
                _year--;
                numeric.Text = _year.ToString();
            }
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            if (_year < DateTime.Now.Year)
            {
                _year++;
                numeric.Text = _year.ToString();
            }
        }

        private void addModel_Click(object sender, RoutedEventArgs e)
        {
            AddModel add = new AddModel(AutoShop);
            add.ShowDialog();

            _models = AutoShop._dataSet.Tables["Models"].AsEnumerable().Select(m => new Model
            {
                ModelName = m.Field<string>("Model")
            }).ToList();
            model.ItemsSource = _models;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            DataRow row = AutoShop._dataSet.Tables["Cars"].NewRow();
            row["Model"] = model.SelectedValue;
            row["Year"] = _year;

            AutoShop.AddCar(row);

            MessageBox.Show("Автомобіль успішно додано!");
            Close();
        }

        private void model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (model.SelectedIndex != -1) add.IsEnabled = true;
            else add.IsEnabled = false;
        }
    }
}
