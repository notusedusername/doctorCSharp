using DoctorCSharp.Model.Items;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {
                var activeComplaint = new ActiveComplaint();
               activeComplaint.diagnosis = DiagnosticTextBox.Text;

                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("diagnosis", activeComplaint.diagnosis),
            });
                string id = IDLabel.ToString();
                var url = "http://localhost:52218/api/treatment/active/";
                url += int.Parse(id); 
                using var client = new HttpClient();

                var response = await client.PostAsync(url, content);

                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
                string[] sp = result.Split(":");
                string[] sp2 = sp[1].Split("\"");
                MessageBox.Show(result);
            }catch (Exception)
            {

            }


            MainWindow m = new MainWindow();
            m.Top = this.Top;
            m.Left = this.Left;
            m.Show();
            this.Close();*/
        }
    }
}
