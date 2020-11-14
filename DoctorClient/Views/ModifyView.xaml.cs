﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DoctorClient.Views
{
    /// <summary>
    /// Interaction logic for ModifyView.xaml
    /// </summary>
    public partial class ModifyView : Window
    {
        public ModifyView()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Top = this.Top;
            m.Left = this.Left;
            m.Show();
            this.Close();
        }
    }
}