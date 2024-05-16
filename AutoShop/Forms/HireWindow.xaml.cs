using AutoShop.AdditionalClasses;
using AutoShop.ClassesDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
        public static HireWindow Window;
        AutoShopDB AutoShop;
        private PasswordBox _pb;
        private TextBox _tb;
        private Border border = new Border();

        public HireWindow(AutoShopDB db)
        {
            // PasswodBox
            _pb = new PasswordBox();
            _pb.Margin = new Thickness(5);
            _pb.PasswordChar = '●';
            _pb.Background = (Brush)Application.Current.TryFindResource("enable");
            _pb.BorderThickness = new Thickness(0);

            // TextPasswordBox
            _tb = new TextBox();
            _tb.Width = 250;
            _tb.Height = 25;
            _tb.Margin = new Thickness(5);
            Style myElementStyle = Application.Current.FindResource("textBox") as Style;
            _tb.Style = myElementStyle;
            _tb.IsEnabled = true;

            // Binding text and password
            _tb.TextChanged += TextBox_TextChanged;
            _pb.PasswordChanged += PasswordBox_PasswordChanged;

            border.Background = _pb.IsEnabled ? (Brush)Application.Current.TryFindResource("enable") : (Brush)Application.Current.TryFindResource("disable");
            border.CornerRadius = new CornerRadius(10);
            border.Width = 250;
            border.Height = 25;
            border.Child = _pb;
            border.Margin = new Thickness(5);

            InitializeComponent();
            Window = this;
            AutoShop = db;
            newAdd.Checked += new_Checked;
            date.DisplayDateEnd = DateTime.Now.AddYears(-18);
            date.DisplayDateStart = DateTime.Now.AddYears(-100);

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

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                HireWindow.Window.DragMove();
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBoxAssistant.SetBoundPassword(_pb, _tb.Text);
            MoveCursorToEnd(_pb);
            TextChanged(null, null);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBoxAssistant.SetBoundPassword(_pb, (sender as PasswordBox).Password);
            _tb.Text = PasswordBoxAssistant.GetBoundPassword(_pb);
            TextChanged(null, null);
        }

        private void MoveCursorToEnd(PasswordBox pb)
        {
            pb.GetType().GetMethod("Select", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(pb, new object[] { pb.Password.Length, 0 });
        }

        private void new_Checked(object sender, RoutedEventArgs e)
        {
            newEmployee.IsEnabled = true;
            restore.SelectedIndex = -1;
            restore.IsEnabled = false;
            _pb.IsEnabled = true;
            _pb.Background = (Brush)Application.Current.TryFindResource("enable");
            border.Background = _pb.IsEnabled ? (Brush)Application.Current.TryFindResource("enable") : (Brush)Application.Current.TryFindResource("disable");
            _tb.IsEnabled = true;

            TextChanged(null, null);
        }

        private void old_Ckecked(object sender, RoutedEventArgs e)
        {
            newEmployee.IsEnabled = false;
            restore.IsEnabled = true;
            _pb.IsEnabled = false;
            _pb.Background = (Brush)Application.Current.TryFindResource("disable");
            border.Background = _pb.IsEnabled ? (Brush)Application.Current.TryFindResource("enable") : (Brush)Application.Current.TryFindResource("disable");
            _tb.IsEnabled = false;

            SelectionChanged(null, null);
        }

        private void hire_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (newAdd.IsChecked == true)
                {
                    if (Password.ValidatePassword(_tb.Text))
                    {

                        DataRow manager = AutoShop._dataSet.Tables["Managers"].NewRow();
                        DataRow access = AutoShop._dataSet.Tables["Access"].NewRow();

                        manager["FirstName"] = first.Text;
                        manager["LastName"] = last.Text;
                        manager["MiddleName"] = middle.Text;
                        manager["Birthday"] = date.DisplayDate;
                        manager["Experience"] = 0;
                        manager["PhoneNumber"] = phoneNumber.Text;

                        access["Login"] = login.Text;
                        access["Password"] = _tb.Text;
                        access["Level"] = isMainManager.IsChecked;

                        AutoShop.AddManager(manager, access);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Пароль має складатися не менше ніж з 8 символів та містити великі літери, малі, спецсимволи! Також він може мати цифри та не може мати пробілів.");
                    }
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

        private void visiblePassword_Checked(object sender, RoutedEventArgs e)
        {
            Path path = new Path();
            path.Data = Geometry.Parse("M 20,20 C 25,10 35,10 40,20 C 35,30 25,30 20,20 Z");
            path.Fill = Brushes.White;
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;

            Ellipse ellipse = new Ellipse();
            ellipse.Width = 10;
            ellipse.Height = 10;
            ellipse.Fill = Brushes.Black;
            ellipse.Margin = new Thickness(20, 2, 0, 0);

            eye.Children.Clear();
            eye.Children.Add(path);
            eye.Children.Add(ellipse);

            passwordGrid.Children.Clear();
            passwordGrid.Children.Add(_tb);
        }

        private void visiblePassword_Unchecked(object sender, RoutedEventArgs e)
        {
            Path path = new Path();
            path.Data = Geometry.Parse("M 20,20 C 25,10 35,10 40,20 C 35,30 25,30 20,20 Z");
            path.Fill = Brushes.White;
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 1;

            Line line = new Line();
            line.X1 = 20;
            line.Y1 = 20;
            line.X2 = 40;
            line.Y2 = 20;
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 1;

            eye.Children.Clear();
            eye.Children.Add(path);
            eye.Children.Add(line);

            passwordGrid.Children.Clear();
            passwordGrid.Children.Add(border);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (newAdd.IsChecked == true)
            {
                if (!string.IsNullOrWhiteSpace(first.Text) && !string.IsNullOrWhiteSpace(last.Text) && !string.IsNullOrWhiteSpace(middle.Text) && !string.IsNullOrWhiteSpace(phoneNumber.Text) && !string.IsNullOrWhiteSpace(login.Text) && !string.IsNullOrWhiteSpace(_tb.Text) && date.SelectedDate != null)
                {
                    hire.IsEnabled = true;
                }
                else hire.IsEnabled = false;
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (restore.SelectedIndex != -1 && restore.SelectedItem.ToString() != "Нема звільнених менеджерів")
            {
                hire.IsEnabled = true;
            }
            else hire.IsEnabled = false;
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TextChanged(null, null);
        }
    }
}
