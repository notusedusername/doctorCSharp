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
                patientsList.Add(new Patient(int.Parse(sp[i + 1]),sp[i + 3], sp[i + 5], sp[i + 7], sp[i + 9]));
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

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameLabel.Content.Equals(""))
            {
                MessageBox.Show("Patient is not selected!");
            }
            else if (ComplaintTextBox.Text.Equals(""))
            {
                MessageBox.Show("Complaint is neccesary to send!");
            }
            else
            {
                try
                {
                    int id = 0;
                    for (int i = 0; i < patientsList.Count; i++)
                    {
                        if (ItemListBox.SelectedItem.ToString().Equals(patientsList[i].name))
                        {
                            id = patientsList[i].id;
                        }
                    }
                    var complaint = new ActiveComplaint();
                    complaint.patient_id = id;
                    complaint.complaint = ComplaintTextBox.Text.ToString();


                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("complaint",complaint.complaint)
            });

                    var url = "http://localhost:52218/api/treatment/active";
                    url += "/" + complaint.patient_id.ToString();

                    using var client = new HttpClient();
                    var response = await client.PostAsync(url, content);
                    string result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);

                    string[] sp = result.Split(":");
                    string[] sp2 = sp[1].Split("\"");
                    if (sp2[1].Equals("The patient is waiting for treatment! You can not pick up new treatment while the older one is not closed."))
                    {
                        MessageBox.Show("The patient is waiting for treatment! You can not pick up new treatment while the older one is not closed.");
                    }
                    else
                    {
                        MessageBox.Show("Complaint is send to doctor!");
                        NameLabel.Content = "";
                        TajLabel.Content = "";
                        PhoneLabel.Content = "";
                        AddressLabel.Content = "";
                        ComplaintTextBox.Text = "";
                    }
                }
                catch (Exceptions.InvalidInputException ex)
                {
                    MessageBox.Show(ex.message);
                }
            }
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
