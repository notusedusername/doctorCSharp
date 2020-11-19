using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using DoctorClient.Models;
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
            start();
        }

        List<ActiveComplaint> activeComplaints = new List<ActiveComplaint>();
        List<Patient> patientsDatas = new List<Patient>();
        List<Patient> patients = new List<Patient>();
        List<Patient> patientstolistbox = new List<Patient>();
        static readonly HttpClient client = new HttpClient();
        public string name;
        public int id;
        public string address;
        public string phone;
        public string taj;

        private void bindListBox()
        {
            ItemListBox.ItemsSource = patientstolistbox;
        }
        public void start()
        {
            getDataComplaint();
        }
        public async void getDataComplaint()
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:52218/api/treatment/active");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            string[] sp = responseBody.ToString().Split(new string[] { "[", "]", "{", "}", ",", ":", "\"" }, StringSplitOptions.RemoveEmptyEntries);
            string[] sp2 = responseBody.ToString().Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries);
            int j = 0;
            for (int i = 0; i < sp.Length; i += 10)
            {
                ActiveComplaint ac = new ActiveComplaint();
                ac.id = int.Parse(sp[i + 1]);
                ac.patient_id = int.Parse(sp[i + 3]);
                ac.arrival = DateTime.Parse(sp2[j + 7]);
                ac.complaint = sp[i + 9].ToString();
                activeComplaints.Add(ac);
                j += 12;
            }
            activeComplaints = activeComplaints.OrderBy(x => x.arrival).ToList();
            getDataPatient();
        }
        public async void getDataPatient()
        {
            HttpResponseMessage response = await client.GetAsync("http://localhost:52218/api/patient");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            string[] sp = responseBody.ToString().Split(new string[] { "[", "]", "{", "}", ",", ":", "\"" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < sp.Length; i += 10)
            {
                patientsDatas.Add(new Patient(int.Parse(sp[i + 1]), sp[i + 3], sp[i + 5], sp[i + 7], sp[i + 9]));
            }
            getData();
        }
        public void getData()
        {
            for (int i = 0; i < patientsDatas.Count; i++)
            {
                for (int j = 0; j < activeComplaints.Count; j++)
                {
                    if(patientsDatas[i].id == activeComplaints[j].patient_id)
                    {
                        patients.Add(new Patient(patientsDatas[i].id, patientsDatas[i].name, patientsDatas[i].taj, patientsDatas[i].address, patientsDatas[i].phone));
                    }
                }
            }
            for (int i = 0; i < patients.Count; i++)
            {
                string p = patients[i].name + " | " + patients[i].id;
                patientstolistbox.Add(new Patient(p));
            }
            bindListBox();
        }

        private void ItemListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < patients.Count; i++)
            {
                if (ItemListBox.SelectedItem.ToString().Equals(patientstolistbox[i].name))
                {
                    name = patients[i].name.ToString();
                    taj = patients[i].taj.ToString();
                    phone = patients[i].phone.ToString();
                    address = patients[i].address.ToString();
                    id = patients[i].id;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (name.Equals(""))
            {
                MessageBox.Show("Select an Item!");       
            }
            else
            {
                ModifyView m = new ModifyView();

                for (int i = 0; i < activeComplaints.Count; i++)
                {
                    if (activeComplaints[i].id == id)
                    {
                        m.ComplaintLabel.Content = activeComplaints[i].complaint;
                    }
                }

                m.Top = this.Top;
                m.Left = this.Left;
                m.NameLabel.Content = name;
                m.AddressLabel.Content = address;
                m.TajLabel.Content = taj;
                m.PhoneLabel.Content = phone;
                m.IDLabel.Content = id;
                m.Show();
                this.Close();
            }
        }
    }
}
