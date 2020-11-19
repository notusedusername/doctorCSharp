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
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AssistentClient.Views;

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
            getDatas();
        }
        List<Patient> patientsList = new List<Patient>();
        static readonly HttpClient client = new HttpClient();
        Patient p;

        private void bindList()
        {
            dataGrid.ItemsSource = patientsList;
            dataGrid.Items.Refresh();
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
                patientsList.Add(new Patient(int.Parse(sp[i + 1]), sp[i + 3], sp[i + 7], sp[i + 5], sp[i + 9]));
            }
            bindList();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            patientsList.Clear();
            dataGrid.Items.Refresh();
            string filter = filterTextBox.Text.ToString();
            string url = "http://localhost:52218/api/patient?filter=" + filter;
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);
            string[] sp = responseBody.ToString().Split(new string[] { "[", "]", "{", "}", ",", ":", "\"" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < sp.Length; i += 10)
            {
                patientsList.Add(new Patient(int.Parse(sp[i + 1]), sp[i + 3], sp[i + 7], sp[i + 5], sp[i + 9]));
            }
            bindList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            p = (Patient)dataGrid.SelectedItem;
        }
        private async void ComplaintButton_Click(object sender, RoutedEventArgs e)
        {
            if (p.name.Equals(null))
            {
                MessageBox.Show("Patient is not selected!");
            }
            else if (complaintTextBox.Text.Equals(""))
            {
                MessageBox.Show("Complaint is neccesary to send!");
            }
            else
            {
                try
                {
                    var complaint = new ActiveComplaint();
                    complaint.patient_id = p.id;
                    complaint.complaint = complaintTextBox.Text.ToString();


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
                        complaintTextBox.Text = "";
                    }
                }
                catch (Exceptions.InvalidInputException ex)
                {
                    MessageBox.Show(ex.message);
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow m = new RegisterWindow();
            m.Top = this.Top;
            m.Left = this.Left;
            m.Show();
            this.Close();
        }
        private async void SearchAllButton_Click(object sender, RoutedEventArgs e)
        {
            filterTextBox.Text = "";
            patientsList.Clear();
            dataGrid.Items.Refresh();
            string filter = filterTextBox.Text.ToString();
            string url = "http://localhost:52218/api/patient?filter=" + filter;
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);
            string[] sp = responseBody.ToString().Split(new string[] { "[", "]", "{", "}", ",", ":", "\"" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < sp.Length; i += 10)
            {
                patientsList.Add(new Patient(int.Parse(sp[i + 1]), sp[i + 3], sp[i + 7], sp[i + 5], sp[i + 9]));
            }
            bindList();
        }
    }
}
