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
using System.Net.Http;
using Newtonsoft.Json;
using AssistentClient.Models;

namespace AssistentClient.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                var patient = new Patient();
                patient.name = NameTextBox.Text;
                patient.phone = PhoneTextBox.Text;
                patient.taj = TajTextBox.Text;
                patient.address = AddressTextBox.Text;

                patient.validateEmpty();
                patient.validateName();
                patient.validateTAJ();
                patient.validateAddress();
                patient.validatePhone();

                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("name", patient.name),
                new KeyValuePair<string, string>("taJ_nr",patient.taj),
                new KeyValuePair<string, string>("address",patient.address),
                new KeyValuePair<string, string>("phone",patient.phone)
            });

                var url = "http://localhost:52218/api/patient";
                using var client = new HttpClient();

                var response = await client.PostAsync(url, content);

                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
                string[] sp = result.Split(":");
                string[] sp2 = sp[1].Split("\"");
                if (sp2[1].Equals("There is already a patient with this TAJ number!"))
                {
                    MessageBox.Show("There is already a patient with this TAJ number!");
                }
                else
                {
                    MessageBox.Show("New patient succesfully saved!");
                    NameTextBox.Text = "";
                    PhoneTextBox.Text = "";
                    AddressTextBox.Text = "";
                    TajTextBox.Text = "";
                }
            }
            catch (Exceptions.InvalidInputException ex)
            {
                MessageBox.Show(ex.message);
            }
        }

}
}
