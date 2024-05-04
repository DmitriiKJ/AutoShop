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
using System.Windows.Shapes;

namespace AutoShop.Forms
{
    /// <summary>
    /// Interaction logic for HireWindow.xaml
    /// </summary>
    public partial class HireWindow : Window
    {
        AutoShopDB AutoShop;
        public HireWindow(AutoShopDB db)
        {
            InitializeComponent();
            AutoShop = db;
            newAdd.Checked += new_Checked;
        }

        private void new_Checked(object sender, RoutedEventArgs e)
        {
            newEmployee.IsEnabled = true;
            restore.SelectedIndex = -1;
            restore.IsEnabled = false;
        }

        private void old_Ckecked(object sender, RoutedEventArgs e)
        {
            newEmployee.IsEnabled = false;
            restore.IsEnabled = true;
        }
    }
}
