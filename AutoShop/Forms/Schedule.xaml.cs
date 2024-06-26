﻿using AutoShop.AdditionalClasses;
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
    /// Interaction logic for Schedule.xaml
    /// </summary>
    public partial class Schedule : Window
    {
        public static Schedule Window;
        AutoShopDB AutoShop;
        public Schedule(AutoShopDB db, string _login)
        {
            InitializeComponent();
            Window = this;
            AutoShop = db;
            login.Text = _login;

            int id = AutoShop._dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(m => m.Field<string>("Login") == _login).Field<int>("ManagerId");

            List<Graph> grafs = new List<Graph>();
            foreach (var g in AutoShop._dataSet.Tables["WorkHistory"].AsEnumerable().Where(w => w.Field<int>("ManagerId") == id && w.Field<DateTime?>("EndTime") != null).GroupBy(w => w.Field<DateTime>("StartTime").ToShortDateString()))
            {
                Graph graf = new Graph { Date = g.Key, Time = TimeSpan.Zero };
                foreach (var s in g)
                {
                    graf.Time += new TimeSpan((s.Field<DateTime>("EndTime") - s.Field<DateTime>("StartTime")).Days, (s.Field<DateTime>("EndTime") - s.Field<DateTime>("StartTime")).Hours, (s.Field<DateTime>("EndTime") - s.Field<DateTime>("StartTime")).Minutes, (s.Field<DateTime>("EndTime") - s.Field<DateTime>("StartTime")).Seconds);
                }
                grafs.Add(graf);
            }
            schedule.ItemsSource = grafs;
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Schedule.Window.DragMove();
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
    }
}
