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
        public static AddCar Window;
        AutoShopDB AutoShop;
        private int _year = 2000;
        private int _count = 1;
        List<Model> _models;

        public AddCar(AutoShopDB dB)
        {
            InitializeComponent();
            Window = this;
            AutoShop = dB;
            AutoShop.UpdateAllDataSet();
            numeric.Text = _year.ToString();
            count.Text = _count.ToString();

            _models = AutoShop._dataSet.Tables["Models"].AsEnumerable().Select(m => new Model
            {
                ModelName = m.Field<string>("Model")
            }).ToList();

            model.ItemsSource = _models;
            model.DisplayMemberPath = nameof(Model.ModelName);
            model.SelectedValuePath = nameof(Model.ModelName);
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                AddCar.Window.DragMove();
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
            for (int i = 1; i <= _count; i++)
            {
                DataRow row = AutoShop._dataSet.Tables["Cars"].NewRow();
                row["Model"] = model.SelectedValue;
                row["Year"] = _year;

                AutoShop.AddCar(row);
            }

            
            if(_count == 1) MessageBox.Show("Автомобіль успішно додано!");
            else MessageBox.Show("Автомобілі успішно додано!");
            Close();
        }

        private void model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (model.SelectedIndex != -1) add.IsEnabled = true;
            else add.IsEnabled = false;
        }

        private void _up_Click(object sender, RoutedEventArgs e)
        {
            if (_count < 10)
            {
                _count++;
                count.Text = _count.ToString();
            }
        }

        private void _down_Click(object sender, RoutedEventArgs e)
        {
            if (_count > 1)
            {
                _count--;
                count.Text = _count.ToString();
            }
        }
    }
}
