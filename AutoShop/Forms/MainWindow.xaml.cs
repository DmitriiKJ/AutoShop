using AutoShop.ClassesDB;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoShop.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Window;
        private AutoShopDB AutoShop;
        public MainWindow()
        {
            InitializeComponent();
            Window = this;
            AutoShop = new AutoShopDB();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                MainWindow.Window.DragMove();
            }
        }

        private void MainManager_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow(true, AutoShop);
            lw.ShowDialog();
        }

        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow(false, AutoShop);
            lw.ShowDialog();
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

        private void history_Click(object sender, RoutedEventArgs e)
        {
            History history = new History(AutoShop);
            history.ShowDialog();
        }
    }
}
