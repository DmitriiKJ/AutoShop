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

            try
            {
                int count = AutoShop._dataSet.Tables["Managers"]
                            .AsEnumerable()
                            .Where(m => m.Field<bool?>("isFired") == true).Count();
                if (count > 0)
                {
                    List<int> ids = AutoShop._dataSet.Tables["Managers"]
                    .AsEnumerable()
                    .Where(m => m.Field<bool?>("isFired") == true).Select(m => m.Field<int>("Id")).ToList();

                    var workers = AutoShop._dataSet.Tables["Access"]
                    .AsEnumerable()
                    .Where(a => a.Field<bool>("Level") == false && ids.Contains(a.Field<int>("ManagerId")))
                    .CopyToDataTable();

                    restore.DisplayMemberPath = "Login";
                    restore.ItemsSource = workers.DefaultView;
                }
                else
                {
                    restore.Items.Add("Нема звільнених менеджерів");
                    restore.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void hire_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (newAdd.IsChecked == true)
                {

                }
                else
                {
                    DataRowView selectedRow = (DataRowView)restore.SelectedItem;
                    string login1 = selectedRow["Login"].ToString();

                    DataRow row = AutoShop._dataSet.Tables["Managers"].AsEnumerable().FirstOrDefault(m => m.Field<int>("Id") == AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == login1).Field<int>("ManagerId"));

                    AutoShop.RestoreManager(row);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
