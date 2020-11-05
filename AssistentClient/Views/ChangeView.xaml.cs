using AssistentClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AssistentClient.Models;

namespace AssistentClient.Views
{
    /// <summary>
    /// Interaction logic for ChangeView.xaml
    /// </summary>
    public partial class ChangeView : UserControl
    {
        public ChangeView()
        {
            InitializeComponent();

            patientsList.Add(new Patient(1, "asd1", "addr1", "111", "","11111"));
            patientsList.Add(new Patient(2, "asd2", "addr2", "222", "","22222"));
            patientsList.Add(new Patient(3, "asd3", "addr3", "333", "","33333"));
            patientsList.Add(new Patient(4, "asd4", "addr4", "444", "","44444"));
            patientsList.Add(new Patient(5, "asd5", "addr5", "555", "","55555"));
            patientsList.Add(new Patient(6, "asd6", "addr6", "666", "","66666"));
            patientsList.Add(new Patient(7, "asd7", "addr7", "777", "","77777"));
            patientsList.Add(new Patient(7, "asd8", "addr8", "888", "", "88888"));
            patientsList.Add(new Patient(7, "asd9", "addr9", "999", "", "99999"));
            patientsList.Add(new Patient(7, "asd10", "addr10", "100", "", "10000"));
            patientsList.Add(new Patient(7, "asd11", "addr11", "111", "", "11000"));

            for (int i = 0; i < patientsList.Count; i++)
            {
                patients.Add(new Patient(patientsList[i].name));
            }

            bindListBox();

        }
        List<Patient> patientsList = new List<Patient>();
        List<Patient> patients = new List<Patient>();

        private void bindListBox()
        {          
            ItemListBox.ItemsSource = patients;   
            
        }

        private void ItemListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < patientsList.Count; i++)
            {
                if (ItemListBox.SelectedItem.ToString().Equals(patientsList[i].name))
                {
                    NameLabel.Content = patientsList[i].name.ToString();
                    TajLabel.Content = patientsList[i].taj.ToString();
                    PhoneLabel.Content = patientsList[i].phone.ToString();
                    AddressLabel.Content = patientsList[i].address.ToString();
                }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            NameLabel.Content = "";
            TajLabel.Content = "";
            PhoneLabel.Content = "";
            AddressLabel.Content = "";
            ComplaintTextBox.Text = "";
        }
    }
}
