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
using AssistentClient.ViewModels;

namespace AssistentClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new HomeViewModel();
        }
        private void ChangeView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new ChangeViewModel();
        }

        private void HomeView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new HomeViewModel();
        }
    }
}
