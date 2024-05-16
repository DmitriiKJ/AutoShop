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
    /// Interaction logic for AddEngine.xaml
    /// </summary>
    public partial class AddEngine : Window
    {
        public static AddEngine Window;
        AutoShopDB AutoShop;
        int power = 300;
        public AddEngine(AutoShopDB db)
        {
            InitializeComponent();
            Window = this;
            numeric.Text = power.ToString();
            AutoShop = db;
            engineType.Items.Add("Електричний");
            engineType.Items.Add("Бензин");
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                AddEngine.Window.DragMove();
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
            DataRow row = AutoShop._dataSet.Tables["Engines"].NewRow();
            row["EngineModel"] = modelEngine.Text;
            row["TypeEngine"] = engineType.SelectedValue;
            row["Power"] = int.Parse(numeric.Text);

            AutoShop.AddEngine(row);
            Close();
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            if(power < 1000)
            {
                power++;
                numeric.Text = power.ToString();
            }
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            if (power > 100)
            {
                power--;
                numeric.Text = power.ToString();
            }
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(modelEngine.Text) && engineType.SelectedIndex != -1)
            {
                addEngine.IsEnabled = true;
            }
            else
            {
                addEngine.IsEnabled = false;
            }

            if(int.TryParse(numeric.Text, out power))
            {
                if (power > 1000) power = 1000;
                else if (power < 100) power = 100;
            }
            else
            {
                power = 300;
                numeric.Text = power.ToString();
            }
        }

        private void engineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextChanged(null, null);
        }
    }
}
