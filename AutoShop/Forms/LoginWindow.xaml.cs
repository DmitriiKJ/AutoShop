using AutoShop.AdditionalClasses;
using AutoShop.ClassesDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutoShop.Forms
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private Border border = new Border();
        private bool manager;
        private AutoShopDB AutoShop;
        private PasswordBox _pb;
        private TextBox _tb;
        public LoginWindow(bool isMainManager, AutoShopDB db)
        {
            // PasswodBox
            _pb = new PasswordBox();
            _pb.Margin = new Thickness(5);
            _pb.PasswordChar = '●';
            _pb.IsEnabled = false;
            _pb.Background = (Brush)Application.Current.TryFindResource("disable");
            _pb.BorderThickness = new Thickness(0);

            // TextPasswordBox
            _tb = new TextBox();
            _tb.Width = 250;
            _tb.Height = 25;
            _tb.Margin = new Thickness(5);
            Style myElementStyle = Application.Current.FindResource("textBox") as Style;
            _tb.Style = myElementStyle;
            _tb.IsEnabled = false;

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
            manager = isMainManager;

            try
            {
                AutoShop = db;
                if (isMainManager)
                {
                    List<int> workers = AutoShop._dataSet.Tables["Managers"]
                    .AsEnumerable()
                    .Where(m => m.Field<bool?>("isFired") != true).Select(m => m.Field<int>("Id")).ToList();

                    var filteredRows = AutoShop._dataSet.Tables["Access"]
                    .AsEnumerable()
                    .Where(a => a.Field<bool>("Level") == true && workers.Contains(a.Field<int>("ManagerId")))
                    .CopyToDataTable();

                    login.DisplayMemberPath = "Login";
                    login.ItemsSource = filteredRows.DefaultView;
                }
                else
                {
                    List<int> workers = AutoShop._dataSet.Tables["Managers"]
                    .AsEnumerable()
                    .Where(m => m.Field<bool?>("isFired") != true).Select(m => m.Field<int>("Id")).ToList();

                    var filteredRows = AutoShop._dataSet.Tables["Access"]
                    .AsEnumerable()
                    .Where(a => a.Field<bool>("Level") == false && workers.Contains(a.Field<int>("ManagerId")))
                    .CopyToDataTable();

                    login.DisplayMemberPath = "Login";
                    login.ItemsSource = filteredRows.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordBoxAssistant.SetBoundPassword(_pb, _tb.Text);
            MoveCursorToEnd(_pb);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBoxAssistant.SetBoundPassword(_pb, (sender as PasswordBox).Password);
            _tb.Text = PasswordBoxAssistant.GetBoundPassword(_pb);
        }

        private void MoveCursorToEnd(PasswordBox pb)
        {
            pb.GetType().GetMethod("Select", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(pb, new object[] { pb.Password.Length, 0 });
        }

        private void Login_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(login.SelectedItem != null)
            {
                _tb.IsEnabled = true;
                _pb.IsEnabled = true;
                _pb.Background = (Brush)Application.Current.TryFindResource("enable");
                border.Background = _pb.IsEnabled ? (Brush)Application.Current.TryFindResource("enable") : (Brush)Application.Current.TryFindResource("disable");
                btnLogin.IsEnabled = true;
            }
        }

        private void ChangeTheme(object sender, MouseButtonEventArgs e)
        {
            ResourceDictionary otherThemeDictionary = new ResourceDictionary();
            otherThemeDictionary.Source = new Uri("Styles/" + (sender as TextBlock).Tag.ToString() + "Style.xaml", UriKind.RelativeOrAbsolute);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(otherThemeDictionary);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (Password.ValidatePassword(_tb.Text))
                {
                    DataRowView selectedRow = (DataRowView)login.SelectedItem;
                    string login1 = selectedRow["Login"].ToString();

                    string salt = selectedRow["Salt"].ToString();

                    if (AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == login1).Field<string>("Password") == Password.SHA1(_tb.Text + salt))
                    {
                        this.Close();
                        WindowForManagers wfm = new WindowForManagers(login1, AutoShop, manager);
                        wfm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Неправильний пароль!");
                    }
                }
                else
                {
                    MessageBox.Show("Пароль має складатися не менше ніж з 8 символів та містити великі літери, малі, спецсимволи! Також він може мати цифри та не може мати пробілів.");
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
    }

    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPasswordProperty =
    DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBoundPasswordChanged));


        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.Password = (string)e.NewValue;
            }
        }
    }
}
