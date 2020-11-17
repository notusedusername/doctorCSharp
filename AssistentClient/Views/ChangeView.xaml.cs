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
using System.Net.Http;

namespace AssistentClient.Views
{
    /// <summary>
    /// Interaction logic for ChangeView.xaml
    /// </summary>
    public partial class ChangeView : UserControl
    {
        public  ChangeView()
        {
            InitializeComponent();
            getDatas();
            //patientsList.Add(new Patient("asd1", "addr1", "111", "11111"));
            //patientsList.Add(new Patient("asd2", "addr2", "222", "22222"));
            //patientsList.Add(new Patient("asd3", "addr3", "333", "33333"));
            //patientsList.Add(new Patient("asd4", "addr4", "444", "44444"));
            //patientsList.Add(new Patient("asd5", "addr5", "555", "55555"));
            //patientsList.Add(new Patient("asd6", "addr6", "666", "66666"));
            //patientsList.Add(new Patient("asd7", "addr7", "777", "77777"));
            //patientsList.Add(new Patient("asd8", "addr8", "888", "88888"));
            //patientsList.Add(new Patient("asd9", "addr9", "999", "99999"));
            //patientsList.Add(new Patient("asd10", "addr10", "100", "10000"));
            //patientsList.Add(new Patient("asd11", "addr11", "111", "11000"));

            //for (int i = 0; i < patientsList.Count; i++)
            //{
            //    patients.Add(new Patient(patientsList[i].name));
            //}

        }
        List<Patient> patientsList = new List<Patient>();
        List<Patient> patients = new List<Patient>();
        static readonly HttpClient client = new HttpClient();

        private void bindListBox()
        {
            ItemListBox.ItemsSource = patients;

        }
        public async void getDatas()
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:52218/api/patient");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);
            string[] sp = responseBody.ToString().Split(new string[] { "[", "]", "{", "}", ",", ":", "\"" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < sp.Length; i += 10)
            {
                patientsList.Add(new Patient(sp[i + 3], sp[i + 5], sp[i + 7], sp[i + 9]));
            }
            for (int i = 0; i < patientsList.Count; i++)
            {
                patients.Add(new Patient(patientsList[i].name));
            }
            bindListBox();
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

        private void SearchIDButton_Click(object sender, RoutedEventArgs e)
        {
            SearchIDTextBox.Text = "";
        }

        private void SearchNameButton_Click(object sender, RoutedEventArgs e)
        {
            SearchNameTextBox.Text = "";
        }
    }
}
