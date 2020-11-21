using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DoctorClient;
using DoctorClient.ViewModels;
using DoctorClient.Views;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DoctorCSharp.Model.Items;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.VisualStudio.PlatformUI;
using Newtonsoft.Json;
using Commons.Items;

namespace DoctorClient.ViewModels
{
    class DoctorClientViewModel
    {
        static readonly HttpClient client = new HttpClient();

        public ActiveComplaint selectedComplaint { get; set; }

        public Patient selectedPatientData { get; set; }

        public ICommand SelectPatientCommand { get; }
        
        public ICommand RefreshWaitingPatientsCommand { get; }

        public ICommand PostDiagnosisCommand { get; }

        public ICommand UpdatePatientDataCommand { get; }

        public ICommand DeletePatientAllDataCommand { get; }

        public ObservableCollection<ActiveComplaint> patients { get; }

        public DoctorClientViewModel()
        {
            patients = new ObservableCollection<ActiveComplaint>();
            SelectPatientCommand = new DelegateCommand(SelectPatient);
            PostDiagnosisCommand = new DelegateCommand(PostDiagnosis);
            UpdatePatientDataCommand = new DelegateCommand(UpdatePatient);
            DeletePatientAllDataCommand = new DelegateCommand(DeletePatient);
            RefreshWaitingPatientsCommand = new DelegateCommand(RefreshWaitingPatientList);
            RefreshWaitingPatientList();
        }

        private async void RefreshWaitingPatientList()
        {
            HttpResponseMessage response;
            try
            {
               response = await client.GetAsync("http://localhost:52218/api/treatment/active");
            } catch(Exception e)
            {
                handleHttpExceptions(e);
                return;
            }
            
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                patients.Clear();
                JsonConvert.DeserializeObject<List<ActiveComplaint>>(responseBody).ForEach((item) => patients.Add(item));
            }
            else
            {
                ParseAndShowErrorResponseFromServer(response);
            }
        }

        private void SelectPatient()
        {
            
        }

        private void PostDiagnosis()
        {
            MessageBox.Show("asd");
        }

        private void UpdatePatient()
        {
            MessageBox.Show("asd");
        }

        private void DeletePatient()
        {
            MessageBox.Show("asd");
        }

        private void handleHttpExceptions(Exception e)
        {
            if (e is InvalidOperationException || e is ArgumentNullException)
            {
                Console.WriteLine("There were some errors with the structure of the request", e);
                MessageBox.Show("There are problems with the request...", "Beat the developer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (e is HttpRequestException)
            {
                Console.WriteLine("Can not connect to the server!");
                MessageBox.Show("Can not connect to the server!", "No internet? :/", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private async void ParseAndShowErrorResponseFromServer(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            jsonError jsonError = JsonConvert.DeserializeObject<jsonError>(responseBody);
            ShowErrorResponseFromServer(response, jsonError);

        }

        private void ShowErrorResponseFromServer(HttpResponseMessage response, jsonError jsonError)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show(jsonError.message, "Invalid value", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("The requested resource can not be found", "Not found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Something went wrong!", "The server is confused ...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /*

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
                    if (patientsDatas[i].id == activeComplaints[j].patient_id)
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
       
        }*/
    }
}
