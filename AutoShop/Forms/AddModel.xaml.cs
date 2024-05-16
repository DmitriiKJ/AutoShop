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
    /// Interaction logic for AddModel.xaml
    /// </summary>
    public partial class AddModel : Window
    {
        public static AddModel Window;
        AutoShopDB AutoShop;
        List<Engine> _engines;
        List<string> _brands;
        public AddModel(AutoShopDB db)
        {
            InitializeComponent();
            Window = this;
            AutoShop = db;
            equipment.Items.Add("Економ");
            equipment.Items.Add("Стандарт");
            equipment.Items.Add("Преміум");

            _engines = AutoShop._dataSet.Tables["Engines"].AsEnumerable().Select(e => new Engine
            {
                EngineModel = e.Field<string>("EngineModel"),
                EngineType = e.Field<string>("TypeEngine"),
                Power = e.Field<int>("Power")
            }).ToList();

            _brands = AutoShop._dataSet.Tables["Brands"].AsEnumerable().Select(b => b.Field<string>("BrandName")).ToList();

            engine.ItemsSource = _engines;
            engine.DisplayMemberPath = "EngineModel";
            engine.SelectedValuePath = "EngineModel";

            brand.ItemsSource = _brands;
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                AddModel.Window.DragMove();
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

        private void addEngine_Click(object sender, RoutedEventArgs e)
        {
            AddEngine engineF = new AddEngine(AutoShop);
            engineF.ShowDialog();

            _engines = AutoShop._dataSet.Tables["Engines"].AsEnumerable().Select(en => new Engine
            {
                EngineModel = en.Field<string>("EngineModel"),
                EngineType = en.Field<string>("TypeEngine"),
                Power = en.Field<int>("Power")
            }).ToList();

            engine.ItemsSource = _engines;
        }

        private void addModel_Click(object sender, RoutedEventArgs e)
        {
            DataRow row = AutoShop._dataSet.Tables["Models"].NewRow();
            row["Model"] = model.Text;
            row["Equipment"] = equipment.Text;
            row["Price"] = Convert.ToDouble(price.Text);
            row["EngineModel"] = engine.SelectedValue;
            row["Brand"] = brand.SelectedValue;

            AutoShop.AddModel(row);
            Close();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            double tmp;
            if(!string.IsNullOrWhiteSpace(model.Text) && !string.IsNullOrWhiteSpace(price.Text) && double.TryParse(price.Text, out tmp) && equipment.SelectedIndex != -1 && engine.SelectedIndex != -1 && brand.SelectedIndex != -1)
            {
                addModel.IsEnabled = true;
            }
            else addModel.IsEnabled = false;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextChanged(null, null);
        }

        private void addBrand_Click(object sender, RoutedEventArgs e)
        {
            AddBrand brandW = new AddBrand(AutoShop);
            brandW.ShowDialog();

            _brands = AutoShop._dataSet.Tables["Brands"].AsEnumerable().Select(b => b.Field<string>("BrandName")).ToList();

            brand.ItemsSource = _brands;
        }
    }
}
