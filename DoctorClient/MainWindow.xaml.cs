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
using AssistentClient.Models;
using DoctorClient;
using DoctorClient.ViewModels;
using DoctorClient.Views;

namespace DoctorClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            patientsList.Add(new Patient(1, "asd1", "addr1", "111", "Complaint1", "11111"));
            patientsList.Add(new Patient(2, "asd2", "addr2", "222", "Complaint2", "22222"));
            patientsList.Add(new Patient(3, "asd3", "addr3", "333", "Complaint3", "33333"));
            patientsList.Add(new Patient(4, "asd4", "addr4", "444", "Complaint4", "44444"));
            patientsList.Add(new Patient(5, "asd5", "addr5", "555", "Complaint5", "55555"));
            patientsList.Add(new Patient(6, "asd6", "addr6", "666", "Complaint6", "66666"));
            patientsList.Add(new Patient(7, "asd7", "addr7", "777", "Complaint7", "77777"));
            patientsList.Add(new Patient(7, "asd8", "addr8", "888", "Complaint8", "88888"));
            patientsList.Add(new Patient(7, "asd9", "addr9", "999", "Complaint9", "99999"));
            patientsList.Add(new Patient(7, "asd10", "addr10", "100", "Complaint10", "10000"));
            patientsList.Add(new Patient(7, "asd11", "addr11", "111", "Complaint11", "11000"));

            for (int i = 0; i < patientsList.Count; i++)
            {
                patients.Add(new Patient(patientsList[i].name));
            }

            bindListBox();
        }

        List<Patient> patientsList = new List<Patient>();
        List<Patient> patients = new List<Patient>();
        private string NameLabel = "";
        private string TajLabel;
        private string PhoneLabel;
        private string AddressLabel;
        private string ComplaintLabel;

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
                    NameLabel = patientsList[i].name.ToString();
                    TajLabel = patientsList[i].taj.ToString();
                    PhoneLabel = patientsList[i].phone.ToString();
                    AddressLabel = patientsList[i].address.ToString();
                    ComplaintLabel = patientsList[i].complaint.ToString();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NameLabel.Equals(""))
            {
                MessageBox.Show("Select an Item!");
                
            }
            else
            {
                ModifyView m = new ModifyView();

                m.Top = this.Top;
                m.Left = this.Left;
                m.NameLabel.Content = NameLabel;
                m.AddressLabel.Content = AddressLabel;
                m.TajLabel.Content = TajLabel;
                m.PhoneLabel.Content = PhoneLabel;
                m.ComplaintLabel.Content = ComplaintLabel;
                m.Show();
                this.Close();
            }

        }
    }
}
