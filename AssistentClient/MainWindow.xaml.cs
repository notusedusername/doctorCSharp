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
using System.Net.Http;
using AssistentClient.Views;
using DoctorCSharp.Model.Items;
using Newtonsoft.Json;
using Commons.Items;

namespace AssistentClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //getDatas();
        }
    /*    

        List<Patient> patientsList = new List<Patient>();
        static readonly HttpClient client = new HttpClient();
        Patient p = new Patient();

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
           patientsList = JsonConvert.DeserializeObject<List<Patient>>(responseBody);
           bindList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           p = (Patient)dataGrid.SelectedItem;
        }

        private async void ComplaintButton_Click(object sender, RoutedEventArgs e)
        {
           if (p.name == null)
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

                   var url = "http://localhost:52218/api/treatment/active/"+ complaint.patient_id.ToString();

                   using var client = new HttpClient();
                   var response = await client.PostAsync(url, content);
                   string result = response.Content.ReadAsStringAsync().Result;
                   Console.WriteLine(result);

                   if (!response.IsSuccessStatusCode)
                   {
                       jsonError err = new jsonError();
                       err = JsonConvert.DeserializeObject<jsonError>(result);
                       MessageBox.Show(err.message) ;
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

        private async void filterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           patientsList.Clear();
           dataGrid.Items.Refresh();
           string filter = filterTextBox.Text.ToString();
           string url = "http://localhost:52218/api/patient?filter=" + filter;
           HttpResponseMessage response = await client.GetAsync(url);
           response.EnsureSuccessStatusCode();
           string responseBody = await response.Content.ReadAsStringAsync();
           patientsList = JsonConvert.DeserializeObject<List<Patient>>(responseBody);
           bindList();
        }
    */
    }
}
